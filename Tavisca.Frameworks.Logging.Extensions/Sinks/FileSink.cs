using System.Configuration;
using Microsoft.VisualBasic.Logging;
using Tavisca.Frameworks.Logging.Extensions.Infrastructure;
using Tavisca.Frameworks.Logging.Extensions.Resources;
using Tavisca.Frameworks.Logging.Extensions.Settings;

namespace Tavisca.Frameworks.Logging.Extensions.Sinks
{
    /// <summary>
    /// A file writer implementation of a sink. Converts the objects into a string format
    /// using <see cref="IEntryStringTranslator"/> obtained from the base class <see cref="StringWritingSinkBase"/>.
    /// Causes a rollover of one day for all files, default max-file size is 10mb.
    /// </summary>
    public class FileSink : StringWritingSinkBase
    {
        #region StringWritingLoggerBase Members

        protected override void WriteTransaction(ITransactionEntry transactionEntry)
        {
            var translator = GetTranslator();

            var data = translator.TranslateTransaction(transactionEntry);

            WriteToFile(data);
        }

        protected override void WriteException(IExceptionEntry eventEntry)
        {
            var translator = GetTranslator();

            var data = translator.TranslateException(eventEntry);

            WriteToFile(data);
        }

        protected override void WriteEvent(IEventEntry eventEntry)
        {
            var translator = GetTranslator();

            var data = translator.TranslateEvent(eventEntry);

            WriteToFile(data);
        }

        #endregion

        #region Protected Members

        protected virtual void WriteToFile(string data)
        {
            var path = GetFilePath();

            if (string.IsNullOrWhiteSpace(path))
                throw new ConfigurationErrorsException(string.Format(LogExtensionResources.FileLogger_FilePath_NotFound, KeyStorage.AppSettingKeys.FileLoggerFilePath));

            GetWriter().WriteLine(data);
        }

        protected virtual string GetFilePath()
        {
            return ConfigurationManager.AppSettings[KeyStorage.AppSettingKeys.FileLoggerFilePath];
        }

        protected virtual long GetMaxFileSize()
        {
            long fileSize;

            if (long.TryParse(ConfigurationManager.AppSettings[KeyStorage.AppSettingKeys.MaxFileSize], out fileSize))
                return fileSize;

            return 10485760; //10 MB
        }

        private static FileLogTraceListener _writer;
        private static readonly object Locker = new object();

        protected FileLogTraceListener GetWriter()
        {
            if (_writer != null)
                return _writer;

            lock (Locker)
            {
                if (_writer == null)
                {
                    var path = GetFilePath();

                    var dirPath = System.IO.Path.GetDirectoryName(path);

                    var rootFileName = System.IO.Path.GetFileNameWithoutExtension(path);

                    var writer = new FileLogTraceListener()
                    {
                        Location = LogFileLocation.Custom,
                        CustomLocation = dirPath,
                        BaseFileName = rootFileName,
                        MaxFileSize = GetMaxFileSize(),
                        AutoFlush = true,
                        LogFileCreationSchedule = LogFileCreationScheduleOption.Daily
                    };

                    _writer = writer;
                }
            }

            return _writer;
        }

        #endregion
    }
}
