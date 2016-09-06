using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Tavisca.Frameworks.Logging.Extensions.Infrastructure;
using Tavisca.Frameworks.Logging.Extensions.Sinks;

namespace Tavisca.Frameworks.Logging.Tests.Mock
{
    public class DummyLocatorAdapter : ServiceLocatorImplBase
    {
        protected override object DoGetInstance(Type serviceType, string key)
        {
            switch (key)
            {
                case KeyStore.Loggers.DBLogger:
                    return new DBSink();
                case KeyStore.Loggers.EventViewerLoggerMock:
                    return new EventViewerSink();
                case KeyStore.Loggers.EventViewerLogger:
                    return new Extensions.Sinks.EventViewerSink();
                case KeyStore.Loggers.FileLogger:
                    return new FileSink();
                case KeyStore.Loggers.RedisSink:
                    return new RedisSink();

            }

            if (typeof(ISink).IsAssignableFrom(serviceType))
                return new ExceptionThrowingSink();

            if (typeof(IExceptionEntry).IsAssignableFrom(serviceType))
                return new ExceptionEntry();

            if (typeof(ITransactionEntry).IsAssignableFrom(serviceType))
                return new TransactionEntry();

            if (typeof(IEntryStringTranslator).IsAssignableFrom(serviceType))
                return new EntryStringTranslator();

            throw new NotImplementedException();
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            yield return DoGetInstance(serviceType, null);
        }
    }
}
