using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQConsumer
{
    public delegate Task ReceivedMessageEventHandler(BasicDeliverEventArgs ea);

    public interface IMQConsumptionQueue : IDisposable
    {
        event ReceivedMessageEventHandler Received;

        IModel Channel { get; }

        void StartConsuming();
        void StopConsuming();
    }
}
