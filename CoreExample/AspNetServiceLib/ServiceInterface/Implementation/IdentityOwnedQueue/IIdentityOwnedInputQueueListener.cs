using RabbitMQ.Client.Events;

namespace AspNetServiceLib.ServiceInterface.Implementation.IdentityOwnedQueue
{
    public interface IIdentityOwnedInputQueueListener
    {
        bool CanHandleReceivedMessage(BasicDeliverEventArgs ea);
        Task HandleReceivedMessage(object sender, BasicDeliverEventArgs ea);
    }
}
