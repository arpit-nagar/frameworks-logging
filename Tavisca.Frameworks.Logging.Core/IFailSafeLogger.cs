using System;

namespace Tavisca.Frameworks.Logging
{
    /// <summary>
    /// Defines the methods of a fail safe logger, a fail safe logger will 
    /// log errors encountered while logging in the system.
    /// </summary>
    internal interface IFailSafeLogger
    {
        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="ex">The exception to be logged.</param>
        void Log(Exception ex);
    }
}