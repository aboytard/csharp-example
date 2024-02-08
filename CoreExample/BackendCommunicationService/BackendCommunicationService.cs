using AspNetServiceLib;
using Microsoft.Extensions.Logging;

namespace BackendCommunicationService
{
    public class BackendCommunicationService : IDisposable
    {
        protected readonly ILogger logger = Logging.Factory.CreateLogger<BackendCommunicationService>();
        //protected readonly Dictionary<string, CommunicationConsumer> consumers = new();

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}