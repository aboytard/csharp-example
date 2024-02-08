using AspNetServiceLib.DataStructures;

namespace AspNetServiceLib.ServiceInterface.Implementation.Workflow
{
    public interface IWorkflowMessageCommunicator
    {
        ServiceCall ServiceCall { get; }

        void StartRecording(IWorkflowRecorder workflowRecorder);

        void StopRecording();
    }
}
