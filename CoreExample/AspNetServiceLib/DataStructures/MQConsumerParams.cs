namespace AspNetServiceLib.DataStructures
{
    public struct MQConsumerParams
    {
        public bool UseAck { get; }
        public string ExchangeType { get; }
        public bool UsePublicQueue { get; }
        public bool ExplicitStart { get; }

        public MQConsumerParams(bool useAck, string exchangeType, bool usePublicQueue, bool explicitStart)
        {
            UseAck = useAck;
            ExchangeType = exchangeType;
            UsePublicQueue = usePublicQueue;
            ExplicitStart = explicitStart;
        }
    }
}
