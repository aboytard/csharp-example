using AspNetServiceLib.DataStructures;
using AspNetServiceLib.ServiceInterface.Implementation.IdentityOwnedQueue;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Diagnostics;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQConsumer
{
    internal class IdentityOwnedQueueServiceCallListener : IIdentityOwnedInputQueueListener
    {
        private readonly IdentityOwnedInputQueue identityOwnedInputQueue;
        private readonly Dictionary<(string exchangeName, string routingKey), PrivateConsumptionQueueView> serviceCallBindings;
        private readonly Dictionary<string, List<string>> routingKeysForExchangeName;

        public IdentityOwnedQueueServiceCallListener(IdentityOwnedInputQueue identityOwnedInputQueue)
        {
            this.identityOwnedInputQueue = identityOwnedInputQueue;
            serviceCallBindings = new Dictionary<(string exchangeName, string routingKey), PrivateConsumptionQueueView>();
            routingKeysForExchangeName = new Dictionary<string, List<string>>();
        }

        public bool CanHandleReceivedMessage(BasicDeliverEventArgs ea)
        {
            if (ea.BasicProperties.IsCorrelationIdPresent()
                && !ea.BasicProperties.IsReplyToPresent()
                && string.IsNullOrEmpty(ea.Exchange))
            {
                return false;   //We don't handle reply messages
            }
            return GetConsumptionQueueViewForMessage(ea) != null;
        }

        public async Task HandleReceivedMessage(object sender, BasicDeliverEventArgs ea)
        {
            var queue = GetConsumptionQueueViewForMessage(ea);
            Debug.Assert(queue != null);
            await queue.Consume(ea);
        }

        private PrivateConsumptionQueueView GetConsumptionQueueViewForMessage(BasicDeliverEventArgs ea)
        {
            var exchangeName = ea.Exchange;
            var routingKey = ea.RoutingKey;
            var bindingRoutingKey = GetMatchingBindingRoutingKey(exchangeName, routingKey);
            if (bindingRoutingKey != null && serviceCallBindings.TryGetValue((exchangeName, bindingRoutingKey), out var queue))
            {
                return queue;
            }
            return null;
        }

        public IMQConsumptionQueue RegisterServiceCall(ServiceCall serviceCall, MQConsumerParams consumerParams)
        {
            if (consumerParams.UsePublicQueue)
            {
                throw new Exception("Service calls with public queue requirement cannot be consumed on an identity-owned base.");
            }

            var serviceCallName = serviceCall.ServiceCallName;
            var interfaceName = serviceCall.InterfaceName;
            var descriptors = serviceCall.Descriptors;
            var routingKey = MQConventionHelper.GetRoutingKey(descriptors);
            var exchangeName = MQConventionHelper.GetServiceCallExchangeName(interfaceName, serviceCallName);
            if (serviceCallBindings.ContainsKey((exchangeName, routingKey)))
            {
                throw new Exception(
                    $"Registration for service call [{interfaceName}.{serviceCallName}] using descriptors [{descriptors}] already exists.");
            }

            var channel = identityOwnedInputQueue.Channel;
            channel.ExchangeDeclare(exchangeName, consumerParams.ExchangeType);
            channel.QueueBind(identityOwnedInputQueue.QueueName, exchangeName, routingKey);

            var privateView = new PrivateConsumptionQueueView(this, serviceCall);
            serviceCallBindings.Add((exchangeName, routingKey), privateView);
            AddRoutingKeyForExchangeName(routingKey, exchangeName);
            return privateView;
        }

        private void AddRoutingKeyForExchangeName(string routingKey, string exchangeName)
        {
            if (!routingKeysForExchangeName.TryGetValue(exchangeName, out var routingKeys))
            {
                routingKeys = new List<string>();
                routingKeysForExchangeName.Add(exchangeName, routingKeys);
            }
            Debug.Assert(!routingKeys.Contains(routingKey));
            routingKeys.Add(routingKey);
        }

        private void UnregisterServiceCall(ServiceCall serviceCall)
        {
            var serviceCallName = serviceCall.ServiceCallName;
            var interfaceName = serviceCall.InterfaceName;
            var descriptors = serviceCall.Descriptors;
            var routingKey = MQConventionHelper.GetRoutingKey(descriptors);
            var exchangeName = MQConventionHelper.GetServiceCallExchangeName(interfaceName, serviceCallName);
            if (serviceCallBindings.Remove((exchangeName, routingKey)))
            {
                Debug.Assert(routingKeysForExchangeName.ContainsKey(exchangeName));
                var removed = routingKeysForExchangeName[exchangeName].Remove(routingKey);
                Debug.Assert(removed);
                var channel = identityOwnedInputQueue.Channel;
                channel.QueueUnbind(identityOwnedInputQueue.QueueName, exchangeName, routingKey);
            }
        }

        private string GetMatchingBindingRoutingKey(string exchangeName, string routingKey)
        {
            if (routingKeysForExchangeName.TryGetValue(exchangeName, out var boundRoutingKeys))
            {
                return boundRoutingKeys.FirstOrDefault(binding => IsRoutingKeyMatching(binding, routingKey));
            }
            return null;
        }

        private bool IsRoutingKeyMatching(string routingKeyBinding, string routingKey)
        {
            var segments1 = routingKeyBinding.Split('.');
            var segments2 = routingKey.Split('.');
            if (segments1.Length != segments2.Length)
            {
                return false;
            }

            return segments1.Zip(segments2, (a, b) => a == "*" || a == b).All(matches => matches);
        }

        private class PrivateConsumptionQueueView : IMQConsumptionQueue
        {
            private readonly IdentityOwnedQueueServiceCallListener serviceCallQueueListener;
            private readonly ServiceCall serviceCall;

            public IModel Channel => serviceCallQueueListener.identityOwnedInputQueue.Channel;

            public event ReceivedMessageEventHandler Received;

            public PrivateConsumptionQueueView(
                IdentityOwnedQueueServiceCallListener serviceCallQueueListener,
                ServiceCall serviceCall)
            {
                this.serviceCallQueueListener = serviceCallQueueListener;
                this.serviceCall = serviceCall;
            }

            public void Dispose()
            {
                serviceCallQueueListener.UnregisterServiceCall(serviceCall);
            }

            public Task Consume(BasicDeliverEventArgs ea)
            {
                return Received?.Invoke(ea);
            }

            public void StartConsuming()
            {
                serviceCallQueueListener.identityOwnedInputQueue.StartConsuming();
            }

            public void StopConsuming()
            {
                serviceCallQueueListener.identityOwnedInputQueue.StopConsuming();
            }
        }
    }


}
