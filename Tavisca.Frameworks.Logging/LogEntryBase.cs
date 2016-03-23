using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using Tavisca.Frameworks.Logging.Infrastructure;

namespace Tavisca.Frameworks.Logging
{
    /// <summary>
    /// Default implementation of <see cref="ILogEntry"/>.
    /// </summary>
    public abstract class LogEntryBase : ILogEntry
    {
        protected readonly StringBuilder MessageBuilder;
        private string _sessionId;

        #region ILogEntry Members

        public Guid TracingToken { get; set; }

        public Guid Id { get; set; }

        /// <summary>
        /// Gets the logId, this might only be available after a save.
        /// </summary>
        public int LogId { get; set; }

        /// <summary>
        /// Gets the priority integer representation.
        /// </summary>
        public int Priority
        {
            get { return (int)PriorityType; }
        }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public PriorityOptions PriorityType { get; set; }

        /// <summary>
        /// Gets or sets the title of the log.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the timestamp associated with the log.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the machine name in which the entry was created.
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Gets or sets the appdomain in which the logger was functioning.
        /// </summary>
        public string AppDomainName { get; set; }

        /// <summary>
        /// Gets or sets the process id in which the logger was functioning.
        /// </summary>
        public string ProcessId { get; set; }

        /// <summary>
        /// Gets or sets the process name in which the logger was functioning.
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// Gets or sets the thread name in which the entry was logged.
        /// </summary>
        public string ThreadName { get; set; }

        /// <summary>
        /// Gets or sets the thread id in which the entry was created.
        /// </summary>
        public string Win32ThreadId { get; set; }

        /// <summary>
        /// Gets or sets any additional info associated with the log.
        /// </summary>
        public IDictionary<string, string> AdditionalInfo { get; protected set; }

        /// <summary>
        /// Gets or sets the message associated with the log, preferably use <seealso cref="AddMessage"/>
        /// </summary>
        public virtual string Message
        {
            get { return MessageBuilder.ToString(); }
            protected set
            {
                MessageBuilder.Length = 0;

                MessageBuilder.Append(value);
            }
        }

        /// <summary>
        /// Gets the severity string representation.
        /// </summary>
        public string Severity
        {
            get { return Enum.GetName(typeof(SeverityOptions), this.SeverityType); }
        }

        /// <summary>
        /// Gets or sets the severity.
        /// </summary>
        public SeverityOptions SeverityType { get; set; }

        /// <summary>
        /// Gets or sets the user session id associated with the request.
        /// </summary>
        public string UserSessionId { get; set; }

        /// <summary>
        /// Gets or sets the sessionId associated with the request.
        /// </summary>
        public virtual string SessionId
        {
            get { return _sessionId; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _sessionId = value;
            }
        }

        /// <summary>
        /// Gets or sets any user identifier asssociated with the request.
        /// </summary>
        public virtual string UserIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the ip address associated with the request.
        /// </summary>
        public virtual string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the application name in which the logger is functioning.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets any context identifier associated with the event.
        /// </summary>
        public string ContextIdentifier { get; set; }

        public void AddMessage(string message)
        {
            MessageBuilder.AppendLine(message);
        }

        public void AddAdditionalInfo(string key, string value)
        {
            AdditionalInfo[key] = value;
        }

        public abstract ILogEntry Clone();

        #endregion

        #region Constructors

        protected LogEntryBase()
        {
            MessageBuilder = new StringBuilder();
            AdditionalInfo = new Dictionary<string, string>();
        }

        #endregion

        #region Protected Methods

        protected void InitializeIntrinsicProperties()
        {
            this.Timestamp = GetCurrentTimeStamp();
            this.MachineName = GetCurrentMachineName();
            this.AppDomainName = GetCurrentAppDomainName();
            this.ApplicationName = GetCurrentApplicationName();

            var processInfo = GetCurrentProcessInfo();

            this.ProcessId = processInfo.Item1.ToString(CultureInfo.InvariantCulture);
            this.ProcessName = processInfo.Item2;
            this.ThreadName = GetCurrentThreadName();
            this.Win32ThreadId = GetCurrentWin32ThreadId();
            this.IpAddress = GetIPAddress();
        }

        protected void Clone<T>(T target) where T : ILogEntry
        {
            foreach (var pair in this.AdditionalInfo)
            {
                target.AddAdditionalInfo(pair.Key, pair.Value);
            }

            target.Id = this.Id;
            target.AppDomainName = this.AppDomainName;
            target.ApplicationName = this.ApplicationName;
            target.ContextIdentifier = this.ContextIdentifier;
            target.IpAddress = this.IpAddress;
            target.MachineName = this.MachineName;
            target.AddMessage(this.Message);
            target.PriorityType = this.PriorityType;
            target.ProcessId = this.ProcessId;
            target.ProcessName = this.ProcessName;
            target.SessionId = this.SessionId;
            target.SeverityType = this.SeverityType;
            target.ThreadName = this.ThreadName;
            target.Timestamp = this.Timestamp;
            target.Title = this.Title;
            target.UserIdentifier = this.UserIdentifier;
            target.UserSessionId = this.UserSessionId;
            target.Win32ThreadId = this.Win32ThreadId;
            target.TracingToken = this.TracingToken;
        }

        protected virtual DateTime GetCurrentTimeStamp()
        {
            return DateTime.UtcNow;
        }

        private static string _machineName;
        protected virtual string GetCurrentMachineName()
        {
            try
            {
                return _machineName ?? (_machineName = Environment.MachineName);
            }
            catch
            {
                return null;
            }
        }

        private static string _appDomainName;
        protected virtual string GetCurrentAppDomainName()
        {
            try
            {
                return _appDomainName ?? (_appDomainName = AppDomain.CurrentDomain.FriendlyName);
            }
            catch
            {
                return null;
            }
        }

        private static string _applicationName;
        protected virtual string GetCurrentApplicationName()
        {
            if (_applicationName != null)
                return _applicationName;

            try
            {
                if (AppDomain.CurrentDomain.ActivationContext != null && AppDomain.CurrentDomain.ActivationContext.Identity != null)
                    return (_applicationName = AppDomain.CurrentDomain.ActivationContext.Identity.FullName);

                if (AppDomain.CurrentDomain.ApplicationIdentity != null)
                    return (_applicationName = AppDomain.CurrentDomain.ApplicationIdentity.FullName);

                return null;
            }
            catch
            {
                return null;
            }
        }

        private static Tuple<int, string> _currentProcessInfo;
        protected virtual Tuple<int, string> GetCurrentProcessInfo()
        {
            if (_currentProcessInfo != null)
                return _currentProcessInfo;

            try
            {
                var process = System.Diagnostics.Process.GetCurrentProcess();

                return (_currentProcessInfo = new Tuple<int, string>(process.Id, process.ProcessName));
            }
            catch
            {
                return null;
            }
        }

        protected virtual string GetCurrentThreadName()
        {
            try
            {
                return System.Threading.Thread.CurrentThread.Name;
            }
            catch
            {
                return null;
            }
        }

        protected virtual string GetCurrentWin32ThreadId()
        {
            try
            {
                return System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        private static string _ipAdresses= null;
        protected virtual string GetIPAddress()
        {
            try
            {
                if (_ipAdresses == null)
                {
                    var sb = new StringBuilder();
                    var ipEntry = Dns.GetHostEntry(Dns.GetHostName());
                    var addrList = ipEntry.AddressList;
                    if (addrList != null)
                    {
                        foreach (IPAddress addr in addrList)
                        {
                            sb.AppendFormat("{0},", addr);
                        }
                    }

                    if (sb.Length > 0)
                        sb.Remove(sb.Length - 1, 1);
                    _ipAdresses = sb.ToString();
                }
            }
            catch
            {
                _ipAdresses = string.Empty;
            }
            return _ipAdresses;
        }

        #endregion
    }
}
