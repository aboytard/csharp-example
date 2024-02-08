using AspNetServiceLib.DataStructures;
using RabbitMQ.Client;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQConsumer
{
    public interface IMQConsumerFactory
    {
        IMQConsumer CreateMQConsumer(
            IModel consumeChannel,
            ServiceCall serviceCall,
            ReceiveHandlerAsyncDelegate receiveHandler,
            MQConsumerParams consumerParams);
    }
}
