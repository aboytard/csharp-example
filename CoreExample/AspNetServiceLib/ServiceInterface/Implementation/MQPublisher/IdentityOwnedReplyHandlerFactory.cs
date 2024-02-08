using AspNetServiceLib.DataStructures;
using RabbitMQ.Client;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQPublisher
{
    internal class IdentityOwnedReplyHandlerFactory : IReplyHandlerFactory
    {
        private readonly IdentityOwnedQueueReplyListener queueListener;

        public IdentityOwnedReplyHandlerFactory(IdentityOwnedQueueReplyListener queueListener)
        {
            this.queueListener = queueListener;
        }

        public IReplyHandler CreateReplyHandler(ServiceCall serviceCall, IModel channel)
        {
            return new IdentityOwnedReplyHandler(serviceCall, queueListener);
        }
    }
}
