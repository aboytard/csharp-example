using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;
using AspNetServiceLib.DataStructures;

namespace AspNetServiceLib.ServiceInterface.Implementation.Workflow.WorkflowEvents
{
    [Obfuscation(Exclude = true)]
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal class WorkflowReceivedMessageEvent : WorkflowEvent
    {
        public const string ClassEventType = "Received";
        public override string EventType => ClassEventType;

        public ServiceMessage Message { get; }
        public ServiceMessage Reply { get; }

        [JsonConstructor]
        public WorkflowReceivedMessageEvent(ServiceCall serviceCall, ServiceMessage message, ServiceMessage reply)
            : base(serviceCall)
        {
            Message = message;
            Reply = reply;
        }
    }
}
