using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Tavisca.Frameworks.Logging.DependencyInjection;
using Tavisca.Frameworks.Logging.Exceptions;
using Tavisca.Frameworks.Logging.Infrastructure;

namespace Tavisca.Frameworks.Logging.Formatting
{
    /// <summary>
    /// The default formatter for formatting exceptions, 
    /// this class can be inherited for ease of translating exceptions into entries.
    /// </summary>
    public class DefaultFormatter : ILogEntryFormatter
    {
        #region ILogEntryFormatter Members

        /// <summary>
        /// Default implementation, returns the object with the request and response compressed, 
        /// this method should be overridden in case of any customizations 
        /// required e.g. masking capability addition. To keep compression, the base method should be called 
        /// at the end.
        /// Also think of overriding <seealso cref="FormatException(Tavisca.Frameworks.Logging.IExceptionEntry)"/>
        /// </summary>
        /// <param name="transactionEntry">The event entry to be formatted.</param>
        /// <returns>Formatted event.</returns>
        public virtual ITransactionEntry FormatTransaction(ITransactionEntry transactionEntry)
        {
            var request = transactionEntry.Request;

            if (!string.IsNullOrWhiteSpace(request))
            {
                request = request.Compress();

                transactionEntry.SetRequestString(request);
            }

            var response = transactionEntry.Response;

            if (!string.IsNullOrWhiteSpace(response))
            {
                response = response.Compress();

                transactionEntry.SetResponseString(response);
            }

            return transactionEntry;
        }

        public IEventEntry FormatEvent(IEventEntry eventEntry)
        {
            var payload = eventEntry.Payload;

            if (!string.IsNullOrWhiteSpace(payload))
            {
                payload = payload.Compress();

                eventEntry.SetPayloadString(payload);
            }
            return eventEntry;
        }

        /// <summary>
        /// Default implementation, returns the object "as is", this method should be overridden in 
        /// case of any customizations required e.g. masking capability addition.
        /// Also think of overriding <seealso cref="FormatTransaction"/>
        /// </summary>
        /// <param name="exceptionEntry">The exception entry to be formatted.</param>
        /// <returns>Formatted exception.</returns>
        public virtual IExceptionEntry FormatException(IExceptionEntry exceptionEntry)
        {
            return exceptionEntry;
        }

        /// <summary>
        /// Responsible for converting exceptions into <seealso cref="IExceptionEntry"/> object, 
        /// gets the entry empty object and passes to <seealso cref="FormatException(System.Exception, Tavisca.Frameworks.Logging.IExceptionEntry)"/>
        /// The method calls <seealso cref="FormatException(Tavisca.Frameworks.Logging.IExceptionEntry)"/> at the end for
        /// final formatting, override this method only to change how an exception is converted to an entry.
        /// </summary>
        /// <param name="exception">The exception entry to be converted.</param>
        /// <returns>An <see cref="IExceptionEntry"/> object.</returns>
        public virtual IExceptionEntry FormatException(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");

            var entry = LocatorProvider.GetContainer().GetInstance<IExceptionEntry>();

            if (entry == null)
                throw new LogConfigurationException("IExceptionEntry is not configured in the DI container.");

            entry = FormatException(exception, entry);

            return FormatException(entry);
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Formats and translates the exception into an <see cref="IExceptionEntry"/> object.
        /// </summary>
        /// <param name="exception">The exception to format.</param>
        /// <param name="entry">The <see cref="IExceptionEntry"/> object to format.</param>
        /// <returns>The entry object provided with excpetion information added.</returns>
        protected internal virtual IExceptionEntry FormatException(Exception exception, IExceptionEntry entry)
        {
            if (exception.InnerException != null)
            {
                entry.AddInnerExceptionMessage(exception.InnerException.ToString());

                var builder = new StringBuilder();

                var innerException = exception.InnerException;
                builder.AppendLine("----Flattened Exception Data[BEGIN]----");
                while (innerException != null)
                {
                    foreach (var pair in innerException.Data.Cast<DictionaryEntry>())
                    {
                        builder.Append(pair.Key).Append(":").Append(pair.Value).AppendLine("---");
                    }

                    innerException = innerException.InnerException;
                }
                builder.AppendLine("----Flattened Exception Data[END]----");

                entry.AddInnerExceptionMessage(builder.ToString());
            }

            entry.AddMessage(exception.Message);

            entry.Source = exception.Source;

            entry.Type = exception.GetType().Name;

            entry.StackTrace = exception.StackTrace;

            entry.TargetSite = exception.TargetSite == null ? string.Empty : exception.TargetSite.Name;

            if (exception.Data.Count > 0)
            {
                entry.AddMessage("---Main Exception Data---");
                try
                {
                    foreach (var pair in exception.Data.Cast<DictionaryEntry>())
                    {
                        entry.AddMessage(pair.Key + ": " + pair.Value);
                    }
                }
                catch { }
            }

            return entry;
        }

        #endregion
    }
}
