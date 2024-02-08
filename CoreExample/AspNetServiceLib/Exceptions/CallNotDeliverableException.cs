using AspNetServiceLib.DataStructures;

namespace AspNetServiceLib.Exceptions
{
    public class CallNotDeliverableException : Exception
    {
        public ServiceMessage ServiceMessage { get; }

        public string InterfaceName { get; }

        public string ServiceCallName { get; }

        public CallNotDeliverableException(string interfaceName, string serviceCallName, ServiceMessage message, Exception innerException = null)
            : base($"Service call [{interfaceName}.{serviceCallName}] not deliverable.", innerException)
        {
            InterfaceName = interfaceName;
            ServiceCallName = serviceCallName;
            ServiceMessage = message;
        }
    }
}
