using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNetServiceLib.ServiceInterface.Implementation.Workflow.WorkflowEvents
{
    internal class WorkflowEventConverter : JsonConverter<WorkflowEvent>
    {
        private readonly Dictionary<string, Type> workflowEvents;

        public override bool CanRead => true;
        public override bool CanWrite => false;

        public WorkflowEventConverter()
        {
            workflowEvents = new Dictionary<string, Type>();
        }

        public void RegisterWorkflowEventType<T>(string eventType) where T : WorkflowEvent
        {
            workflowEvents.Add(eventType, typeof(T));
        }

        public override WorkflowEvent ReadJson(
            JsonReader reader,
            Type objectType,
            WorkflowEvent existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            var eventToken = JToken.ReadFrom(reader);
            var eventTypeString = eventToken.Value<string>("eventType");
            if (workflowEvents.TryGetValue(eventTypeString, out var eventType))
            {
                return (WorkflowEvent)eventToken.ToObject(eventType);
            }
            throw new ArgumentException($"Unknown event type '{eventTypeString}'.");
        }

        public override void WriteJson(JsonWriter writer, WorkflowEvent value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}
