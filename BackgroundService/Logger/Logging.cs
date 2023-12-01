using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace MyServiceApi.Logger
{
    public static class Logging
    {
        // recheck what it means and what it does
        [CompilerGenerated]
        private static ILoggerFactory m_A = new LoggerFactory();

        public static ILoggerFactory Factory
        {
            [CompilerGenerated]
            get
            {
                return Logging.m_A;
            }
            [CompilerGenerated]
            private set
            {
                Logging.m_A = value;
            }
        }
    }
}
