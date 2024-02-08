namespace AspNetServiceLib.Exceptions
{
    internal class MQPublisherDisposedException : Exception
    {
        public MQPublisherDisposedException(string serviceInterfaceName, string serviceCallName)
            : base($"MQ publisher for service interface [{serviceInterfaceName}] " +
                   $"call [{serviceCallName}] has already been disposed.")
        { }
    }
}
