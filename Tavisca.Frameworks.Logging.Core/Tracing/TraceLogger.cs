using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Tavisca.Frameworks.Logging.Configuration;

namespace Tavisca.Frameworks.Logging.Tracing
{
    /// <summary>
    /// See <see cref="ITraceLogger"/>.
    /// </summary>
    public class TraceLogger : Logger, ITraceLogger
    {
        #region Constructors

        public TraceLogger(IOptions<ApplicationLogSection> configurations) : base(configurations)
        { }

        public TraceLogger(IOptions<ApplicationLogSection> configurations, bool overriddeEntryFilters) : base(configurations, overriddeEntryFilters) { }

        #endregion

        #region Overridden Methods

        protected override IEnumerable<ISink> GetLoggers(string category)
        {
            var loggers = LogSection.TraceLoggers;

            if (loggers == null || loggers.Count == 0)
            {
                var defaultLogger = GetDefaultLogger(category);

                if (defaultLogger != null)
                    yield return defaultLogger;

                yield break;
            }

            foreach (LoggerElement loggerElement in loggers)
            {
                yield return GetLoggerByName(loggerElement.Name);
            }
        }

        #endregion
    }
}
