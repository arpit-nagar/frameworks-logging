using System.Configuration;
using System.Diagnostics;
using Tavisca.Frameworks.Logging.Extensions.Infrastructure;
using Tavisca.Frameworks.Logging.Extensions.Settings;

namespace Tavisca.Frameworks.Logging.Extensions.Sinks
{
    /// <summary>
    /// A windows event writer implementation of a sink. Converts the objects into a string format
    /// using <see cref="IEntryStringTranslator"/> obtained from the base class <see cref="StringWritingSinkBase"/>.
    /// </summary>
    public class EventViewerSink : StringWritingSinkBase
    {
        #region Constants

        protected const string LogCategory = "Application";
        protected const string DefaultSource = "Tavisca.Frameworks.Logging.Extensions";
        private static string _source;

        #endregion

        #region StringWritingLoggerBase Methods

        protected override void WriteTransaction(ITransactionEntry transactionEntry)
        {
            var translator = GetTranslator();

            var data = translator.TranslateEvent(transactionEntry);

            WriteToEventViewer(GetSource(), data, EventLogEntryType.Information);
        }

        protected override void WriteException(IExceptionEntry eventEntry)
        {
            var translator = GetTranslator();

            var data = translator.TranslateException(eventEntry);

            WriteToEventViewer(GetSource(), data, EventLogEntryType.Error);
        }

        #endregion

        #region Protected Members

        protected void WriteToEventViewer(string source, string data, EventLogEntryType logEntryType)
        {
            if (string.IsNullOrWhiteSpace(data))
                return;

            if (!EventLog.SourceExists(source))
                EventLog.CreateEventSource(source, LogCategory);

            EventLog.WriteEntry(source, data, logEntryType);
        }

        protected virtual string GetSource()
        {
            if (!string.IsNullOrWhiteSpace(_source))
                return _source;

            var source = ConfigurationManager.AppSettings[KeyStorage.AppSettingKeys.EventVwrSource];

            _source = string.IsNullOrWhiteSpace(source) ? DefaultSource : source;

            return _source;
        }

        #endregion
    }
}
