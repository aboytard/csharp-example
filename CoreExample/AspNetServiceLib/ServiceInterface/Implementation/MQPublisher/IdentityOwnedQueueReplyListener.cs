using AspNetServiceLib.DataStructures;
using AspNetServiceLib.Exceptions;
using AspNetServiceLib.ServiceInterface.Implementation.IdentityOwnedQueue;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQPublisher
{
    internal class IdentityOwnedQueueReplyListener : IIdentityOwnedInputQueueListener
    {
        private static readonly ILogger logger = Logging.Factory.CreateLogger<IdentityOwnedQueueReplyListener>();
        private readonly IdentityOwnedInputQueue identityOwnedInputQueue;
        private readonly ConcurrentDictionary<string, PrivateReplyWaiter> replyWaiters;
        public IdentityOwnedQueueReplyListener(IdentityOwnedInputQueue identityOwnedInputQueue)
        {
            this.identityOwnedInputQueue = identityOwnedInputQueue;
            replyWaiters = new ConcurrentDictionary<string, PrivateReplyWaiter>();
        }

        public bool CanHandleReceivedMessage(BasicDeliverEventArgs ea)
        {
            return ea.BasicProperties.IsCorrelationIdPresent()
                && !ea.BasicProperties.IsReplyToPresent()
                && string.IsNullOrEmpty(ea.Exchange);
        }

        public Task HandleReceivedMessage(object sender, BasicDeliverEventArgs ea)
        {
            return HandleAndAckReplyMessage(ea);
        }

        public IReplyWaiter RegisterWaitForReply(ServiceCall serviceCall)
        {
            var replyWaiter = new PrivateReplyWaiter(serviceCall, this);
            replyWaiters.TryAdd(replyWaiter.CorrelationId, replyWaiter);
            identityOwnedInputQueue.StartConsuming();
            return replyWaiter;
        }

        private void UnregisterReplyWaiter(string correlationId)
        {
            replyWaiters.TryRemove(correlationId, out _);
        }

        private async Task HandleAndAckReplyMessage(BasicDeliverEventArgs ea)
        {
            var (success, serviceCall) = HandleReplyMessage(ea);
            try
            {
                var channel = identityOwnedInputQueue.Channel;
                if (success)
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
                if (serviceCall.HasValue)
                {
                    logger.LogError(ex,
                        "Error trying to acknowledge received reply messsage from call [{interfaceName}.{callName}] using descriptors [{descriptors}]: {errorMessage}",
                        serviceCall.Value.InterfaceName, serviceCall.Value.ServiceCallName, serviceCall.Value.Descriptors.ToString(), ex.Message);
                }
                else
                {
                    logger.LogError(ex,
                        "Error trying to acknowledge received reply messsage from unknown call: {errorMessage}",
                        ex.Message);
                }
            }
        }

        private (bool success, ServiceCall? serviceCall) HandleReplyMessage(BasicDeliverEventArgs ea)
        {
            if (replyWaiters.TryGetValue(ea.BasicProperties.CorrelationId, out var replyWaiter))
            {
                var success = replyWaiter.HandleReplyMessage(ea);
                return (success, replyWaiter.ServiceCall);
            }
            return (false, null);
        }

        private class PrivateReplyWaiter : IReplyWaiter
        {
            private readonly IdentityOwnedQueueReplyListener identityOwnedQueueReplyListener;
            private readonly TaskCompletionSource<ServiceMessage> replyReceivedCompletionSource;

            public string ReplyQueueName { get; }

            public string CorrelationId { get; }

            public ServiceCall ServiceCall { get; }

            public PrivateReplyWaiter(ServiceCall serviceCall, IdentityOwnedQueueReplyListener identityOwnedQueueReplyListener)
            {
                ServiceCall = serviceCall;
                this.identityOwnedQueueReplyListener = identityOwnedQueueReplyListener;
                ReplyQueueName = identityOwnedQueueReplyListener.identityOwnedInputQueue.QueueName;
                CorrelationId = Guid.NewGuid().ToString();
                replyReceivedCompletionSource = new TaskCompletionSource<ServiceMessage>();
            }

            public void Dispose()
            {
                identityOwnedQueueReplyListener.UnregisterReplyWaiter(CorrelationId);
                replyReceivedCompletionSource.TrySetCanceled();
            }

            public async Task<ServiceMessage> WaitForReplyAsync(TimeSpan timeout, CancellationToken ct)
            {
                using (ct.Register(() => replyReceivedCompletionSource.TrySetCanceled()))
                {
                    var finishedTask = await Task.WhenAny(replyReceivedCompletionSource.Task, Task.Delay(timeout));
                    if (finishedTask == replyReceivedCompletionSource.Task)
                    {
                        return await replyReceivedCompletionSource.Task;
                    }
                    throw new WaitForReplyTimeoutException();
                }
            }

            public bool HandleReplyMessage(BasicDeliverEventArgs ea)
            {
                var props = ea.BasicProperties;
                var replyMessage = new ServiceMessage(ea.Body.ToArray(), props.ContentType);
                return replyReceivedCompletionSource.TrySetResult(replyMessage);
            }
        }
    }
}
