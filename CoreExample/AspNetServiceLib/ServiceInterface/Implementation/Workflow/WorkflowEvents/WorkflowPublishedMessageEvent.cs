using AspNetServiceLib.DataStructures;
using System.Text.Json.Serialization;

namespace AspNetServiceLib.ServiceInterface.Implementation.Workflow.WorkflowEvents
{
    internal class WorkflowPublishedMessageEvent : WorkflowEvent
    {
        public const string ClassEventType = "Published";
        public override string EventType => ClassEventType;

        public ServiceMessage Message { get; }
        public ServiceMessage Reply { get; }

        [JsonConstructor]
        public WorkflowPublishedMessageEvent(ServiceCall serviceCall, ServiceMessage message, ServiceMessage reply)
            : base(serviceCall)
        {
            Message = message;
            Reply = reply;
        }
    }
}
