using CocoService;
using LoggingService;
using Microsoft.AspNetCore.Mvc;

namespace LogWriterExample_WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {        
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogService<WeatherForecastController> _logger;

        private readonly IWeatherForecastHelper _weatherForecastHelper;
        private readonly ICocoClass _coco;

        public WeatherForecastController(ILogService<WeatherForecastController> logger,
            IWeatherForecastHelper weatherForecastHelper,
            ICocoClass coco
            )
        {
            _logger = logger;
            _logger.LogInformation("WeatherForecastController has been initialized");
            _weatherForecastHelper = weatherForecastHelper;
            _coco = coco;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {

            _logger.LogInformation($"Test Log with date {DateTime.UtcNow.ToLongTimeString()}");
            _weatherForecastHelper.WriteALogMessage("test");
            _coco.SetCocoName("KIKI");
            _coco.WriteLog(_coco._cocoServiceName);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}