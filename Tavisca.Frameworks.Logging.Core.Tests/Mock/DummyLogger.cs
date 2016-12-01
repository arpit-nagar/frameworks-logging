using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Tavisca.Frameworks.Logging.Configuration;

namespace Tavisca.Frameworks.Logging.Tests.Mock
{
    public class DummyLogger : Logger
    {
        public DummyLogger(IOptions<ApplicationLogSection> setting) : base(setting)
        { }

        protected override ISink GetLoggerByName(string name)
        {
            return new ExceptionThrowingSink();
        }
    }

    public class MockData
    {
        public static IOptions<ApplicationLogSection> GetDummyConfiguaration()
        {
            return new DummySettings();
        }
    }

    public class DummySettings : IOptions<ApplicationLogSection>
    {
        public ApplicationLogSection Value
        {
            get
            {
                return new ApplicationLogSection
                {
                    DefaultLogger = "RedisLogger",
                    Categories = new List<CategoryElement> {
                        //new CategoryElement { Name = "Default",  Loggers = new List<LoggerElement> { new LoggerElement {Name = "EventViewerLogger" } } },
                         new CategoryElement { Name = "RedisLogger",  Loggers = new List<LoggerElement> { new LoggerElement {Name = "RedisSink" } } },
                        // new CategoryElement { Name = "FileLogger",  Loggers = new List<LoggerElement> { new LoggerElement {Name = "FileLogger" } } },
                        // new CategoryElement { Name = "EventViewerLogger",  Loggers = new List<LoggerElement> { new LoggerElement {Name = "EventViewerLogger" } } },
                        // new CategoryElement { Name = "DBLogger",  Loggers = new List<LoggerElement> { new LoggerElement {Name = "DBLogger" } } },
                        // new CategoryElement { Name = "spDBLogger",  Loggers = new List<LoggerElement> { new LoggerElement {Name = "spDBLogger" } } },
                    },
                    TraceLoggers = new List<LoggerElement> { new LoggerElement { Name = "RedisLogger" } },
                    CompressionType = Infrastructure.CompressionTypeOptions.Zip,
                    ExceptionSwitch = SwitchOptions.On,
                    EventSwitch = SwitchOptions.On,
                    MaxThreads = 5,
                    ReThrowLogExceptions = SwitchOptions.Off,
                    CustomLocatorAdapter = "Tavisca.Frameworks.Logging.Extensions.DependencyInjection.Adapters.LogSpecificAdapter, Tavisca.Frameworks.Logging.Core.Extensions",
                    CustomFormatter = "Tavisca.Frameworks.Logging.Extensions.Formatters.CreditCardMaskFormatter,Tavisca.Frameworks.Logging.Core.Extensions",
                    CustomConfigurations = new List<Configuration.Configuration>
                    {
                        new Configuration.Configuration {Key = "Logging.RedislistKey", Value = "P-Dev-LoggingQueue"},
                        new Configuration.Configuration {Key = "Logging.RedisServerConnString", Value = "redis-dev.oski.tavisca.com:6379"},
                        new Configuration.Configuration {Key = "Tavisca.Frameworks.Logging.Extensions.Loggers.FileLogger.FilePath", Value = "D:\\temp\\mydata.log"},
                        new Configuration.Configuration {Key = "Tavisca.Frameworks.Logging.Extensions.Loggers.FileLogger.MaxFileSize", Value = "10485760"},
                        new Configuration.Configuration {Key = "Tavisca.Frameworks.Logging.Extensions.Formatters.ICreditCardMaskDataProvider", Value = "Tavisca.Frameworks.Logging.Tests.Mock.DummyCreditCardMaskProvider, Tavisca.Frameworks.Logging.Tests.Mock"}
                    }
                };
            }
        }
    }
}
