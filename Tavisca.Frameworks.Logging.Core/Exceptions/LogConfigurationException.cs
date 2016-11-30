using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Tavisca.Frameworks.Logging.Exceptions
{
    /// <summary>
    /// The exception thrown by the framework in case of a incorrect configuration for the framework, the actual exception
    /// is usually contained in the inner exception.
    /// This class should not be used outside of the framework apart from catching the same.
    /// </summary>
    [Serializable]
    public class LogConfigurationException: LogException
    {
        #region Constructors

        public LogConfigurationException() : base() { }

        public LogConfigurationException(string message) : base(message) { }

        public LogConfigurationException(string message, Exception innerException) : base(message, innerException) { }

        #endregion
    }
}
