using AspNetServiceLib.DataStructures;
using AspNetServiceLib.Exceptions;
using AspNetServiceLib.ServiceInterface;
using AspNetServiceLib.ServiceInterface.Implementation;
using AspNetServiceLib.ServiceInterface.Implementation.Workflow;
using RabbitMQ.Client;
using System.Linq;

namespace AspNetServiceLib.Setup
{
    public class ServiceInterfaceSetup : IDisposable
    {
        private readonly QueueSetup queueSetup;
        private readonly List<ServiceInterfaceConsumer> serviceInterfaceConsumers = new List<ServiceInterfaceConsumer>();
        private readonly List<ServiceInterfaceProvider> serviceInterfaceProviders = new List<ServiceInterfaceProvider>();

        internal IConnection Connection { get; }

        internal bool IsUpdatable { get; set; }

        public TimeSpan DefaultServiceCallTimeout { get; internal set; }

        public Dictionary<string, CustomServiceCallTimeout> CustomServiceCallTimeouts { get; internal set; } = new();

        public IEnumerable<IServiceInterface> AllRelatedServiceInterfaces => serviceInterfaceConsumers.Concat<IServiceInterface>(serviceInterfaceProviders);

        public IEnumerable<IServiceInterfaceConsumer> ServiceInterfaceConsumers => serviceInterfaceConsumers;

        public IEnumerable<IServiceInterfaceProvider> ServiceInterfaceProviders => serviceInterfaceProviders;

        public IWorkflowEngine WorkflowEngine { get; }

        public bool UsesIdentityOwnedInputQueue => queueSetup.IdentityOwnedInputQueue != null;

        public event Func<Task> OnMessageReceivedFromIdentityOwnedInputQueue;

        public ServiceInterfaceSetup(ServiceContext context, ServiceParameters parameters)
        {
            Connection = context.Connection;
            IsUpdatable = false;
            WorkflowEngine workflowEngine = null;
            if (parameters.UseWorkflowEngine)
            {
                workflowEngine = new WorkflowEngine();
                WorkflowEngine = workflowEngine;
            }
            queueSetup = new QueueSetup(context, workflowEngine, parameters);
            var identityOwned = queueSetup.IdentityOwnedInputQueue;
            if (identityOwned != null)
            {
                identityOwned.OnMessageReceived += HandleMessageReceivedFromIdentityOwnedInputQueue;
            }
        }

        public void Dispose()
        {
            foreach (var serviceInterface in AllRelatedServiceInterfaces.ToList())
            {
                serviceInterface.Dispose();
            }
            var identityOwned = queueSetup.IdentityOwnedInputQueue;
            if (identityOwned != null)
            {
                identityOwned.OnMessageReceived -= HandleMessageReceivedFromIdentityOwnedInputQueue;
            }
            queueSetup.Dispose();
        }

        public void StartProcessing()
        {
            foreach (var si in serviceInterfaceConsumers.Concat<ServiceInterfaceBase>(serviceInterfaceProviders))
            {
                si.StartProcessing();
            }
        }

        public void StopProcessing()
        {
            foreach (var si in serviceInterfaceConsumers.Concat<ServiceInterfaceBase>(serviceInterfaceProviders))
            {
                si.StopProcessing();
            }
        }

        public IServiceInterfaceConsumer CreateServiceInterfaceConsumer(string interfaceName, ServiceInterfaceDescriptors descriptors = default)
        {
            CheckUpdatability();
            var consumer = new ServiceInterfaceConsumer(this, interfaceName, descriptors, queueSetup);
            consumer.StopProcessing();
            serviceInterfaceConsumers.Add(consumer);
            return consumer;
        }

        public IServiceInterfaceProvider CreateServiceInterfaceProvider(string interfaceName, ServiceInterfaceDescriptors descriptors = default)
        {
            CheckUpdatability();
            if (!descriptors.IsSpecific)
            {
                throw new UnspecificDescriptorsException("providing an interface");
            }

            var provider = new ServiceInterfaceProvider(this, interfaceName, descriptors, queueSetup);
            serviceInterfaceProviders.Add(provider);
            return provider;
        }

        public IEnumerable<IServiceInterfaceConsumer> GetServiceInterfaceConsumers(string interfaceName, ServiceInterfaceDescriptors descriptors = default)
        {
            return serviceInterfaceConsumers.Where(c => c.InterfaceName == interfaceName && c.Descriptors.Equals(descriptors));
        }

        public IEnumerable<IServiceInterfaceProvider> GetServiceInterfaceProvider(string interfaceName, ServiceInterfaceDescriptors descriptors = default)
        {
            return serviceInterfaceProviders.Where(c => c.InterfaceName == interfaceName && c.Descriptors.Equals(descriptors));
        }

        public void ReleaseActiveConsumerInstance()
        {
            queueSetup.IdentityOwnedInputQueue?.Release();
        }

        internal void DisposeServiceInterface(IServiceInterface serviceInterface)
        {
            CheckUpdatability();

            if (serviceInterface is ServiceInterfaceConsumer consumer)
            {
                serviceInterfaceConsumers.Remove(consumer);
            }
            else if (serviceInterface is ServiceInterfaceProvider provider)
            {
                serviceInterfaceProviders.Remove(provider);
            }
        }

        internal void CheckUpdatability()
        {
            if (!IsUpdatable)
            {
                throw new SetupNotUpdatableException();
            }
        }

        private Task HandleMessageReceivedFromIdentityOwnedInputQueue()
        {
            return OnMessageReceivedFromIdentityOwnedInputQueue?.Invoke();
        }
    }

}
