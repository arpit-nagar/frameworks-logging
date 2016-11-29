using System;
using System.Text;
using Tavisca.Frameworks.Logging.Infrastructure;
using Tavisca.Frameworks.Logging.Resources;

namespace Tavisca.Frameworks.Logging
{
    /// <summary>
    /// Default implementation of <see cref="IExceptionEntry"/>, this class can be inherited and extended.
    /// </summary>
    public class ExceptionEntry: LogEntryBase, IExceptionEntry
    {
        #region Private Members

        private readonly StringBuilder _innerExceptionBuilder;

        #endregion

        #region IExceptionEntry Members

        public string Type { get; set; }
        public string Source { get; set; }
        public string TargetSite { get; set; }
        public string StackTrace { get; set; }
        public string ThreadIdentity { get; set; }
        public string InnerExceptions
        {
            get { return _innerExceptionBuilder.ToString(); }
        }

        public void AddInnerExceptionMessage(string message)
        {
            _innerExceptionBuilder.AppendLine(message);
            _innerExceptionBuilder.AppendLine("--------------------------------------------");
        }

        public void AddInnerExceptionMessage(string message, bool finalValue)
        {
            _innerExceptionBuilder.AppendLine(message);
            
            if (!finalValue)
                _innerExceptionBuilder.AppendLine("--------------------------------------------");
        }

        public override ILogEntry CopyTo()
        {
            var entry = new ExceptionEntry();

            this.CopyTo(entry);

            entry.AddInnerExceptionMessage(this.InnerExceptions);

            entry.Source = this.Source;
            entry.StackTrace = this.StackTrace;
            entry.TargetSite = this.TargetSite;
            entry.ThreadIdentity = this.ThreadIdentity;
            entry.Type = this.Type;

            return entry;
        }

        #endregion

        #region Constructors

        public ExceptionEntry()
        {
            _innerExceptionBuilder = new StringBuilder();

            this.InitializeIntrinsicProperties();

            this.SeverityType = SeverityOptions.Error;
            this.Title = this.Severity;
        }

        #endregion
    }
}
