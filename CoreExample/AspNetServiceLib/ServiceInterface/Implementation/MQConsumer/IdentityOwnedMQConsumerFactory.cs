using AspNetServiceLib.DataStructures;
using RabbitMQ.Client;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQConsumer
{
    internal class IdentityOwnedMQConsumerFactory : IMQConsumerFactory
    {
        private readonly IdentityOwnedQueueServiceCallListener queueListener;

        public IdentityOwnedMQConsumerFactory(IdentityOwnedQueueServiceCallListener queueListener)
        {
            this.queueListener = queueListener;
        }

        public IMQConsumer CreateMQConsumer(
            IModel consumeChannel,
            ServiceCall serviceCall,
            ReceiveHandlerAsyncDelegate receiveHandler,
            MQConsumerParams consumerParams)
        {
            var adjustedConsumerParams = AdjustConsumerParams(consumerParams);
            var consumptionQueue = CreateConsumptionQueue(
                consumeChannel,
                serviceCall,
                adjustedConsumerParams);
            return new MQConsumer(serviceCall, receiveHandler, adjustedConsumerParams, consumptionQueue);
        }

        private IMQConsumptionQueue CreateConsumptionQueue(
            IModel consumeChannel,
            ServiceCall serviceCall,
            MQConsumerParams consumerParams)
        {
            if (consumerParams.UsePublicQueue)
            {
                var consumptionQueue = new BasicMQConsumptionQueue(consumeChannel, serviceCall, consumerParams);
                consumptionQueue.InitializeQueue();
                return consumptionQueue;
            }

            return queueListener.RegisterServiceCall(serviceCall, consumerParams);
        }

        private MQConsumerParams AdjustConsumerParams(MQConsumerParams consumerParams)
        {
            if (consumerParams.UsePublicQueue)
            {
                return consumerParams;
            }

            return new MQConsumerParams(
                true,   // useAck always needs to be "true", because the identity-owned queue has auto-ack deactivated
                consumerParams.ExchangeType,
                false,
                consumerParams.ExplicitStart);
        }
    }
}
