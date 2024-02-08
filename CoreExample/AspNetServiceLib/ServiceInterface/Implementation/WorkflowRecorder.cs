using AspNetServiceLib.DataStructures;
using AspNetServiceLib.ServiceInterface.Implementation.Workflow;
using AspNetServiceLib.ServiceInterface.Implementation.Workflow.WorkflowEvents;

namespace AspNetServiceLib.ServiceInterface.Implementation
{
    internal class WorkflowRecorder : IWorkflowRecorder
    {
        public WorkflowRecording WorkflowRecording { get; }

        public WorkflowRecorder()
        {
            WorkflowRecording = new WorkflowRecording();
        }

        public Action<ServiceMessage> RecordReceivedMessage(ServiceCall serviceCall, ServiceMessage message)
        {
            var messageEvent = new WorkflowReceivedMessageEvent(serviceCall, message, null);
            WorkflowRecording.AddWorkflowEvent(messageEvent);
            void SetReply(ServiceMessage reply)
            {
                if (reply != null)
                {
                    var messageEventWithReply = new WorkflowReceivedMessageEvent(serviceCall, message, reply);
                    WorkflowRecording.ReplaceWorkflowEvent(messageEvent, messageEventWithReply);
                }
            }
            return SetReply;
        }

        public Action<ServiceMessage> RecordPublishedMessage(ServiceCall serviceCall, ServiceMessage message)
        {
            var messageEvent = new WorkflowPublishedMessageEvent(serviceCall, message, null);
            WorkflowRecording.AddWorkflowEvent(messageEvent);
            void SetReply(ServiceMessage reply)
            {
                if (reply != null)
                {
                    var messageEventWithReply = new WorkflowPublishedMessageEvent(serviceCall, message, reply);
                    WorkflowRecording.ReplaceWorkflowEvent(messageEvent, messageEventWithReply);
                }
            }
            return SetReply;
        }
    }
}
