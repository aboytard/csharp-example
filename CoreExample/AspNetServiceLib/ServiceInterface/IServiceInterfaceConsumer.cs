using AspNetServiceLib.ServiceInterface.Implementation.ServiceCalls;

namespace AspNetServiceLib.ServiceInterface
{
    public interface IServiceInterfaceConsumer : IServiceInterface
    {
        IServiceCommand RegisterCommand(string commandName, bool waitForDelivery = true, bool? usePublicQueue = null);

        IServiceRequest RegisterRequest(string requestName, bool? usePublicQueue = null);

        void SubscribeEventHandler(string eventName, ServiceEventHandler eventHandler, bool useAck = true);
    }
}
