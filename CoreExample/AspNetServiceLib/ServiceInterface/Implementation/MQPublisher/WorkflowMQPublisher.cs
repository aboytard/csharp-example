using AspNetServiceLib.DataStructures;
using AspNetServiceLib.ServiceInterface.Implementation.Workflow;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQPublisher
{
    internal class WorkflowMQPublisher : IMQPublisher, IWorkflowMessagePublisher
    {
        private readonly WorkflowEngine workflowEngine;
        private readonly IMQPublisher inner;
        private IWorkflowReplay currentWorkflowReplay;
        private IWorkflowRecorder currentWorkflowRecorder;

        public ServiceCall ServiceCall { get; }
        public WorkflowMQPublisher(WorkflowEngine workflowEngine, IMQPublisher inner, ServiceCall serviceCall)
        {
            this.workflowEngine = workflowEngine;
            this.inner = inner;
            ServiceCall = serviceCall;
            workflowEngine.RegisterWorkflowMessagePublisher(this);
        }

        public void Dispose()
        {
            workflowEngine.UnregisterWorkflowMessagePublisher(this);
            inner.Dispose();
        }

        public async Task<ServiceMessage> PublishAsync(ServiceMessage message, TimeSpan timeout, CancellationToken ct)
        {
            var (alreadyPerformed, reply) = RecoverPublishedMessage(message);
            var setReply = currentWorkflowRecorder?.RecordPublishedMessage(ServiceCall, message);
            if (!alreadyPerformed)
            {
                reply = await inner.PublishAsync(message, timeout, ct);
            }
            setReply?.Invoke(reply);
            return reply;
        }

        public void UseWorkflowReplay(IWorkflowReplay workflowReplay)
        {
            currentWorkflowReplay = workflowReplay;
        }

        public void StopWorkflowReplay()
        {
            currentWorkflowReplay = null;
        }

        public void StartRecording(IWorkflowRecorder workflowRecorder)
        {
            currentWorkflowRecorder = workflowRecorder;
        }

        public void StopRecording()
        {
            currentWorkflowRecorder = null;
        }

        private (bool alreadyPerformed, ServiceMessage reply) RecoverPublishedMessage(ServiceMessage message)
        {
            if (currentWorkflowReplay == null)
            {
                return (false, null);
            }
            return currentWorkflowReplay.RecoverPublishedMessage(ServiceCall, message);
        }
    }
}
