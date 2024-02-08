using Microsoft.Extensions.Logging;

namespace MainService
{
    partial class Program
    {
        public static void Main(string[] args)
        {
            using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger logger = factory.CreateLogger("Program");

            logger.LogInformation("Hello World! Logging is {Description}.", "fun");
            LogStartupMessage(logger, "fun");

            ILogger logger1 = factory.CreateLogger<Program>();
            logger1.LogInformation("Hello World! Logging is {Description}.", "fun");

            Console.ReadKey();
        }

        [LoggerMessage(Level = LogLevel.Information, Message = "Hello World! Logging is {Description}.")]
        static partial void LogStartupMessage(ILogger logger, string description);
    }
}
