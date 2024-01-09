using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using System.Reflection;

namespace AspNetServiceLib
{
    public static class Logging
    {
        public static ILoggerFactory Factory { get; private set; } = new LoggerFactory();
        public static ILogger Logger { get; private set; } = Factory.CreateLogger("uninitialized");
        public static void Initialize(Type serviceType, string serviceName)
        {
            Factory = new LoggerFactory().AddNLog();
            var configFile = GetNLogConfigPath(serviceType, serviceName);
            LoggingConfiguration config;
            if (configFile == null)
            {
                //Create default config with console output only:
                config = new LoggingConfiguration();
                var consoleTarget = new ColoredConsoleTarget("target1");
                config.AddTarget(consoleTarget);
                config.AddRuleForAllLevels(consoleTarget);
            }
            else
            {
                config = new XmlLoggingConfiguration(configFile);
                config.Variables.Add("serviceName", serviceName);
                Directory.CreateDirectory(ServiceEnvironment.GetServiceWorkDir(serviceName));
                config.Variables.Add("workDir", ServiceEnvironment.GetServiceWorkDir(serviceName));
            }

            NLog.LogManager.Configuration = config;

            NLog.LogManager.Configuration.Install(new InstallationContext());
            Logger = Factory.CreateLogger(serviceName);
        }

        private static string GetNLogConfigPath(Type serviceType, string serviceName)
        {
            string nlogConfig = "nlog.config";
            string individualNlogConfig = $"{serviceName}.nlog";
            string serviceNlogConfig = $"{Path.GetFileNameWithoutExtension(serviceType.Assembly.FullName)}.nlog";

            //First look in service DLL dir, then in exe path and finally in current directory:
            var servicePath = Path.GetDirectoryName(serviceType.Assembly.Location);
            var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var currentPath = Directory.GetCurrentDirectory();
            foreach (var dir in new string[] { servicePath, exePath, currentPath })
            {
                var path = Path.Combine(dir, individualNlogConfig);
                if (File.Exists(path))
                {
                    return path;
                }

                path = Path.Combine(dir, serviceNlogConfig);
                if (File.Exists(path))
                {
                    return path;
                }

                path = Path.Combine(dir, nlogConfig);
                if (File.Exists(path))
                {
                    return path;
                }
            }

            return null;
        }
    }
}
