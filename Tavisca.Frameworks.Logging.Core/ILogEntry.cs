﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Tavisca.Frameworks.Logging.Infrastructure;

namespace Tavisca.Frameworks.Logging
{
    /// <summary>
    /// Defines the properties of a log entry in the system.
    /// For specific types see <see cref="ITransactionEntry"/> and <see cref="IExceptionEntry"/>
    /// </summary>
    public interface ILogEntry
    {
        /// <summary>
        /// This is typically unique among a logical set of multiple requests, e.g. a set of requests made by a user.
        /// </summary>
        string CorrelationId { get; set; }

        string StackId { get; set; }

        string TransactionId { get; set; }

        /// <summary>
        /// Uniquely identifies an entry
        /// </summary>
        Guid Id { get; set; }

        string TenantId { get; set; }

        string InstanceId { get; set; }

        /// <summary>
        /// Gets or sets the UTC time stamp.
        /// </summary>
        DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the machine name in which the event occurred.
        /// </summary>
        string MachineName { get; set; }

        /// <summary>
        /// Gets or sets the app domain name in which the event occurred.
        /// </summary>
        string AppDomainName { get; set; }

        /// <summary>
        /// Gets or sets the process id in which the application is running.
        /// </summary>
        string ProcessId { get; set; }

        /// <summary>
        /// Gets or sets the process name in which the application is running.
        /// </summary>
        string ProcessName { get; set; }

        /// <summary>
        /// Gets or sets the additional info, consider using <seealso cref="AddAdditionalInfo"/>
        /// </summary>
        IDictionary<string, string> AdditionalInfo { get; }

        /// <summary>
        /// Gets the message added via <seealso cref="AddMessage"/> function.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Gets the severity string converted from the set value of <seealso cref="SeverityType"/>
        /// </summary>
        string Severity { get; }

        /// <summary>
        /// Gets or sets the <see cref="SeverityOptions"/>.
        /// </summary>
        SeverityOptions SeverityType { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        string UserIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the IP address.
        /// </summary>
        string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        string ApplicationName { get; set; }

        /// <summary>
        /// Adds a message into the log entry incrementally.
        /// </summary>
        /// <param name="message">The message to be added in the entry.</param>
        void AddMessage(string message);

        /// <summary>
        /// Adds an additional info
        /// </summary>
        /// <param name="key">The key to be added.</param>
        /// <param name="value">The value against the key.</param>
        void AddAdditionalInfo(string key, string value);

        /// <summary>
        /// Clones the log entry.
        /// </summary>
        ILogEntry CopyTo();
    }
}
