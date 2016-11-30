using System;
using System.Threading.Tasks;
using Tavisca.Frameworks.Logging.Configuration;

namespace Tavisca.Frameworks.Logging
{
    /// <summary>
    /// Defines the log factory.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets or sets a value whether all set limitations on which entry is to be logged is to be ignored
        /// causing all entries to be all logged. This property can be used by maintaining different instances
        /// of the this logger and choosing instance with this property set to "true" in specific circumstances.
        /// </summary>
        bool OverriddeEntryFilters { get; set; }

        /// <summary>
        /// Writes an entry into the system asynchronously.
        /// </summary>
        /// <param name="entry">The <see cref="ILogEntry"/> to be added.</param>
        /// <param name="category">The category key to be used, see <see cref="CategoryElement"/></param>
        void WriteAsync(ILogEntry entry, string category);

        /// <summary>
        /// Writes an entry into the system asynchronously.
        /// </summary>
        /// <param name="entry">The <see cref="ILogEntry"/> to be added.</param>
        /// <param name="category">The category key to be used, see <see cref="CategoryElement"/></param>
        /// <param name="continueWith">The continuation function to be executed 
        /// asynchronously after the write is completed.</param>
        void WriteAsync(ILogEntry entry, string category, Action<Task> continueWith);

        /// <summary>
        /// Delegates writing the entry into the configured <see cref="ISink"/>(s).
        /// </summary>
        /// <param name="entry">The <see cref="ILogEntry"/> to be added.</param>
        /// <param name="category">The category key to be used, see <see cref="CategoryElement"/></param>
        void Write(ILogEntry entry, string category);

        /// <summary>
        /// Delegates writing the exception into the configured <see cref="ISink"/>(s).
        /// </summary>
        /// <param name="exception">The exception to be logged.</param>
        /// <param name="category">The category key to be used, see <see cref="CategoryElement"/></param>
        void WriteExceptionAsync(Exception exception, string category);

        /// <summary>
        /// Delegates writing the exception asynchronously into the configured <see cref="ISink"/>(s).
        /// </summary>
        /// <param name="exception">The exception to be logged.</param>
        /// <param name="continueWith">The continuation function to be executed 
        /// asynchronously after the write is completed.</param>
        /// <param name="category">The category key to be used, see <see cref="CategoryElement"/></param>
        void WriteExceptionAsync(Exception exception, string category, Action<Task> continueWith);

        /// <summary>
        /// Delegates writing the exception into the configured <see cref="ISink"/>(s).
        /// </summary>
        /// <param name="exception">The exception to be logged.</param>
        /// <param name="category">The category key to be used, see <see cref="CategoryElement"/></param>
        void WriteException(Exception exception, string category);

        /// <summary>
        /// Refreshes the settings of the logger
        /// </summary>
        /// <param name="settings">The settings to use.</param>
        void RefreshSettings(IApplicationLogSettings settings);
    }
}
