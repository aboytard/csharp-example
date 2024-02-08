using AspNetServiceLib.DataStructures;
using AspNetServiceLib.Exceptions;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQPublisher
{
    internal class DedicatedQueueReplyWaiter : IReplyWaiter
    {
        private readonly ServiceCall serviceCall;
        private readonly AsyncEventingBasicConsumer replyConsumer;
        private readonly TaskCompletionSource<ServiceMessage> replyReceivedCompletionSource;

        private static readonly ILogger logger = Logging.Factory.CreateLogger<DedicatedQueueReplyWaiter>();

        public string ReplyQueueName { get; }

        public string CorrelationId { get; }


        public DedicatedQueueReplyWaiter(ServiceCall serviceCall, AsyncEventingBasicConsumer replyConsumer, string replyQueueName)
        {
            this.serviceCall = serviceCall;
            this.replyConsumer = replyConsumer;
            CorrelationId = Guid.NewGuid().ToString();
            ReplyQueueName = replyQueueName;
            this.replyConsumer.Received += ReplyConsumer_Received;
            replyReceivedCompletionSource = new TaskCompletionSource<ServiceMessage>();
        }

        public void Dispose()
        {
            replyConsumer.Received -= ReplyConsumer_Received;
        }

        private Task ReplyConsumer_Received(object sender, BasicDeliverEventArgs ea)
        {
            var props = ea.BasicProperties;
            if (props.CorrelationId == CorrelationId)
            {
                //This is "our" reply:
                var consumer = sender as AsyncEventingBasicConsumer;
                try
                {
                    var replyMessage = new ServiceMessage(ea.Body.ToArray(), props.ContentType);
                    replyReceivedCompletionSource.SetResult(replyMessage);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex,
                        "Error trying to process received reply message for service interface call [{interfaceName}.{callName}] using descriptors [{descriptors}]: {error}",
                        serviceCall.InterfaceName, serviceCall.ServiceCallName, serviceCall.Descriptors.ToString(), ex.Message);
                }

                //Note: No need to ack/nack the message here, because the dedicated reply queue uses auto-acking.
            }

            return Task.CompletedTask;
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
    }
}
