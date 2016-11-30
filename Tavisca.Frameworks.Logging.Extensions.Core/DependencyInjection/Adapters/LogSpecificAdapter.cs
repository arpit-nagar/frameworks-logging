using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using Tavisca.Frameworks.Logging.Extensions.Infrastructure;
using Tavisca.Frameworks.Logging.Extensions.Resources;
using Tavisca.Frameworks.Logging.Extensions.Sinks;
using Tavisca.Frameworks.Logging.Tracing;
using System.Reflection;
using Microsoft.Extensions.Options;
using Tavisca.Frameworks.Logging.Configuration;

namespace Tavisca.Frameworks.Logging.Extensions.DependencyInjection.Adapters
{
    /// <summary>
    /// An implementation of log specific adapter, inherit this class to override certain defaults or to extend the type
    /// of loggers supported.
    /// </summary>
    public class LogSpecificAdapter : ServiceLocatorImplBase
    {
        private IOptions<ApplicationLogSection> LoggerSetting;

        public LogSpecificAdapter(IOptions<ApplicationLogSection> settings)
        {
            LoggerSetting = settings;
        }

        #region ServiceLocatorImplBase Members

        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (typeof(ITransactionEntry).IsAssignableFrom(serviceType))
            {
                return new TransactionEntry();
            }

            if (typeof(IExceptionEntry).IsAssignableFrom(serviceType))
            {
                return new ExceptionEntry();
            }

            if (typeof(IEventEntry).IsAssignableFrom(serviceType))
            {
                return new EventEntry();
            }

            if (typeof(ISink).IsAssignableFrom(serviceType))
            {
                return GetSinkByKey(key) ?? GetDefaultSink();
            }

            if (typeof(IEntryStringTranslator).IsAssignableFrom(serviceType))
                return new EntryStringTranslator();

            if (typeof(ITraceLogger).IsAssignableFrom(serviceType))
                return new TraceLogger(LoggerSetting);

            throw new NotSupportedException(string.Format
                                                (
                                                    LogExtensionResources.LogSpecificAdapter_TypeNotSupported,
                                                    serviceType.FullName, key
                                                )
                                            );
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            yield return DoGetInstance(serviceType, null);
        }

        #endregion

        #region Protected Members

        protected virtual ISink GetDefaultSink()
        {
            return new FileSink();
        }

        protected virtual ISink GetSinkByKey(string key)
        {
            switch (key.ToUpper())
            {
                case "EVENTVIEWERSINK":
                    return new EventViewerSink();
                case "FILESINK":
                    return new FileSink();
                case "REDISSINK":
                    return new RedisSink();
                default:
                    return GetDefaultSink();
            }
        }

        #endregion
    }
}
