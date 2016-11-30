using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Tavisca.Frameworks.Logging.Configuration;
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

            //<CRITICAL .net core> /ConfigurationException and FileLogTraceListener does not supported in .net core
            if (string.IsNullOrWhiteSpace(path))
                throw new Exception(string.Format(LogExtensionResources.FileLogger_FilePath_NotFound, KeyStorage.AppSettingKeys.FileLoggerFilePath));

            if (string.IsNullOrWhiteSpace(data))
                return;

            const int maxRolledLogCount = 3;
            var maxLogSize = GetMaxFileSize();

            lock (path)
            {
                RollLogFile(path, maxRolledLogCount, maxLogSize);
                File.AppendAllText(path, data + Environment.NewLine, Encoding.UTF8);
            }
        }

        protected virtual string GetFilePath()
        {
            return ApplicationLogSetting.GetCustomConfiguration(KeyStorage.AppSettingKeys.FileLoggerFilePath);
        }

        protected virtual long GetMaxFileSize()
        {
            long fileSize;

            if (long.TryParse(ApplicationLogSetting.GetCustomConfiguration(KeyStorage.AppSettingKeys.MaxFileSize), out fileSize))
                return fileSize;

            return 10485760; //10 MB
        }

        //private static FileLogTraceListener _writer;
        //private static readonly object Locker = new object();

        //protected FileLogTraceListener GetWriter()
        //{
        //    if (_writer != null)
        //        return _writer;

        //    lock (Locker)
        //    {
        //        if (_writer == null)
        //        {
        //            var path = GetFilePath();

        //            var dirPath = System.IO.Path.GetDirectoryName(path);

        //            var rootFileName = System.IO.Path.GetFileNameWithoutExtension(path);

        //            var writer = new FileLogTraceListener()
        //            {
        //                Location = LogFileLocation.Custom,
        //                CustomLocation = dirPath,
        //                BaseFileName = rootFileName,
        //                MaxFileSize = GetMaxFileSize(),
        //                AutoFlush = true,
        //                LogFileCreationSchedule = LogFileCreationScheduleOption.Daily
        //            };

        //            _writer = writer;
        //        }
        //    }

        //    return _writer;
        //}

        private void RollLogFile(string logFilePath, int maxRolledLogCount, long maxLogSize)
        {
            try
            {
                var length = new FileInfo(logFilePath).Length;

                if (length > maxLogSize)
                {
                    var path = Path.GetDirectoryName(logFilePath);
                    var wildLogName = Path.GetFileNameWithoutExtension(logFilePath) + "*" + Path.GetExtension(logFilePath);
                    var bareLogFilePath = Path.Combine(path, Path.GetFileNameWithoutExtension(logFilePath));
                    string[] logFileList = Directory.GetFiles(path, wildLogName, SearchOption.TopDirectoryOnly);
                    if (logFileList.Length > 0)
                    {
                        // only take files like logfilename.log and logfilename.0.log, so there also can be a maximum of 10 additional rolled files (0..9)
                        var rolledLogFileList = logFileList.Where(fileName => fileName.Length == (logFilePath.Length + 2)).ToArray();
                        Array.Sort(rolledLogFileList, 0, rolledLogFileList.Length);
                        if (rolledLogFileList.Length >= maxRolledLogCount)
                        {
                            File.Delete(rolledLogFileList[maxRolledLogCount - 1]);
                            var list = rolledLogFileList.ToList();
                            list.RemoveAt(maxRolledLogCount - 1);
                            rolledLogFileList = list.ToArray();
                        }
                        // move remaining rolled files
                        for (int i = rolledLogFileList.Length; i > 0; --i)
                            File.Move(rolledLogFileList[i - 1], bareLogFilePath + "." + i + Path.GetExtension(logFilePath));
                        var targetPath = bareLogFilePath + ".0" + Path.GetExtension(logFilePath);
                        // move original file
                        File.Move(logFilePath, targetPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        #endregion
    }

    
}
