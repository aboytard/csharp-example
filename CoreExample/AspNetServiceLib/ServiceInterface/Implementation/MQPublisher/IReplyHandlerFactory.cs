using AspNetServiceLib.DataStructures;
using RabbitMQ.Client;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQPublisher
{
    internal interface IReplyHandlerFactory
    {
        IReplyHandler CreateReplyHandler(ServiceCall serviceCall, IModel channel);
    }
}
