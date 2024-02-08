using AspNetServiceLib.Exceptions;
using AspNetServiceLib.ServiceInterface.Implementation.Workflow.WorkflowEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNetServiceLib.DataStructures
{
    public class WorkflowRecording
    {
        private static readonly JsonSerializer serializer;
        private readonly List<WorkflowEvent> eventHistory;
        private readonly object mutex = new object();

        internal IEnumerable<WorkflowEvent> EventHistory => eventHistory;

        public delegate void RecordingChangedDelegate(int index, JObject eventMessage);
        public event RecordingChangedDelegate RecordingChanged;
        public event Action RecordingCompleted;

        public WorkflowRecording(IEnumerable<JObject> eventMessages) : this()
        {
            foreach (var eventMessage in eventMessages)
            {
                eventHistory.Add(eventMessage.ToObject<WorkflowEvent>(serializer));
            }
        }

        internal WorkflowRecording()
        {
            eventHistory = new List<WorkflowEvent>();
        }

        static WorkflowRecording()
        {
            var eventConverter = new WorkflowEventConverter();
            eventConverter.RegisterWorkflowEventType<WorkflowPublishedMessageEvent>(WorkflowPublishedMessageEvent.ClassEventType);
            eventConverter.RegisterWorkflowEventType<WorkflowReceivedMessageEvent>(WorkflowReceivedMessageEvent.ClassEventType);
            serializer = new JsonSerializer()
            {
                Converters = { eventConverter }
            };
        }

        internal void AddWorkflowEvent(WorkflowEvent workflowEvent)
        {
            int newIndex;
            lock (mutex)
            {
                eventHistory.Add(workflowEvent);
                newIndex = eventHistory.Count - 1;
            }
            RecordingChanged?.Invoke(newIndex, JObject.FromObject(workflowEvent));
        }

        internal void ReplaceWorkflowEvent(WorkflowEvent oldWorkflowEvent, WorkflowEvent newWorkflowEvent)
        {
            int index;
            lock (mutex)
            {
                index = eventHistory.IndexOf(oldWorkflowEvent);
                if (index < 0)
                {
                    throw new WorkflowException("Workflow event to replace not found.");
                }
                eventHistory[index] = newWorkflowEvent;
            }
            RecordingChanged?.Invoke(index, JObject.FromObject(newWorkflowEvent));
        }

        internal void Complete()
        {
            RecordingCompleted?.Invoke();
        }
    }
}
