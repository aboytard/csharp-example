using AspNetServiceLib.DataStructures;
using RabbitMQ.Client;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQPublisher
{
    internal class MQPublisherFactory : IMQPublisherFactory
    {
        private readonly IConnection connection;
        private readonly IReplyHandlerFactory replyHandlerFactory;

        public MQPublisherFactory(IConnection connection, IReplyHandlerFactory replyHandlerFactory)
        {
            this.connection = connection;
            this.replyHandlerFactory = replyHandlerFactory;
        }

        public IMQPublisher CreateMQPublisher(ServiceCall serviceCall, MQPublisherParams publisherParams)
        {
            return new MQPublisher(connection, serviceCall, publisherParams, replyHandlerFactory);
        }
    }
}
