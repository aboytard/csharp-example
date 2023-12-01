using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyServiceApi.Logger;

namespace MyServiceApi
{
    public class MyServiceManager : WindowsBackgroundService
    {
        public IHost WebHost { get; private set; }

        public MyServiceManager(MyService myService, ILogger<WindowsBackgroundService> logger) : base(myService, logger)
        {
        }

        public void Reconfigure()
        {
            // here are all those parameters harcoded --> to change
            MainConfiguration config = new();
            string myServiceWorkDir = "C:\\Users\\a00542157\\Documents\\TestMyService";
            WebHost = CreateHostBuilder(config.ListeningUrl, myServiceWorkDir).Build();
            Console.WriteLine("WebHost Created and start Running");
            WebHost.Run(); // I block the host thread in that case
        }

        public override void Dispose()
        {
            WebHost?.Dispose();
            base.Dispose(); 
        }

        public static IHostBuilder CreateHostBuilder(string listeningUrl, string workDir) =>
            Host.CreateDefaultBuilder(new string[0])
                .UseWindowsService( options =>
                {
                    options.ServiceName = "MyService";
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSetting("workDir", workDir);
                    webBuilder.UseUrls(listeningUrl);
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(loggingBuilder =>

                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(LogLevel.Debug);
                    loggingBuilder.AddProvider(new MyServiceLogger());
                });

    }
}
