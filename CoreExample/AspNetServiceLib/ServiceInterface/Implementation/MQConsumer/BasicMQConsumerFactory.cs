using AspNetServiceLib.DataStructures;
using RabbitMQ.Client;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQConsumer
{
    internal class BasicMQConsumerFactory : IMQConsumerFactory
    {
        public IMQConsumer CreateMQConsumer(
            IModel consumeChannel,
            ServiceCall serviceCall,
            ReceiveHandlerAsyncDelegate receiveHandler,
            MQConsumerParams consumerParams)
        {
            var consumptionQueue = new BasicMQConsumptionQueue(consumeChannel, serviceCall, consumerParams);
            consumptionQueue.InitializeQueue();
            return new MQConsumer(serviceCall, receiveHandler, consumerParams, consumptionQueue);
        }
    }
}
