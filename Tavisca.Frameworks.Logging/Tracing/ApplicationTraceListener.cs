using System.Diagnostics;
using Tavisca.Frameworks.Logging.DependencyInjection;

namespace Tavisca.Frameworks.Logging.Tracing
{
    /// <summary>
    /// A <see cref="TraceListener"/> derived object, used as the primary listener for tracing events in the
    /// application. This calls the <see cref="ITraceLogger"/> which manages the actual delegation of logging.
    /// </summary>
    public class ApplicationTraceListener: TraceListener
    {
        #region Fields

        protected const string ApplicationTraceConfigurationKey = "na"; //Do not remove

        #endregion

        #region TraceListener Methods

        public override void Write(string message)
        {
            var entry = GetLogEntry();

            entry.AddMessage(message);
            
            GetTraceLogFactory().WriteAsync(entry, ApplicationTraceConfigurationKey);
        }

        public override void WriteLine(string message)
        {
            this.Write(message);
        }

        #endregion

        #region Methods

        protected virtual ITraceLogger GetTraceLogFactory()
        {
            return LocatorProvider.GetContainer().GetInstance<ITraceLogger>();
        }

        protected virtual ILogEntry GetLogEntry()
        {
            return LocatorProvider.GetContainer().GetInstance<IEventEntry>();
        }

        #endregion
    }
}
