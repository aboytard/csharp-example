using Microsoft.Extensions.Logging;

namespace AspNetServiceLib.LoggerProvider
{
    internal class ServiceLogger : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            throw new NotImplementedException();
            return Logging.Factory.CreateLogger(categoryName);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
