using Microsoft.Extensions.Logging;

namespace LoggingService
{

    public class LogService<T> : ILogService<T>
    {
        private readonly ILogger<T> _logger;

        public LogService(ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger<T>();
        }

        public void LogError(string message)
        {
            _logger.LogError(message);
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }
    }
}