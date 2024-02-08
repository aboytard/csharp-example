using AspNetServiceLib.DataStructures;
using AspNetServiceLib.ServiceInterface.Implementation.Workflow;
using RabbitMQ.Client;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQConsumer
{
    internal class WorkflowMQConsumerFactory : IMQConsumerFactory
    {
        private readonly IMQConsumerFactory baseFactory;
        private readonly WorkflowEngine workflowEngine;

        public WorkflowMQConsumerFactory(IMQConsumerFactory baseFactory, WorkflowEngine workflowEngine)
        {
            this.baseFactory = baseFactory;
            this.workflowEngine = workflowEngine;
        }

        public IMQConsumer CreateMQConsumer(
            IModel consumeChannel,
            ServiceCall serviceCall,
            ReceiveHandlerAsyncDelegate receiveHandler,
            MQConsumerParams consumerParams)
        {
            if (consumerParams.UsePublicQueue)
            {
                return baseFactory.CreateMQConsumer(consumeChannel, serviceCall, receiveHandler, consumerParams);
            }

            return new WorkflowMQConsumer(
                consumeChannel,
                serviceCall,
                receiveHandler,
                consumerParams,
                baseFactory,
                workflowEngine);
        }
    }
}
