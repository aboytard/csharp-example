using CocoService;
using LoggingService;
using LogWriterExample_WebApi;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();

        var config = new ConfigurationBuilder()
           .SetBasePath(System.IO.Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddEnvironmentVariables()
           .Build();

        NLog.LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));

        var logger = LogManager.Setup()
                       .LoadConfigurationFromAppSettings()
                       .GetCurrentClassLogger();

        // this one is missing
        builder.Logging.AddNLog();

        logger.Info("Program Main Nlog-logger configured");
        // THIS one is useless
        //builder.WebHost.UseNLog();

        // register the singleton logger in the io
        builder.Services.AddSingleton(typeof(ILogService<>), typeof(LogService<>));


        builder.Services.AddTransient<IWeatherForecastHelper, WeatherForecastHelper>();
        builder.Services.AddTransient<ICocoClass, CocoClass>();

        builder.Services.AddSingleton<ICocoWrapper, CocoWrapper>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}