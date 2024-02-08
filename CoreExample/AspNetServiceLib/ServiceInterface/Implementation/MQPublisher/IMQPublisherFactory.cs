using AspNetServiceLib.DataStructures;

namespace AspNetServiceLib.ServiceInterface.Implementation.MQPublisher
{
    public interface IMQPublisherFactory
    {
        IMQPublisher CreateMQPublisher(ServiceCall serviceCall, MQPublisherParams publisherParams);
    }
}
