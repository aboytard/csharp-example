using AspNetServiceLib.DataStructures;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQConsumer
{
    public delegate Task<ServiceMessage> ReceiveHandlerAsyncDelegate(ServiceMessage message);

    public class MQConsumer : IMQConsumer
    {
        private readonly ServiceCall serviceCall;
        private readonly IModel channel;
        private readonly ReceiveHandlerAsyncDelegate receiveHandler;
        private readonly IMQConsumptionQueue consumptionQueue;
        private readonly bool useAck;
        private bool isDisposed;
        private readonly CancellationTokenSource disposeCts = new CancellationTokenSource();
        private TaskCompletionSource<bool> pauseCompletionSource;

        private static readonly ILogger logger = Logging.Factory.CreateLogger<MQConsumer>();
        public MQConsumer(
            ServiceCall serviceCall,
            ReceiveHandlerAsyncDelegate receiveHandler,
            MQConsumerParams consumerParams,
            IMQConsumptionQueue consumptionQueue)
        {
            this.serviceCall = serviceCall;
            this.receiveHandler = receiveHandler;
            this.consumptionQueue = consumptionQueue;
            useAck = consumerParams.UseAck;
            channel = consumptionQueue.Channel;
            consumptionQueue.Received += ReceivedMessageAsync;

            if (consumerParams.ExplicitStart)
            {
                StopProcessing();
            }
            else
            {
                StartProcessing();
            }
        }
        public virtual void Dispose()
        {
            //Do *not* call this dispose method from a thread which is asynchronously inside consumer received method right now.
            //This would result in a deadlock.

            isDisposed = true;
            pauseCompletionSource?.TrySetResult(false);
            pauseCompletionSource = null;
            disposeCts.Cancel();
            disposeCts.Dispose();
            consumptionQueue.Dispose();
        }

        public void StartProcessing()
        {
            pauseCompletionSource?.TrySetResult(true);
            pauseCompletionSource = null;
            consumptionQueue.StartConsuming();
        }

        public void StopProcessing()
        {
            pauseCompletionSource = new TaskCompletionSource<bool>();
            consumptionQueue.StopConsuming();
        }

        private async Task ReceivedMessageAsync(BasicDeliverEventArgs ea)
        {
            await (pauseCompletionSource?.Task ?? Task.CompletedTask);  //Wait until pause is over
            if (isDisposed)
            {
                return;
            }

            logger.LogDebug("Received service interface message from call [{interfaceName}.{callName}] using descriptors [{descriptors}].",
                serviceCall.InterfaceName, serviceCall.ServiceCallName, serviceCall.Descriptors.ToString());

            var errorOccured = false;
            var (success, reply) = await ProcessMessage(ea);
            if (!success)
            {
                errorOccured = true;
            }

            if (!await HandleReplyIfNecessary(reply, ea))
            {
                errorOccured = true;
            }

            await HandleAckIfNecessary(ea, errorOccured);
        }
        private async Task<(bool success, ServiceMessage reply)> ProcessMessage(BasicDeliverEventArgs ea)
        {
            try
            {
                var props = ea.BasicProperties;
                var handlerTask = receiveHandler(new ServiceMessage(ea.Body.ToArray(), props.ContentType));
                var disposeEvent = new TaskCompletionSource<bool>();
                using (disposeCts.Token.Register(() => disposeEvent.SetResult(true)))
                {
                    //Wait until handler completes or this consumer is disposed:
                    await Task.WhenAny(handlerTask, disposeEvent.Task);
                }

                if (isDisposed)
                {
                    logger.LogWarning(
                        "Consumer of received service interface message from call [{interfaceName}.{callName}] using descriptors [{descriptors}] was disposed during processing of message.",
                        serviceCall.InterfaceName, serviceCall.ServiceCallName, serviceCall.Descriptors.ToString());
                    return (false, null);
                }

                var reply = await handlerTask;
                logger.LogDebug(
                    "Successfully processed received service interface message from call [{interfaceName}.{callName}] using descriptors [{descriptors}].",
                    serviceCall.InterfaceName, serviceCall.ServiceCallName, serviceCall.Descriptors.ToString());
                return (true, reply);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Error trying to process received service interface message from call [{interfaceName}.{callName}] using descriptors [{descriptors}]: {errorMessage}",
                    serviceCall.InterfaceName, serviceCall.ServiceCallName, serviceCall.Descriptors.ToString(), ex.Message);
                return (false, null);
            }
        }

        private async Task HandleAckIfNecessary(BasicDeliverEventArgs ea, bool errorOccured)
        {
            if (useAck && !isDisposed)
            {
                await HandleAck(ea, errorOccured);
            }
        }

        private async Task HandleAck(BasicDeliverEventArgs ea, bool errorOccured)
        {
            try
            {
                if (!errorOccured)
                {
                    await Task.Run(() => channel.BasicAck(ea.DeliveryTag, false));
                }
                else
                {
                    await Task.Run(() => channel.BasicNack(ea.DeliveryTag, false, false));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Error trying to acknowledge received service interface message from call [{interfaceName}.{callName}] using descriptors [{descriptors}]: {errorMessage}",
                    serviceCall.InterfaceName, serviceCall.ServiceCallName, serviceCall.Descriptors.ToString(), ex.Message);
            }
        }

        private async Task<bool> HandleReplyIfNecessary(ServiceMessage reply, BasicDeliverEventArgs ea)
        {
            if (!isDisposed && ea.BasicProperties.ReplyTo != null)
            {
                return await HandleReply(reply, ea);
            }
            return true;
        }

        private async Task<bool> HandleReply(ServiceMessage reply, BasicDeliverEventArgs ea)
        {
            try
            {
                var props = ea.BasicProperties;
                if (reply == null)
                {
                    //In case a reply is expected, but we did not get one from the receive handler, we send an empty message back:
                    reply = new ServiceMessage(new byte[0], props.ContentType);
                }

                //Reply through reply channel:
                var replyProps = channel.CreateBasicProperties();
                replyProps.ContentType = reply.MimeType;
                replyProps.CorrelationId = props.CorrelationId;
                props.Priority = 1; //Has higher priority than normal messages
                if (props.Expiration != null)
                {
                    replyProps.Expiration = props.Expiration;
                }
                await Task.Run(() => channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: props.ReplyTo,
                    basicProperties: replyProps,
                    body: reply.MessageData));
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Error trying to reply to received service interface message from call [{interfaceName}.{callName}] using descriptors [{descriptors}]: {errorMessage}",
                    serviceCall.InterfaceName, serviceCall.ServiceCallName, serviceCall.Descriptors.ToString(), ex.Message);
                return false;
            }
        }
    }
}
