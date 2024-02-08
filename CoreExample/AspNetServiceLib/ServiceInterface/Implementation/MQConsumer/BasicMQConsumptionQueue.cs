using AspNetServiceLib.DataStructures;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQConsumer
{
    internal class BasicMQConsumptionQueue : IMQConsumptionQueue, IDisposable
    {
        private readonly ServiceCall serviceCall;
        private readonly MQConsumerParams consumerParams;
        private string consumerTag;
        private string queueName;
        private event ReceivedMessageEventHandler ReceivedInternal;

        public event ReceivedMessageEventHandler Received
        {
            add
            {
                ReceivedInternal += value;
                EstablishConsumption();
            }
            remove => ReceivedInternal -= value;
        }

        public IModel Channel { get; }

        public BasicMQConsumptionQueue(
            IModel consumeChannel,
            ServiceCall serviceCall,
            MQConsumerParams consumerParams)
        {
            Channel = consumeChannel;
            this.serviceCall = serviceCall;
            this.consumerParams = consumerParams;
        }

        public void InitializeQueue()
        {
            if (Channel == null)
            {
                return; //No real connection is used (unit test).
            }

            var serviceCallName = serviceCall.ServiceCallName;
            var interfaceName = serviceCall.InterfaceName;
            var descriptors = serviceCall.Descriptors;
            var exchangeName = MQConventionHelper.GetServiceCallExchangeName(interfaceName, serviceCallName);
            Channel.ExchangeDeclare(exchangeName, consumerParams.ExchangeType);

            var usePublicQueue = consumerParams.UsePublicQueue;
            var declare = Channel.QueueDeclare(
                queue: usePublicQueue
                        ? MQConventionHelper.GetServiceCallPublicQueueName(interfaceName, serviceCallName, descriptors)
                        : string.Empty,
                durable: false,
                exclusive: !usePublicQueue,
                autoDelete: !usePublicQueue,
                arguments: null);

            queueName = declare.QueueName;
            Channel.QueueBind(queueName, exchangeName, MQConventionHelper.GetRoutingKey(descriptors));
        }

        public void Dispose()
        {
            try
            {
                if (Channel != null && consumerTag != null)
                {
                    Channel.BasicCancelNoWait(consumerTag);
                }
            }
            catch (AlreadyClosedException) { /* Ignore */ }
        }

        public void StartConsuming()
        {
        }

        public void StopConsuming()
        {
        }

        private Task ReceivedMessageAsync(object sender, BasicDeliverEventArgs ea)
        {
            return ReceivedInternal?.Invoke(ea);
        }

        private void EstablishConsumption()
        {
            if (Channel == null)
            {
                return; //No real connection is used (unit test).
            }

            if (consumerTag == null)
            {
                var consumer = new AsyncEventingBasicConsumer(Channel);
                consumer.Received += ReceivedMessageAsync;
                consumerTag = Channel.BasicConsume(queue: queueName, autoAck: !consumerParams.UseAck, consumer: consumer);
            }
        }
    }
}
