using AspNetServiceLib.DataStructures;

namespace AspNetServiceLib.ServiceInterface.Implementation.ServiceCalls
{
    public interface IServiceCommand : IDisposable
    {
        Task CallCommandAsync(
            ServiceMessage message,
            TimeSpan timeout = default(TimeSpan),
            CancellationToken ct = default(CancellationToken));
    }
}
