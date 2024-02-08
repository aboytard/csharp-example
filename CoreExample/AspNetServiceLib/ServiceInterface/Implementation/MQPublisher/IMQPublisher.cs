using AspNetServiceLib.DataStructures;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQPublisher
{
    public interface IMQPublisher : IDisposable
    {
        Task<ServiceMessage> PublishAsync(ServiceMessage message, TimeSpan timeout, CancellationToken ct);
    }
}
