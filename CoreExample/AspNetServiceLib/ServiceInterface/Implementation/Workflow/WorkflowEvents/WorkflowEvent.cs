using AspNetServiceLib.DataStructures;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AspNetServiceLib.ServiceInterface.Implementation.Workflow.WorkflowEvents
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal abstract class WorkflowEvent
    {
        public abstract string EventType { get; }

        public ServiceCall ServiceCall { get; }

        protected WorkflowEvent(ServiceCall serviceCall)
        {
            ServiceCall = serviceCall;
        }
    }
}
