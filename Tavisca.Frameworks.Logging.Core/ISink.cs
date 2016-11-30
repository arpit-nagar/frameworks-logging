using System;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Logging
{
    /// <summary>
    /// Defines the logger which actually writes the log entries to its target.
    /// </summary>
    public interface ISink
    {
        /// <summary>
        /// Writes the log entry asynchronously.
        /// </summary>
        /// <param name="entry">The <see cref="ILogEntry"/> to be logged.</param>
        void WriteAsync(ILogEntry entry);

        /// <summary>
        /// Writes the log entry asynchronously.
        /// </summary>
        /// <param name="entry">The <see cref="ILogEntry"/> to be logged.</param>
        /// <param name="continueWith">The continuation function to be executed 
        /// asynchronously after the write is completed.</param>
        void WriteAsync(ILogEntry entry, Action<Task> continueWith);

        /// <summary>
        /// Writes the log entry.
        /// </summary>
        /// <param name="entry">The <see cref="ILogEntry"/> to be logged.</param>
        void Write(ILogEntry entry);

        /// <summary>
        /// Writes an exception asynchrounously in the system.
        /// </summary>
        /// <param name="exception">The exception to be logged.</param>
        void WriteExceptionAsync(Exception exception);

        /// <summary>
        /// Writes an exception asynchrounously in the system.
        /// </summary>
        /// <param name="exception">The exception to be logged.</param>
        /// <param name="continueWith">The continuation function to be executed 
        /// asynchronously after the write is completed.</param>
        void WriteExceptionAsync(Exception exception, Action<Task> continueWith);

        /// <summary>
        /// Writes an exception in the system.
        /// </summary>
        /// <param name="exception">The exception to be logged.</param>
        void WriteException(Exception exception);
    }
}
