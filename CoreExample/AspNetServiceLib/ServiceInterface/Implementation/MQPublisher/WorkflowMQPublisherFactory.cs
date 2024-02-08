using AspNetServiceLib.DataStructures;
using AspNetServiceLib.ServiceInterface.Implementation.Workflow;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQPublisher
{
    internal class WorkflowMQPublisherFactory : IMQPublisherFactory
    {
        private readonly WorkflowEngine workflowEngine;
        private readonly IMQPublisherFactory baseFactory;

        public WorkflowMQPublisherFactory(WorkflowEngine workflowEngine, IMQPublisherFactory baseFactory)
        {
            this.workflowEngine = workflowEngine;
            this.baseFactory = baseFactory;
        }

        public IMQPublisher CreateMQPublisher(ServiceCall serviceCall, MQPublisherParams publisherParams)
        {
            var mqPublisher = baseFactory.CreateMQPublisher(serviceCall, publisherParams);
            return new WorkflowMQPublisher(workflowEngine, mqPublisher, serviceCall);
        }
    }
}
