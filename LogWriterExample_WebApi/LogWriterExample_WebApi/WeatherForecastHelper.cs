using LoggingService;
using LogWriterExample_WebApi.Controllers;

namespace LogWriterExample_WebApi
{

    public interface IWeatherForecastHelper
    {
        void WriteALogMessage(string message);
    }

    public class WeatherForecastHelper : IWeatherForecastHelper
    {
        protected readonly ILogService<WeatherForecastHelper> _logger;

        public WeatherForecastHelper(ILogService<WeatherForecastHelper> logger)
        {
            _logger = logger;
        }

        public void WriteALogMessage(string message)
        {
            _logger.LogInformation($"COCO : {message}");
        }
    }
}
