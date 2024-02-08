using AspNetServiceLib.DataStructures;

namespace AspNetServiceLib.ServiceInterface.Implementation.ServiceCalls
{
    public interface IServiceEvent : IDisposable
    {
        Task PublishEventAsync(
            ServiceMessage message,
            TimeSpan timeout = default(TimeSpan),
            CancellationToken ct = default(CancellationToken));
    }
}
