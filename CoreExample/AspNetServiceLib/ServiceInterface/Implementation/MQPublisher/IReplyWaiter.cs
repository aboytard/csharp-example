using AspNetServiceLib.DataStructures;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQPublisher
{
    internal interface IReplyWaiter : IDisposable
    {
        string ReplyQueueName { get; }
        string CorrelationId { get; }

        Task<ServiceMessage> WaitForReplyAsync(TimeSpan timeout, CancellationToken ct);
    }
}
