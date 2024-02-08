using AspNetServiceLib.DataStructures;
using AspNetServiceLib.Exceptions;
using AspNetServiceLib.ServiceInterface.Implementation.Workflow.WorkflowEvents;
using Microsoft.Extensions.Logging;

namespace AspNetServiceLib.ServiceInterface.Implementation.Workflow
{
    public class WorkflowEngine : IWorkflowEngine
    {
        private readonly ILogger logger = Logging.Factory.CreateLogger<WorkflowEngine>();
        private readonly Dictionary<ServiceCall, IWorkflowMessageCommunicator> allCommunicators;
        private readonly Dictionary<ServiceCall, IWorkflowMessageReceiver> receivers;
        private readonly Dictionary<ServiceCall, IWorkflowMessagePublisher> publishers;
        private WorkflowRecorder currentWorkflowRecorder;

        public WorkflowEngine()
        {
            allCommunicators = new Dictionary<ServiceCall, IWorkflowMessageCommunicator>();
            receivers = new Dictionary<ServiceCall, IWorkflowMessageReceiver>();
            publishers = new Dictionary<ServiceCall, IWorkflowMessagePublisher>();
        }

        public void RegisterWorkflowMessageReceiver(IWorkflowMessageReceiver receiver)
        {
            receivers.Add(receiver.ServiceCall, receiver);
            allCommunicators.Add(receiver.ServiceCall, receiver);
        }

        public void UnregisterWorkflowMessageReceiver(IWorkflowMessageReceiver receiver)
        {
            receivers.Remove(receiver.ServiceCall);
            allCommunicators.Remove(receiver.ServiceCall);
        }

        public void RegisterWorkflowMessagePublisher(IWorkflowMessagePublisher publisher)
        {
            publishers.Add(publisher.ServiceCall, publisher);
            allCommunicators.Add(publisher.ServiceCall, publisher);
        }

        public void UnregisterWorkflowMessagePublisher(IWorkflowMessagePublisher publisher)
        {
            publishers.Remove(publisher.ServiceCall);
            allCommunicators.Remove(publisher.ServiceCall);
        }

        public WorkflowRecording StartRecording(WorkflowRecordingScope scope)
        {
            if (currentWorkflowRecorder != null)
            {
                throw new WorkflowException("Recording already running");
            }
            var workflowRecorder = new WorkflowRecorder();
            foreach (var serviceCall in scope.ServiceCalls)
            {
                if (!allCommunicators.TryGetValue(serviceCall, out var communicator))
                {
                    throw new WorkflowException($"Unobserved service call '{serviceCall}'.");
                }
                communicator.StartRecording(workflowRecorder);
            }
            currentWorkflowRecorder = workflowRecorder;
            return workflowRecorder.WorkflowRecording;
        }

        public void StopRecording(WorkflowRecording workflowRecording)
        {
            if (workflowRecording != currentWorkflowRecorder?.WorkflowRecording)
            {
                throw new WorkflowException("Workflow recording to stop is not the current one.");
            }
            currentWorkflowRecorder = null;
            if (workflowRecording != null)
            {
                foreach (var communicator in allCommunicators.Values)
                {
                    communicator.StopRecording();
                }
                workflowRecording.Complete();
            }
        }

        public async Task<bool> ReplayRecording(
            WorkflowRecording workflowRecording,
            WorkflowReplayParameters parameters,
            CancellationToken ct)
        {
            var workflowExecution = new WorkflowExecution(workflowRecording, parameters, ReplayReceivedMessage);

            try
            {
                PreparePublishers(workflowRecording, workflowExecution);
                return await workflowExecution.Execute(ct);
            }
            finally
            {
                ReleasePublishers();
            }
        }

        private void PreparePublishers(WorkflowRecording workflowRecording, WorkflowExecution workflowExecution)
        {
            var publisherServiceCalls = workflowRecording.EventHistory
                            .OfType<WorkflowPublishedMessageEvent>()
                            .Select(ev => ev.ServiceCall)
                            .Distinct();
            foreach (var serviceCall in publisherServiceCalls)
            {
                if (!publishers.TryGetValue(serviceCall, out var publisher))
                {
                    throw new WorkflowException($"Unobserved service call '{serviceCall}'.");
                }
                publisher.UseWorkflowReplay(workflowExecution);
            }
        }

        private void ReleasePublishers()
        {
            foreach (var publisher in publishers.Values)
            {
                publisher.StopWorkflowReplay();
            }
        }
        private async Task<ServiceMessage> ReplayReceivedMessage(ServiceCall serviceCall, ServiceMessage message)
        {
            if (receivers.TryGetValue(serviceCall, out var receiver))
            {
                logger.LogDebug(
                    "Replaying message of service call '{serviceCall}' for workflow exeuction:\n{message}",
                    serviceCall, message.MessageJson);
                return await receiver.ReplayReceivedMessage(message);
            }
            throw new WorkflowException($"No receiver for replaying a message of service call '{serviceCall}'.");
        }
    }
}
