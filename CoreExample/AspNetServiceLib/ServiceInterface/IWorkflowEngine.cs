using AspNetServiceLib.DataStructures;

namespace AspNetServiceLib.ServiceInterface
{
    public interface IWorkflowEngine
    {
        WorkflowRecording StartRecording(WorkflowRecordingScope scope);
        void StopRecording(WorkflowRecording workflowRecording);

        Task<bool> ReplayRecording(
            WorkflowRecording workflowRecording,
            WorkflowReplayParameters parameters,
            CancellationToken ct);
    }
}
