using AspNetServiceLib.DataStructures;

namespace AspNetServiceLib.ServiceInterface.Implementation
{
    public static class MQConventionHelper
    {
        public static string GetServiceCallExchangeName(string interfaceName, string serviceCallName)
        {
            return $"Ex.{interfaceName}.{serviceCallName}";
        }

        public static string GetServiceCallPublicQueueName(
            string interfaceName,
            string serviceCallName,
            ServiceInterfaceDescriptors descriptors)
        {
            //The "public queue" is used in case of unaddressed service calls to perform load balancing.
            if (descriptors.IsUnaddressed)
            {
                return $"Queue.{interfaceName}.{serviceCallName}";
            }
            return $"Queue.{interfaceName}.{serviceCallName}.{GetRoutingKey(descriptors)}";
        }

        public static string GetIdentityOwnedQueueName(string serviceName) => $"Queue.identity.{serviceName}";

        public static string GetRoutingKey(ServiceInterfaceDescriptors descriptors)
        {
            if (descriptors.IsUnaddressed)
            {
                return "";
            }
            return string.Join(".", descriptors.DescriptorValues.Select(v => v ?? "*"));
        }
    }
}
