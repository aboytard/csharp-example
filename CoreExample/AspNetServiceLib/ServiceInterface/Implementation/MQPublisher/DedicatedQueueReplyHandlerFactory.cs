using AspNetServiceLib.DataStructures;
using RabbitMQ.Client;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQPublisher
{
    internal class DedicatedQueueReplyHandlerFactory : IReplyHandlerFactory
    {
        public IReplyHandler CreateReplyHandler(ServiceCall serviceCall, IModel channel)
        {
            return new DedicatedQueueReplyHandler(serviceCall, channel);
        }
    }
}
