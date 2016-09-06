using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tavisca.Frameworks.Logging.Formatting
{
    /// <summary>
    /// Translates exceptions into entries and acts as an injection point for adding information into the system.
    /// </summary>
    public interface ILogEntryFormatter
    {
        /// <summary>
        /// Formats the event, adds any information required into it.
        /// </summary>
        /// <param name="transactionEntry">The entry to be formatted.</param>
        /// <returns>Formatted entry.</returns>
        ITransactionEntry FormatEvent(ITransactionEntry transactionEntry);

        /// <summary>
        /// Formats the exception, adds any information required into it.
        /// </summary>
        /// <param name="exceptionEntry">The exception to format.</param>
        /// <returns>Formatted exception.</returns>
        IExceptionEntry FormatException(IExceptionEntry exceptionEntry);

        /// <summary>
        /// Translates the exception into an entry and adds any required information into it.
        /// </summary>
        /// <param name="exception">The excpetion to format.</param>
        /// <returns>Formatted exception.</returns>
        IExceptionEntry FormatException(Exception exception);
    }
}
