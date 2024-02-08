using AspNetServiceLib.DataStructures;

namespace AspNetServiceLib.ServiceInterface
{
    public delegate Task ServiceEventHandler(ServiceMessage message);

    public delegate Task ServiceCommandHandler(ServiceMessage message);

    public delegate Task<ServiceMessage> ServiceRequestHandler(ServiceMessage message);
}
