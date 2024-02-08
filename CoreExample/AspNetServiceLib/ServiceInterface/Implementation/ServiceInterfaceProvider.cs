using AspNetServiceLib.DataStructures;
using AspNetServiceLib.ServiceInterface.Implementation.MQPublisher;
using AspNetServiceLib.ServiceInterface.Implementation.ServiceCalls;
using AspNetServiceLib.Setup;

namespace AspNetServiceLib.ServiceInterface.Implementation
{
    internal class ServiceInterfaceProvider : ServiceInterfaceBase, IServiceInterfaceProvider
    {
        private readonly QueueSetup queueSetup;

        public ServiceInterfaceProvider(
            ServiceInterfaceSetup setup,
            string interfaceName,
            ServiceInterfaceDescriptors descriptors,
            QueueSetup queueSetup)
            : base(setup, interfaceName, descriptors, InterfaceRoleEnum.Provided)
        {
            this.queueSetup = queueSetup;
        }

        public void RegisterCommandHandler(string commandName, ServiceCommandHandler commandHandler, bool useAck, bool? usePublicQueue)
        {
            var serviceCall = new ServiceCall(this, commandName);
            var consumerParams = new MQConsumerParams(useAck, "direct", usePublicQueue ?? Descriptors.IsUnaddressed, IsProcessingStopped);
            var consumer = queueSetup.MQConsumerFactory.CreateMQConsumer(
                GetConsumeChannel(),
                serviceCall,
                async m =>
                {
                    await commandHandler(m);
                    return null;
                }, consumerParams);
            AddDisposable(consumer);
            AddConsumer(consumer);
        }

        public void RegisterRequestHandler(string requestName, ServiceRequestHandler requestHandler, bool useAck, bool? usePublicQueue)
        {
            var serviceCall = new ServiceCall(this, requestName);
            var consumerParams = new MQConsumerParams(useAck, "direct", usePublicQueue ?? Descriptors.IsUnaddressed, IsProcessingStopped);
            var consumer = queueSetup.MQConsumerFactory.CreateMQConsumer(
                GetConsumeChannel(),
                serviceCall,
                m => requestHandler(m),
                consumerParams);
            AddDisposable(consumer);
            AddConsumer(consumer);
        }

        public IServiceEvent RegisterEvent(string eventName, bool waitForDelivery)
        {
            var serviceCommand = new ServiceEvent(
                queueSetup.MQPublisherFactory,
                new ServiceCall(this, eventName),
                waitForDelivery);
            AddDisposable(serviceCommand);
            return serviceCommand;
        }

        private class ServiceEvent : IServiceEvent
        {
            private readonly IMQPublisher mqPublisher;

            public ServiceEvent(
                IMQPublisherFactory mqPublisherFactory,
                ServiceCall serviceCall,
                bool waitForDelivery)
            {
                var mqParams = new MQPublisherParams(waitForDelivery, "topic", false, false);
                mqPublisher = mqPublisherFactory.CreateMQPublisher(serviceCall, mqParams);
            }

            public void Dispose()
            {
                mqPublisher.Dispose();
            }

            public Task PublishEventAsync(ServiceMessage message, TimeSpan timeout, CancellationToken ct)
            {
                return mqPublisher.PublishAsync(message, timeout, ct);
            }
        }
    }
}
