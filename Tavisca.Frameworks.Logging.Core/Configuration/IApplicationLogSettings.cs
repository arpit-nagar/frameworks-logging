using System.Collections.Generic;
using Tavisca.Frameworks.Logging.Infrastructure;

namespace Tavisca.Frameworks.Logging.Configuration
{
    /// <summary>
    /// Configuration definition for the logging framework.
    /// </summary>
    public interface IApplicationLogSettings
    {
        /// <summary>
        /// Gets or sets a switch value which determines whether the framework will log exceptions or not. 
        /// This is a universal switch.
        /// </summary>
        SwitchOptions ExceptionSwitch { get; set; }

        /// <summary>
        /// Gets or sets a switch value which determines whether the framework will log events or not. 
        /// This is a universal switch.
        /// </summary>
        SwitchOptions EventSwitch { get; set; }

        /// <summary>
        /// Gets or sets a value which determines whether the framework should rethrow the exception once encountered.
        /// This might cause the application to crash, however ideally a sink should not ever throw an exception and should be allowed to crash in case of errors.
        /// </summary>
        SwitchOptions ReThrowLogExceptions { get; set; }

        /// <summary>
        /// Gets or sets the categories defining the loggers along with key value pairs.
        /// </summary>
        List<CategoryElement> Categories { get; set; }

        /// <summary>
        /// Gets or sets tracing loggers which handles all the trace related logging, the <see cref="ApplicationTraceListener"/> should be configured.
        /// </summary>
        List<LoggerElement> TraceLoggers { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of threads that will be used for logging, if the limit breaches, 
        /// subsequent requests are queued, this will give a performance boost in most scenarios.
        /// </summary>
        int MaxThreads { get; set; }

        /// <summary>
        /// Gets or sets whether the library should use threads from the worker process or create new ones.
        /// </summary>
        bool UseWorkerProcessThreads { get; set; }

        /// <summary>
        /// Gets or sets the minimum priority required in order for a log to pass through to a sink.
        /// </summary>
        PriorityOptions MinPriority { get; set; }

        /// <summary>
        /// Gets or sets an assembly qualified name of a configuration provider; 
        /// type must implement <see cref="IConfigurationProvider"/>
        /// </summary>
        string LogConfigurationProvider { get; set; }

        /// <summary>
        /// Gets or sets an assembly qualified name of a custom service locator, this type must
        /// implement <see cref="IServiceLocator"/>
        /// </summary>
        string CustomLocatorAdapter { get; set; }

        /// <summary>
        /// Gets or sets the custom formatter which is called each time by before an entry is logged.
        /// The formatter must implement <see cref="ILogEntryFormatter"/>. For custom formatting also 
        /// consider inheriting <see cref="DefaultFormatter"/> and overriding appropriate members.
        /// </summary>
        string CustomFormatter { get; set; }

        /// <summary>
        /// Gets or sets the default logger for the framework.
        /// </summary>
        string DefaultLogger { get; set; }

        /// <summary>
        /// Gets or sets the default string compression type.
        /// </summary>
        CompressionTypeOptions CompressionType { get; set; }

        /// <summary>
        /// Gets or sets the fully qualified name of custom compression type.
        /// </summary>
        string CustomCompressionType { get; set; }

    }
}