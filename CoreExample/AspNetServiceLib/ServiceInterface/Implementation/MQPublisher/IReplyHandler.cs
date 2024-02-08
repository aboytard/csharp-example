namespace AspNetServiceLib.ServiceInterface.Implementation.MQPublisher
{
    internal interface IReplyHandler : IDisposable
    {
        IReplyWaiter PrepareReplyWaiter();
    }
}
