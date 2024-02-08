using AspNetServiceLib.DataStructures;

namespace AspNetServiceLib.ServiceInterface.Implementation.Workflow
{
    public interface IWorkflowMessageReceiver : IWorkflowMessageCommunicator
    {
        Task<ServiceMessage> ReplayReceivedMessage(ServiceMessage message);
    }
}
