using System;
using System.Diagnostics;
using System.Text;
using Tavisca.Frameworks.Logging.Infrastructure;

namespace Tavisca.Frameworks.Logging
{
    /// <summary>
    /// A <see cref="IFailSafeLogger"/> implementation, logs in the event viewer.
    /// </summary>
    public sealed class EventViewerLogger : IFailSafeLogger
    {
        private const string Source = "Tavisca.Frameworks.Logging";
        private const string LogCategory = "Application";

        public void Log(Exception ex)
        {
            try
            {
                if (!EventLog.SourceExists(Source))
                    EventLog.CreateEventSource(Source, LogCategory);

                IExceptionEntry e;
                try
                {
                    e = ex.ToEntry();
                }
                catch (Exception)
                {
                    var formatter = new Formatting.DefaultFormatter();

                    e = formatter.FormatException(ex, new ExceptionEntry()); //in case the DI/custom formatter is failing.
                }

                var sb = new StringBuilder()
                    .Append("Application Name: ")
                    .AppendLine(string.IsNullOrWhiteSpace(e.ApplicationName) ? "<was blank>" : e.ApplicationName)
                    .Append("Exception Type: ")
                    .AppendLine(e.Type)
                    .AppendLine("---------Message-------------")
                    .AppendLine(e.Message)
                    .AppendLine(string.Empty).AppendLine("-----------Stack Trace-----------------")
                    .AppendLine(e.StackTrace)
                    .AppendLine("---------Process Name-------------")
                    .AppendLine(e.ProcessName)
                    .AppendLine("---------Entry Assembly Name-------------")
                    .AppendLine(GetEntryAssemblyName())
                    .AppendLine("---------Session Id-------------")
                    .AppendLine("---------Inner Exceptions-------------")
                    .AppendLine(e.InnerExceptions);

                EventLog.WriteEntry(Source, sb.ToString(), EventLogEntryType.Error);
            }
            catch { }
        }

        private string GetEntryAssemblyName()
        {
            try
            {
                var entryAsembly = System.Reflection.Assembly.GetEntryAssembly();

                return entryAsembly != null ? entryAsembly.GetName().Name : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
