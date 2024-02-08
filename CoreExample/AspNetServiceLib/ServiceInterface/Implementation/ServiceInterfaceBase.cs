using AspNetServiceLib.DataStructures;
using AspNetServiceLib.ServiceInterface.Implementation.MQConsumer;
using AspNetServiceLib.Setup;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace AspNetServiceLib.ServiceInterface.Implementation
{
    public abstract class ServiceInterfaceBase : IServiceInterface
    {
        private readonly ServiceInterfaceSetup serviceInterfaceSetup;
        private readonly List<IDisposable> objectsToDispose = new List<IDisposable>();
        private readonly List<IMQConsumer> consumers = new List<IMQConsumer>();
        protected IModel consumeChannel;

        internal IConnection Connection { get; private set; }

        public string InterfaceName { get; }

        public ServiceInterfaceDescriptors Descriptors { get; }

        public InterfaceRoleEnum InterfaceRole { get; }

        public bool IsProcessingStopped { get; private set; }

        public bool UsesIdentityOwnedInputQueue => serviceInterfaceSetup.UsesIdentityOwnedInputQueue;

        protected ServiceInterfaceBase(
            ServiceInterfaceSetup setup,
            string interfaceName,
            ServiceInterfaceDescriptors descriptors,
            InterfaceRoleEnum interfaceRole)
        {
            serviceInterfaceSetup = setup;
            Connection = setup.Connection;
            InterfaceName = interfaceName;
            Descriptors = descriptors;
            InterfaceRole = interfaceRole;
        }

        public virtual void Dispose()
        {
            serviceInterfaceSetup.DisposeServiceInterface(this);

            foreach (var disposable in objectsToDispose)
            {
                disposable.Dispose();
            }
            objectsToDispose.Clear();

            if (consumeChannel != null)
            {
                try
                {
                    consumeChannel.Close();
                }
                catch (AlreadyClosedException) { /* Ignore */ }

                try
                {
                    consumeChannel.Dispose();
                }
                catch (AlreadyClosedException) { /* Ignore */ }
            }
        }

        public void StartProcessing()
        {
            IsProcessingStopped = false;
            foreach (var consumer in consumers)
            {
                consumer.StartProcessing();
            }
        }

        public void StopProcessing()
        {
            IsProcessingStopped = true;
            foreach (var consumer in consumers)
            {
                consumer.StopProcessing();
            }
        }

        internal IModel GetConsumeChannel()
        {
            if (consumeChannel == null && Connection != null)
            {
                consumeChannel = Connection.CreateModel();
                consumeChannel.BasicQos(0, 1, false);
            }
            return consumeChannel;
        }

        protected void AddDisposable(IDisposable disposable)
        {
            objectsToDispose.Add(disposable);
        }

        protected void AddConsumer(IMQConsumer consumer)
        {
            consumers.Add(consumer);
        }
    }
}
