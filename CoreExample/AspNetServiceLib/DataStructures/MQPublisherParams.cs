namespace AspNetServiceLib.DataStructures
{
    public struct MQPublisherParams
    {
        public bool WaitForDelivery { get; }
        public string ExchangeType { get; }
        public bool UsePublicQueue { get; }
        public bool UsesReplies { get; }

        public MQPublisherParams(bool waitForDelivery, string exchangeType, bool usePublicQueue, bool usesReplies)
        {
            WaitForDelivery = waitForDelivery;
            ExchangeType = exchangeType;
            UsePublicQueue = usePublicQueue;
            UsesReplies = usesReplies;
        }
    }
}
