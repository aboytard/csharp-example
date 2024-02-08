namespace AspNetServiceLib.ServiceInterface.Implementation.Workflow
{
    public interface IWorkflowMessagePublisher : IWorkflowMessageCommunicator
    {
        void UseWorkflowReplay(IWorkflowReplay workflowReplay);

        void StopWorkflowReplay();
    }
}
