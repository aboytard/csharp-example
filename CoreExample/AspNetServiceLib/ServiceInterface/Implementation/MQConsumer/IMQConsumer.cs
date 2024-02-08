namespace AspNetServiceLib.ServiceInterface.Implementation.MQConsumer
{
    public interface IMQConsumer : IDisposable
    {
        void StartProcessing();

        void StopProcessing();
    }
}
