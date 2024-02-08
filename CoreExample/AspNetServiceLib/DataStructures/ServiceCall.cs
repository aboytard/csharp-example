using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using AspNetServiceLib.ServiceInterface.Implementation;

namespace AspNetServiceLib.DataStructures
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public struct ServiceCall
    {
        public string InterfaceName { get; }

        public string ServiceCallName { get; }

        public ServiceInterfaceDescriptors Descriptors { get; }

        public ServiceCall(string interfaceName, string serviceCallName, ServiceInterfaceDescriptors descriptors)
        {
            InterfaceName = interfaceName;
            ServiceCallName = serviceCallName;
            Descriptors = descriptors;
        }

        internal ServiceCall(ServiceInterfaceBase serviceInterface, string serviceCallName)
            : this(serviceInterface.InterfaceName, serviceCallName, serviceInterface.Descriptors)
        {
        }

        public override bool Equals(object obj)
        {
            return obj is ServiceCall call &&
                   InterfaceName == call.InterfaceName &&
                   ServiceCallName == call.ServiceCallName &&
                   Descriptors.Equals(call.Descriptors);
        }

        public override int GetHashCode()
        {
            int hashCode = 291730179;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InterfaceName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ServiceCallName);
            hashCode = hashCode * -1521134295 + Descriptors.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"Interface: '{InterfaceName}' - Call: '{ServiceCallName}' - Descriptors: {Descriptors}";
        }
    }
}
