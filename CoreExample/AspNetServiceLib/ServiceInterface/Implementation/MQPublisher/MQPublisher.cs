using AspNetServiceLib.DataStructures;
using AspNetServiceLib.Exceptions;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Diagnostics;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQPublisher
{
    internal class MQPublisher : IMQPublisher
    {
        private readonly IConnection connection;
        private readonly ServiceCall serviceCall;
        private readonly bool waitForDelivery;
        private readonly string exchangeType;
        private readonly bool usePublicQueue;
        private readonly bool usesReplies;
        private readonly IReplyHandlerFactory replyHandlerFactory;
        private readonly SemaphoreSlim accessMutex = new SemaphoreSlim(1);

        private IModel channel;
        private string exchangeName;
        private bool isDisposed;
        private IReplyHandler replyHandler;
        private bool hasBeenReturned = false;

        private static readonly ILogger logger = Logging.Factory.CreateLogger<MQPublisher>();

        public MQPublisher(
            IConnection connection,
            ServiceCall serviceCall,
            MQPublisherParams publisherParams,
            IReplyHandlerFactory replyHandlerFactory)
        {
            this.connection = connection;
            this.serviceCall = serviceCall;
            waitForDelivery = publisherParams.WaitForDelivery;
            exchangeType = publisherParams.ExchangeType;
            usePublicQueue = publisherParams.UsePublicQueue;
            usesReplies = publisherParams.UsesReplies;
            this.replyHandlerFactory = replyHandlerFactory;
        }

        public void Dispose()
        {
            isDisposed = true;
            replyHandler?.Dispose();

            if (channel != null)
            {
                try
                {
                    channel.Close();
                    channel.Dispose();
                }
                catch (AlreadyClosedException) { /* Ignore */ }
            }

            accessMutex.Dispose();
        }

        public async Task<ServiceMessage> PublishAsync(ServiceMessage message, TimeSpan timeout, CancellationToken ct)
        {
            if (isDisposed)
            {
                throw new MQPublisherDisposedException(serviceCall.InterfaceName, serviceCall.ServiceCallName);
            }

            logger.LogDebug("Performing service interface call [{interfaceName}.{callName}] using descriptors [{descriptors}] with timeout of [{timeout}].",
                serviceCall.InterfaceName, serviceCall.ServiceCallName, serviceCall.Descriptors.ToString(), timeout);

            IReplyWaiter replyWaiter = null;

            try
            {
                await accessMutex.WaitAsync(ct);    //Only one delivery through the same channel at the same time
                try
                {
                    if (isDisposed)
                    {
                        throw new MQPublisherDisposedException(serviceCall.InterfaceName, serviceCall.ServiceCallName);
                    }

                    EnsureChannel();

                    var props = channel.CreateBasicProperties();
                    props.ContentType = message.MimeType;
                    if (timeout == TimeSpan.Zero)
                    {
                        timeout = Timeout.InfiniteTimeSpan;
                    }
                    else
                    {
                        props.Expiration = timeout.TotalMilliseconds.ToString();
                    }

                    if (usesReplies)
                    {
                        replyWaiter = EstablishReplyHandler().PrepareReplyWaiter();

                        //Prepare result waiting:
                        props.CorrelationId = replyWaiter.CorrelationId;
                        props.ReplyTo = replyWaiter.ReplyQueueName;
                    }

                    hasBeenReturned = false;
                    //Send messages and wait for delivery ack (if used). These are blocking calls, therefore putting in thread pool:
                    var deliveryException = await Task.Run(() =>
                    {
                        try
                        {
                            channel.BasicPublish(
                                exchange: exchangeName,
                                routingKey: MQConventionHelper.GetRoutingKey(serviceCall.Descriptors),
                                basicProperties: props,
                                body: message.MessageData,
                                mandatory: waitForDelivery);
                            ct.ThrowIfCancellationRequested();
                            WaitForDelivery(message, timeout);
                        }
                        catch (Exception ex)
                        {
                            //Pass back the exception as return value instead of throwing it in Task to avoid VS from interrupting for no good reason
                            return ex;
                        }
                        return null;
                    }, ct);
                    if (deliveryException != null)
                    {
                        throw deliveryException;
                    }
                }
                finally
                {
                    accessMutex.Release();
                }

                ct.ThrowIfCancellationRequested();

                //Wait for reply:
                if (replyWaiter != null)
                {
                    logger.LogDebug("Waiting for reply from service interface call [{interfaceName}.{callName}] using descriptors [{descriptors}].",
                        serviceCall.InterfaceName, serviceCall.ServiceCallName, serviceCall.Descriptors.ToString());
                    var reply = await replyWaiter.WaitForReplyAsync(timeout, ct);
                    logger.LogDebug("Received reply from service interface call [{interfaceName}.{callName}] using descriptors [{descriptors}].",
                        serviceCall.InterfaceName, serviceCall.ServiceCallName, serviceCall.Descriptors.ToString());
                    return reply;
                }
            }
            catch (ObjectDisposedException)
            {
                replyWaiter = null;
                throw new MQPublisherDisposedException(serviceCall.InterfaceName, serviceCall.ServiceCallName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error performing service interface call [{interfaceName}.{callName}] using descriptors [{descriptors}]: {errorMessage}",
                    serviceCall.InterfaceName, serviceCall.ServiceCallName, serviceCall.Descriptors.ToString(), ex.Message);
                throw;
            }
            finally
            {
                if (replyWaiter != null)
                {
                    try
                    {
                        await accessMutex.WaitAsync(ct);
                        try
                        {
                            replyWaiter.Dispose();
                        }
                        finally
                        {
                            accessMutex.Release();
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        throw new MQPublisherDisposedException(serviceCall.InterfaceName, serviceCall.ServiceCallName);
                    }
                }
            }

            logger.LogDebug("Successfully performed service interface call [{interfaceName}.{callName}] using descriptors [{descriptors}].",
                serviceCall.InterfaceName, serviceCall.ServiceCallName, serviceCall.Descriptors.ToString());
            return null;
        }

        private IReplyHandler EstablishReplyHandler()
        {
            if (replyHandler != null)
            {
                return replyHandler;
            }

            Debug.Assert(channel != null);
            replyHandler = replyHandlerFactory.CreateReplyHandler(serviceCall, channel);
            return replyHandler;
        }

        private void EnsureChannel()
        {
            if (channel == null && connection != null)
            {
                channel = connection.CreateModel();
                exchangeName = MQConventionHelper.GetServiceCallExchangeName(serviceCall.InterfaceName, serviceCall.ServiceCallName);
                channel.ExchangeDeclare(exchangeName, exchangeType);
                if (waitForDelivery)
                {
                    channel.BasicReturn += Channel_BasicReturn;
                    channel.ConfirmSelect();
                }

                if (usePublicQueue)
                {
                    //Create public queue (can be used for load balancing etc...):
                    var declare = channel.QueueDeclare(
                        queue: MQConventionHelper.GetServiceCallPublicQueueName(
                            serviceCall.InterfaceName,
                            serviceCall.ServiceCallName,
                            serviceCall.Descriptors),
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var routingKey = MQConventionHelper.GetRoutingKey(serviceCall.Descriptors);
                    channel.QueueBind(declare.QueueName, exchangeName, routingKey);
                }
            }
        }

        private void WaitForDelivery(ServiceMessage message, TimeSpan timeout)
        {
            if (waitForDelivery && !isDisposed)
            {
                try
                {
                    channel.WaitForConfirmsOrDie(timeout);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex,
                        "Error while waiting for delivery confirmation for service interface call [{interfaceName}.{callName}] using descriptors [{descriptors}]: {error}",
                        serviceCall.InterfaceName, serviceCall.ServiceCallName, serviceCall.Descriptors.ToString(), ex.Message);
                    throw new CallNotDeliverableException(serviceCall.InterfaceName, serviceCall.ServiceCallName, message, ex);
                }

                if (hasBeenReturned)
                {
                    logger.LogError(
                        "Message not deliverable for service interface call [{interfaceName}.{callName}] using descriptors [{descriptors}].",
                        serviceCall.InterfaceName, serviceCall.ServiceCallName, serviceCall.Descriptors.ToString());
                    throw new CallNotDeliverableException(serviceCall.InterfaceName, serviceCall.ServiceCallName, message);
                }
            }
        }

        private void Channel_BasicReturn(object sender, BasicReturnEventArgs e)
        {
            logger.LogError(
                "Published message for service interface call [{interfaceName}.{callName}] using descriptors [{descriptors}] has been returned and is therefore not deliverable: {returnReplyText}",
                serviceCall.InterfaceName, serviceCall.ServiceCallName, serviceCall.Descriptors.ToString(), e.ReplyText);
            hasBeenReturned = true;
        }
    }
}
