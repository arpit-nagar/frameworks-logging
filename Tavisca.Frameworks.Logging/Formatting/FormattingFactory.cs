using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tavisca.Frameworks.Logging.Formatting
{
    /// <summary>
    /// The factory responsible for maintaining the formatter instance and selection.
    /// </summary>
    public class FormattingFactory
    {
        private static ILogEntryFormatter _logEntryFormatter;

        /// <summary>
        /// The current formatter set in the system. If nothing is set, this returns the 
        /// <see cref="DefaultFormatter"/> class instance.
        /// </summary>
        public static ILogEntryFormatter CurrentFormatter
        {
            get { return _logEntryFormatter ?? (_logEntryFormatter = new DefaultFormatter()); }
        }

        /// <summary>
        /// Sets the formatter which can then be retrieved from the <seealso cref="CurrentFormatter"/> property.
        /// </summary>
        /// <param name="logEntryFormatter">The formatter instance to be set.</param>
        public static void SetFormatter(ILogEntryFormatter logEntryFormatter)
        {
            if (logEntryFormatter == null)
                throw new ArgumentNullException("logEntryFormatter");

            _logEntryFormatter = logEntryFormatter;
        }
    }
}
