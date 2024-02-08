using AspNetServiceLib.DataStructures;

namespace AspNetServiceLib.ServiceInterface.Implementation.Workflow
{
    public interface IWorkflowReplay
    {
        (bool alreadyPerformed, ServiceMessage reply) RecoverPublishedMessage(ServiceCall serviceCall, ServiceMessage message);
    }
}
