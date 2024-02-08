using AspNetServiceLib.DataStructures;
using AspNetServiceLib.ServiceInterface.Implementation.IdentityOwnedQueue;
using AspNetServiceLib.ServiceInterface.Implementation.MQConsumer;
using AspNetServiceLib.ServiceInterface.Implementation.MQPublisher;
using AspNetServiceLib.ServiceInterface.Implementation.Workflow;

namespace AspNetServiceLib.Setup
{
    public class QueueSetup : IDisposable
    {
        private readonly ServiceContext context;
        private readonly WorkflowEngine workflowEngine;

        private readonly ServiceParameters parameters;

        public IMQConsumerFactory MQConsumerFactory { get; }
        public IMQPublisherFactory MQPublisherFactory { get; }

        public IdentityOwnedInputQueue IdentityOwnedInputQueue { get; private set; }

        public QueueSetup(
            ServiceContext context,
            WorkflowEngine workflowEngine,
            ServiceParameters parameters)
        {
            this.context = context;
            this.workflowEngine = workflowEngine;
            this.parameters = parameters;
            MQConsumerFactory = CreateMQConsumerFactory();
            MQPublisherFactory = CreateMQPublisherFactory(false);
            IdentityOwnedInputQueue?.InitializeQueue();
        }

        public void Dispose()
        {
            IdentityOwnedInputQueue?.Dispose();
        }

        private IMQConsumerFactory CreateMQConsumerFactory()
        {
            var factory = CreateBasicMQConsumerFactory();
            if (workflowEngine == null)
            {
                return factory;
            }
            return new WorkflowMQConsumerFactory(factory, workflowEngine);
        }

        private IMQConsumerFactory CreateBasicMQConsumerFactory()
        {
            if (parameters.UseIdentityOwnedServiceCalls)
            {
                var inputQueue = EstablishIdentityOwnedInputQueue();
                var identityOwnedQueueServiceCallListener = new IdentityOwnedQueueServiceCallListener(inputQueue);
                inputQueue.RegisterListener(identityOwnedQueueServiceCallListener);
                return new IdentityOwnedMQConsumerFactory(identityOwnedQueueServiceCallListener);
            }
            return new BasicMQConsumerFactory();
        }

        private IMQPublisherFactory CreateMQPublisherFactory(bool useIdentityOwnedReplies)
        {
            var replyHandlerFactory = CreateReplyHandlerFactory(useIdentityOwnedReplies);
            var factory = new MQPublisherFactory(context.Connection, replyHandlerFactory);
            if (workflowEngine == null)
            {
                return factory;
            }
            return new WorkflowMQPublisherFactory(workflowEngine, factory);
        }

        private IReplyHandlerFactory CreateReplyHandlerFactory(bool useIdentityOwnedReplies)
        {
            if (useIdentityOwnedReplies)
            {
                var inputQueue = EstablishIdentityOwnedInputQueue();
                var identityOwnedQueueReplyListener = new IdentityOwnedQueueReplyListener(inputQueue);
                inputQueue.RegisterListener(identityOwnedQueueReplyListener);
                return new IdentityOwnedReplyHandlerFactory(identityOwnedQueueReplyListener);
            }
            return new DedicatedQueueReplyHandlerFactory();
        }

        private IdentityOwnedInputQueue EstablishIdentityOwnedInputQueue()
        {
            if (IdentityOwnedInputQueue == null)
            {
                IdentityOwnedInputQueue = new IdentityOwnedInputQueue(
                    context.ServiceName,
                    context.Connection,
                    parameters.UseSingleActiveConsumerInstance);
            }

            return IdentityOwnedInputQueue;
        }
    }
}
