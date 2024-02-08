using AspNetServiceLib.DataStructures;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client;
using System.Diagnostics;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQPublisher
{
    internal class DedicatedQueueReplyHandler : IReplyHandler
    {
        private readonly ServiceCall serviceCall;
        private readonly IModel channel;

        private string replyQueueName;
        private AsyncEventingBasicConsumer replyConsumer;
        private string replyConsumerTag;

        public DedicatedQueueReplyHandler(ServiceCall serviceCall, IModel channel)
        {
            this.serviceCall = serviceCall;
            this.channel = channel;
        }

        public void Dispose()
        {
            try
            {
                if (channel != null && replyConsumerTag != null)
                {
                    channel.BasicCancelNoWait(replyConsumerTag);
                }
            }
            catch (AlreadyClosedException) { /* Ignore */ }
        }

        public IReplyWaiter PrepareReplyWaiter()
        {
            PrepareReplyQueueIfNecessary();
            Debug.Assert(replyConsumer != null);
            Debug.Assert(replyQueueName != null);
            return new DedicatedQueueReplyWaiter(serviceCall, replyConsumer, replyQueueName);
        }

        private void PrepareReplyQueueIfNecessary()
        {
            if (replyQueueName == null)
            {
                replyQueueName = channel.QueueDeclare(queue: GetReplyQueueName(), exclusive: true, autoDelete: true).QueueName;
                replyConsumer = new AsyncEventingBasicConsumer(channel);
                replyConsumerTag = channel.BasicConsume(queue: replyQueueName, autoAck: true, consumer: replyConsumer);
            }
        }

        private string GetReplyQueueName()
        {
            //Do *not* let RabbitMQ create queue name automatically, because the name would be invalid after
            //automatic connection recovery. Instead, create random (but meaningful) name here:
            var publicQueueName = MQConventionHelper.GetServiceCallPublicQueueName(
                serviceCall.InterfaceName,
                serviceCall.ServiceCallName,
                serviceCall.Descriptors);
            return $"Reply.{publicQueueName}-{Guid.NewGuid()}";
        }
    }
}
