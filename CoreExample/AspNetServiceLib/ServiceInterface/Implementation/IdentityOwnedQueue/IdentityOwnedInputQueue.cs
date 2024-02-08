using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace AspNetServiceLib.ServiceInterface.Implementation.IdentityOwnedQueue
{
    public class IdentityOwnedInputQueue : IDisposable
    {
        private static readonly ILogger logger = Logging.Factory.CreateLogger<IdentityOwnedInputQueue>();
        private readonly string serviceName;
        private readonly IConnection connection;
        private readonly bool useSingleActiveConsumerInstance;
        private readonly List<IIdentityOwnedInputQueueListener> listeners;
        private string consumerTag;

        public IModel Channel { get; private set; }
        public string QueueName { get; private set; }

        public event Func<Task> OnMessageReceived;

        public IdentityOwnedInputQueue(string serviceName, IConnection connection, bool useSingleActiveConsumerInstance)
        {
            this.serviceName = serviceName;
            this.connection = connection;
            this.useSingleActiveConsumerInstance = useSingleActiveConsumerInstance;
            listeners = new List<IIdentityOwnedInputQueueListener>();
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

        public void RegisterListener(IIdentityOwnedInputQueueListener listener)
        {
            listeners.Add(listener);
        }

        public void InitializeQueue()
        {
            if (connection == null)
            {
                return; //No real connection is used (unit test).
            }

            Channel = connection.CreateModel();
            Channel.BasicQos(0, 1, false);

            var arguments = new Dictionary<string, object>();
            if (useSingleActiveConsumerInstance)
            {
                arguments.Add("x-single-active-consumer", true);
            }
            var declare = Channel.QueueDeclare(
                queue: MQConventionHelper.GetIdentityOwnedQueueName(serviceName),
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments);
            QueueName = declare.QueueName;
        }

        public void StartConsuming()
        {
            if (Channel != null && consumerTag == null)
            {
                var consumer = new AsyncEventingBasicConsumer(Channel);
                consumer.Received += ReceivedMessageAsync;
                consumerTag = Channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
            }
        }

        public void StopConsuming()
        {
            if (Channel != null && consumerTag != null)
            {
                Channel.BasicCancel(consumerTag);
                consumerTag = null;
            }
        }

        public void Release()
        {
            if (Channel != null && consumerTag != null)
            {
                StopConsuming();
                StartConsuming();
            }
        }

        private async Task ReceivedMessageAsync(object sender, BasicDeliverEventArgs ea)
        {
            var handlerTask = OnMessageReceived?.Invoke();
            if (handlerTask != null)
            {
                await handlerTask;
            }

            foreach (var listener in listeners)
            {
                if (listener.CanHandleReceivedMessage(ea))
                {
                    await listener.HandleReceivedMessage(sender, ea);
                    return;
                }
            }
            await NackUnknownMessage(ea);
        }

        private async Task NackUnknownMessage(BasicDeliverEventArgs ea)
        {
            try
            {
                await Task.Run(() => Channel.BasicNack(ea.DeliveryTag, false, false));
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Error trying to negatively acknowledge service interface message with no consumer " +
                    "received via exchange '{exchangeName}' and routing key '{routingKey}': {errorMessage}",
                    ea.Exchange, ea.RoutingKey, ex.Message);
            }
        }
    }
}
