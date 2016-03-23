using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tavisca.Frameworks.Logging.Infrastructure;

namespace Tavisca.Frameworks.Logging.Sample
{
    public static class Utility
    {
        private static readonly ILogger DefaultLogger = new Logger();

        private static readonly ILogger DoAllLogger = new Logger(true);

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

        public static IEventEntry GetLogEntry()
        {
            var entry = new EventEntry();

            FillEntry(entry);

            return entry;
        }


        public static void FillEntry(ILogEntry entry)
        {
            entry.PriorityType = PriorityOptions.Medium;

            entry.IpAddress = string.Empty;

            entry.UserIdentifier = string.Empty;

            entry.UserSessionId = string.Empty;
        }
    }
}
