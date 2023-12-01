using LoggingService;

namespace CocoService
{
    public interface ICocoClass
    {
        string _cocoServiceName { get; }

        void SetCocoName(string cocoName);
        void WriteLog(string message);
    }

    public class CocoClass : ICocoClass
    {
        private readonly ILogService<CocoClass> _logger;
        public string _cocoServiceName { get; private set; }

        public CocoClass(ILogService<CocoClass> logger)
        {
            _logger = logger;
        }

        public void SetCocoName(string cocoName)
        {
            _cocoServiceName = cocoName;
        }

        public void WriteLog(string message)
        { 
            _logger.LogInformation(message);
        }
    }
}