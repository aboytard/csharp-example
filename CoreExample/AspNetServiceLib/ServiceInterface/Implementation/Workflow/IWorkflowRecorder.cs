using AspNetServiceLib.DataStructures;

namespace AspNetServiceLib.ServiceInterface.Implementation.Workflow
{
    public interface IWorkflowRecorder
    {
        Action<ServiceMessage> RecordReceivedMessage(ServiceCall serviceCall, ServiceMessage message);
        Action<ServiceMessage> RecordPublishedMessage(ServiceCall serviceCall, ServiceMessage message);
    }
}
