using AspNetServiceLib.DataStructures;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQPublisher
{
    internal class IdentityOwnedReplyHandler : IReplyHandler
    {
        private readonly ServiceCall serviceCall;
        private readonly IdentityOwnedQueueReplyListener queueListener;

        public IdentityOwnedReplyHandler(ServiceCall serviceCall, IdentityOwnedQueueReplyListener queueListener)
        {
            this.serviceCall = serviceCall;
            this.queueListener = queueListener;
        }

        public void Dispose()
        {
        }

        public IReplyWaiter PrepareReplyWaiter()
        {
            return queueListener.RegisterWaitForReply(serviceCall);
        }
    }
}
