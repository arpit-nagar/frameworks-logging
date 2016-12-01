using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using Tavisca.Frameworks.Logging.Configuration;
using Tavisca.Frameworks.Logging.Infrastructure;

namespace Tavisca.Frameworks.Logging.Sample
{
    public static class Utility
    {
        private static readonly ILogger DefaultLogger = new Logger(MockData.GetDummyConfiguaration());

        private static readonly ILogger DoAllLogger = new Logger(MockData.GetDummyConfiguaration(),true);

        public static ILogger GetLogger()
        {
            var var = Environment.GetEnvironmentVariable("someVariable");

            if (string.IsNullOrWhiteSpace(var))
                return DefaultLogger;

            return DoAllLogger;
        }

        public static IExceptionEntry ToContextualEntry(this Exception exception)
        {
            var entry = exception.ToEntry();

            FillEntry(entry);

            return entry;
        }

        public static ITransactionEntry GetTransactionLogEntry()
        {
            var entry = new TransactionEntry();

            FillEntry(entry);

            return entry;
        }


        public static void FillEntry(ILogEntry entry)
        {
            entry.IpAddress = string.Empty;

            entry.UserIdentifier = string.Empty;
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
