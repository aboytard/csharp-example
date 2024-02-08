using AspNetServiceLib.DataStructures;
using AspNetServiceLib.Exceptions;
using AspNetServiceLib.ServiceInterface.Implementation.MQPublisher;
using AspNetServiceLib.ServiceInterface.Implementation.ServiceCalls;
using AspNetServiceLib.Setup;

namespace AspNetServiceLib.ServiceInterface.Implementation
{
    public class ServiceInterfaceConsumer : ServiceInterfaceBase, IServiceInterfaceConsumer, IServiceInterfaceConsumerOperationMode
    {
        private readonly QueueSetup queueSetup;

        public ServiceInterfaceConsumer(
            ServiceInterfaceSetup setup,
            string interfaceName,
            ServiceInterfaceDescriptors descriptors,
            QueueSetup queueSetup)
            : base(setup, interfaceName, descriptors, InterfaceRoleEnum.Consumed)
        {
            this.queueSetup = queueSetup;
        }

        public IServiceCommand RegisterCommand(string commandName, bool waitForDelivery, bool? usePublicQueue = null)
        {
            if (!Descriptors.IsSpecific)
            {
                throw new UnspecificDescriptorsException("registering a command call");
            }

            var serviceCommand = new ServiceCommand(
                queueSetup.MQPublisherFactory,
                new ServiceCall(this, commandName),
                waitForDelivery,
                usePublicQueue);
            AddDisposable(serviceCommand);
            return serviceCommand;
        }

        public IServiceRequest RegisterRequest(string requestName, bool? usePublicQueue = null)
        {
            if (!Descriptors.IsSpecific)
            {
                throw new UnspecificDescriptorsException("registering a request call");
            }

            var serviceCommand = new ServiceRequest(
                queueSetup.MQPublisherFactory,
                new ServiceCall(this, requestName),
                usePublicQueue);
            AddDisposable(serviceCommand);
            return serviceCommand;
        }

        public void SubscribeEventHandler(string eventName, ServiceEventHandler eventHandler, bool useAck)
        {
            var serviceCall = new ServiceCall(this, eventName);
            var consumerParams = new MQConsumerParams(useAck, "topic", false, IsProcessingStopped);
            var consumer = queueSetup.MQConsumerFactory.CreateMQConsumer(
                GetConsumeChannel(),
                serviceCall,
                async m =>
                {
                    await eventHandler(m);
                    return null;
                }, consumerParams);
            AddDisposable(consumer);
            AddConsumer(consumer);
        }

        private class ServiceCommand : IServiceCommand
        {
            private readonly IMQPublisher mqPublisher;

            public ServiceCommand(
                IMQPublisherFactory mqPublisherFactory,
                ServiceCall serviceCall,
                bool waitForDelivery,
                bool? usePublicQueue)
            {
                var mqParams = new MQPublisherParams(
                    waitForDelivery,
                    "direct",
                    usePublicQueue ?? serviceCall.Descriptors.IsUnaddressed,
                    false);
                mqPublisher = mqPublisherFactory.CreateMQPublisher(serviceCall, mqParams);
            }

            public void Dispose()
            {
                mqPublisher.Dispose();
            }

            public Task CallCommandAsync(ServiceMessage message, TimeSpan timeout, CancellationToken ct)
            {
                return mqPublisher.PublishAsync(message, timeout, ct);
            }
        }

        private class ServiceRequest : IServiceRequest
        {
            private readonly IMQPublisher mqPublisher;

            public ServiceRequest(
                IMQPublisherFactory mqPublisherFactory,
                ServiceCall serviceCall,
                bool? usePublicQueue)
            {
                var mqParams = new MQPublisherParams(
                    true,
                    "direct",
                    usePublicQueue ?? serviceCall.Descriptors.IsUnaddressed,
                    true);
                mqPublisher = mqPublisherFactory.CreateMQPublisher(serviceCall, mqParams);
            }

            public void Dispose()
            {
                mqPublisher.Dispose();
            }

            public Task<ServiceMessage> CallRequestAsync(ServiceMessage message, TimeSpan timeout, CancellationToken ct)
            {
                return mqPublisher.PublishAsync(message, timeout, ct);
            }
        }
    }
}
