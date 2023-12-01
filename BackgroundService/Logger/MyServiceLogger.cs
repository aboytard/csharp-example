using Microsoft.Extensions.Logging;

namespace MyServiceApi.Logger
{
    public class MyServiceLogger : ILoggerProvider
    {

        public ILogger CreateLogger(string categoryName)
        {
            return Logging.Factory.CreateLogger(categoryName);
        }

        public void Dispose()
        {
        }
    }
}
