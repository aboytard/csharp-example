using AspNetServiceLib.DataStructures;

namespace AspNetServiceLib.ServiceInterface.Implementation.ServiceCalls
{
    public interface IServiceRequest: IDisposable
    {
        Task<ServiceMessage> CallRequestAsync(
            ServiceMessage message,
            TimeSpan timeout = default(TimeSpan),
            CancellationToken ct = default(CancellationToken));
    }
}
