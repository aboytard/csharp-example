using CocoService;
using LoggingService;
using System.Collections.Concurrent;

namespace LogWriterExample_WebApi
{
    public interface ICocoWrapper
    {
        void AddCoco(string cocoName);
    }

    public class CocoWrapper : ICocoWrapper
    {
        private ConcurrentDictionary<string, ICocoClass> _cocos;

        private readonly ILogService<CocoWrapper> _logger;
        public ICocoClass _coco { get; private set; }

        public CocoWrapper(ILogService<CocoWrapper> logger, ICocoClass coco)
        {
            _logger = logger;
            _coco = coco;
        }

        public void AddCoco(string cocoName)
        {
            _coco.SetCocoName(cocoName);
            _cocos.TryAdd(cocoName, _coco);
            _logger.LogInformation($" Added : {_cocos[cocoName]._cocoServiceName}");
        }
    }
}
