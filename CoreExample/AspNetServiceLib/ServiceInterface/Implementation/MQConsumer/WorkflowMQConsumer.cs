using AspNetServiceLib.DataStructures;
using AspNetServiceLib.ServiceInterface.Implementation.Workflow;
using RabbitMQ.Client;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQConsumer
{
    internal class WorkflowMQConsumer : IMQConsumer, IWorkflowMessageReceiver
    {
        private readonly IMQConsumer baseConsumer;
        private readonly ReceiveHandlerAsyncDelegate receiveHandler;
        private readonly WorkflowEngine workflowEngine;
        private IWorkflowRecorder currentWorkflowRecorder;

        public ServiceCall ServiceCall { get; }

        public WorkflowMQConsumer(
            IModel consumeChannel,
            ServiceCall serviceCall,
            ReceiveHandlerAsyncDelegate receiveHandler,
            MQConsumerParams consumerParams,
            IMQConsumerFactory consumerFactory,
            WorkflowEngine workflowEngine)
        {
            this.receiveHandler = receiveHandler;
            baseConsumer = consumerFactory.CreateMQConsumer(
                consumeChannel,
                serviceCall,
                ReceiveHandler,
                consumerParams);
            ServiceCall = serviceCall;
            this.workflowEngine = workflowEngine;
            workflowEngine.RegisterWorkflowMessageReceiver(this);
        }

        public void Dispose()
        {
            workflowEngine.UnregisterWorkflowMessageReceiver(this);
            baseConsumer.Dispose();
        }

        public async Task<ServiceMessage> ReceiveHandler(ServiceMessage message)
        {
            var setReply = currentWorkflowRecorder?.RecordReceivedMessage(ServiceCall, message);
            var reply = await receiveHandler(message);
            setReply?.Invoke(reply);
            return reply;
        }

        public Task<ServiceMessage> ReplayReceivedMessage(ServiceMessage message)
        {
            return ReceiveHandler(message);
        }

        public void StartProcessing()
        {
            baseConsumer.StartProcessing();
        }

        public void StopProcessing()
        {
            baseConsumer.StopProcessing();
        }

        public void StartRecording(IWorkflowRecorder workflowRecorder)
        {
            currentWorkflowRecorder = workflowRecorder;
        }

        public void StopRecording()
        {
            currentWorkflowRecorder = null;
        }
    }
}
