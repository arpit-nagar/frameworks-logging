using System.Configuration;
using Tavisca.Frameworks.Logging.Infrastructure;

namespace Tavisca.Frameworks.Logging.Configuration
{
    public sealed class ApplicationLogSection : ConfigurationSection, IApplicationLogSettings
    {
        [ConfigurationProperty("exceptionSwitch", IsRequired = false, DefaultValue = SwitchOptions.On)]
        public SwitchOptions ExceptionSwitch
        {
            get { return (SwitchOptions)this["exceptionSwitch"]; }
            set { this["exceptionSwitch"] = value; }
        }

        [ConfigurationProperty("eventSwitch", IsRequired = false, DefaultValue = SwitchOptions.On)]
        public SwitchOptions EventSwitch
        {
            get { return (SwitchOptions)this["eventSwitch"]; }
            set { this["eventSwitch"] = value; }
        }

        [ConfigurationProperty("reThrowLogExceptions", IsRequired = false, DefaultValue = SwitchOptions.On)]
        public SwitchOptions ReThrowLogExceptions
        {
            get { return (SwitchOptions)this["reThrowLogExceptions"]; }
            set { this["reThrowLogExceptions"] = value; }
        }

        [ConfigurationProperty("maxThreads", IsRequired = false, DefaultValue = 50)]
        public int MaxThreads
        {
            get { return (int)this["maxThreads"]; }
            set { this["maxThreads"] = value; }
        }

        [ConfigurationProperty("useWorkerProcessThreads", IsRequired = false, DefaultValue = true)]
        public bool UseWorkerProcessThreads
        {
            get { return (bool)this["useWorkerProcessThreads"]; }
            set { this["useWorkerProcessThreads"] = value; }
        }

        [ConfigurationProperty("minPriority", IsRequired = false, DefaultValue = PriorityOptions.Undefined)]
        public PriorityOptions MinPriority
        {
            get { return (PriorityOptions)this["minPriority"]; }
            set { this["minPriority"] = value; }
        }

        [ConfigurationProperty("logConfigurationProvider", IsRequired = false)]
        public string LogConfigurationProvider
        {
            get { return (string)this["logConfigurationProvider"]; }
            set { this["logConfigurationProvider"] = value; }
        }

        [ConfigurationProperty("customLocatorAdapter", IsRequired = false)]
        public string CustomLocatorAdapter
        {
            get { return (string)this["customLocatorAdapter"]; }
            set { this["customLocatorAdapter"] = value; }
        }

        [ConfigurationProperty("customFormatter", IsRequired = false)]
        public string CustomFormatter
        {
            get { return (string)this["customFormatter"]; }
            set { this["customFormatter"] = value; }
        }

        [ConfigurationProperty("defaultLogger", IsRequired = false)]
        public string DefaultLogger
        {
            get { return (string)this["defaultLogger"]; }
            set { this["defaultLogger"] = value; }
        }

        [ConfigurationProperty("categories", IsDefaultCollection = true)]
        public CategoryElementCollection Categories
        {
            get { return (CategoryElementCollection)this["categories"]; }
            set { this["categories"] = value; }
        }

        [ConfigurationProperty("traceLoggers", IsDefaultCollection = false, IsRequired = false)]
        public LoggerElementCollection TraceLoggers
        {
            get { return (LoggerElementCollection)this["traceLoggers"]; }
            set { this["traceLoggers"] = value; }
        }

        [ConfigurationProperty("compressionType", IsRequired = false, DefaultValue = CompressionTypeOptions.Deflate)]
        public CompressionTypeOptions CompressionType
        {
            get { return (CompressionTypeOptions)this["compressionType"]; }
            set { this["compressionType"] = value; }
        }

        [ConfigurationProperty("customCompressionType", IsRequired = false, DefaultValue = "")]
        public string CustomCompressionType
        {
            get { return (string)this["customCompressionType"]; }
            set { this["customCompressionType"] = value; }
        }
    }
}
