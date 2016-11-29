using System.Collections.Generic;
using Tavisca.Frameworks.Logging.Infrastructure;

namespace Tavisca.Frameworks.Logging.Configuration
{
    public class ApplicationLogSection : IApplicationLogSettings
    {
        public SwitchOptions ExceptionSwitch { get; set; }

        public SwitchOptions EventSwitch { get; set; }

        public SwitchOptions ReThrowLogExceptions { get; set; }

        public int MaxThreads { get; set; }

        public bool UseWorkerProcessThreads { get; set; }

        public PriorityOptions MinPriority { get; set; }

        public string LogConfigurationProvider { get; set; }

        public string CustomLocatorAdapter { get; set; }

        public string CustomFormatter { get; set; }

        public string DefaultLogger { get; set; }

        public List<CategoryElement> Categories { get; set; }

        public List<LoggerElement> TraceLoggers { get; set; }

        public CompressionTypeOptions CompressionType { get; set; }

        public string CustomCompressionType { get; set; }
    }
}
