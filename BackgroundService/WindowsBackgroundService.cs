using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyServiceApi
{
    public class WindowsBackgroundService : BackgroundService
    {
        private readonly MyService _myService;
        private readonly ILogger<WindowsBackgroundService> _logger;

        public WindowsBackgroundService(
            MyService myService,
            ILogger<WindowsBackgroundService> logger) =>
            (_myService, _logger) = (myService, logger);

        // how do I get there?
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _myService.TestMethod();
                    _logger.LogWarning("{LOG}", "logging something");

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
