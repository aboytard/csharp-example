using AspNetServiceLib.ServiceInterface.Implementation.ServiceCalls;

namespace AspNetServiceLib.ServiceInterface
{
    public interface IServiceInterfaceProvider : IServiceInterface
    {
        void RegisterCommandHandler(string commandName, ServiceCommandHandler commandHandler, bool useAck = true, bool? usePublicQueue = null);

        void RegisterRequestHandler(string requestName, ServiceRequestHandler requestHandler, bool useAck = true, bool? usePublicQueue = null);

        IServiceEvent RegisterEvent(string eventName, bool waitForDelivery = true);
    }
}
