using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Tavisca.Frameworks.Logging.Configuration;
using Tavisca.Frameworks.Logging.DependencyInjection;
using Tavisca.Frameworks.Logging.Exceptions;
using Tavisca.Frameworks.Logging.Formatting;
using Tavisca.Frameworks.Logging.Infrastructure;
using Tavisca.Frameworks.Logging.Resources;

namespace Tavisca.Frameworks.Logging
{
    public class Logger : ILogger
    {
        #region Static Members

        protected static IApplicationLogSettings LogSection;
        protected static IDictionary<string, LoggerElementCollection> Categories;
        protected static TaskFactory TaskFactory;

        /// <summary>
        /// Initializes configurations of the logging framework.
        /// </summary>
        static Logger()
        {
            EnsureConfigurationLoad();
        }

        protected internal static void EnsureConfigurationLoad()
        {
            if (Flags.IsConfigurationLoaded)
                return;

            try
            {
                var settings = GetConfiguration();

                LoadConfiguration(settings);

                Flags.IsConfigurationLoaded = true;
            }
            catch (LogConfigurationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new LogConfigurationException(LogResources.LogConfigurationError_Generic, ex);
            }
        }

        protected static void LoadConfiguration(IApplicationLogSettings settings)
        {
            try
            {
                if (settings == null)
                    throw new LogConfigurationException(LogResources.MissingConfiguration);

                #region Custom Locator

                var providerType = settings.CustomLocatorAdapter;

                if (string.IsNullOrWhiteSpace(providerType))
                    throw new LogConfigurationException(LogResources.Configuration_MissingCustomLocatorProvider);

                var provider = GetServiceLocatorDelegate(providerType);

                LocatorProvider.SetLocator(provider);

                #endregion

                var taskFactory = TaskScheduling.Utility.GetTaskFactory(settings);

                var categories = new Dictionary<string, LoggerElementCollection>();

                foreach (CategoryElement category in settings.Categories)
                {
                    categories[category.Name] = category.Loggers;
                }

                if (!string.IsNullOrWhiteSpace(settings.CustomFormatter))
                {
                    var type = Type.GetType(settings.CustomFormatter);

                    if (type == null)
                        throw new LogConfigurationException(LogResources.Configuration_CustomFormatter_NotFound);

                    var formatter = Activator.CreateInstance(type) as ILogEntryFormatter;

                    if (formatter == null)
                        throw new LogConfigurationException(LogResources.Configuration_CustomFormatter_Invalid);

                    FormattingFactory.SetFormatter(formatter);
                }

                //Set Compression Provider
                Compression.ICompressionProvider compressionProvider;
                switch (settings.CompressionType)
                {
                    case CompressionTypeOptions.Zip:
                        compressionProvider = new Compression.GZipCompressionProvider();
                        break;
                    case CompressionTypeOptions.Deflate:
                        compressionProvider = new Compression.DeflateCompressionProvider();
                        break;
                    case CompressionTypeOptions.Custom:

                        if (string.IsNullOrWhiteSpace(settings.CustomCompressionType))
                            throw new LogConfigurationException(LogResources.Configuration_CustomCompressionType_NotFound);

                        var type = Type.GetType(settings.CustomCompressionType);

                        if (type == null)
                            throw new LogConfigurationException(LogResources.Configuration_CustomCompressionType_NotFound);

                        compressionProvider = Activator.CreateInstance(type) as Compression.ICompressionProvider;

                        if (compressionProvider == null)
                            throw new LogConfigurationException(LogResources.Configuration_CustomCompressionType_Invalid);
                        break;
                    default:
                        throw new LogConfigurationException(
                            string.Format(LogResources.CompressionType_NotSupported,
                            Enum.GetName(typeof(CompressionTypeOptions), settings.CompressionType)));
                }

                Compression.CompressionProviderFactory.CurrentProvider = compressionProvider;

                LogSection = settings;
                Categories = categories;
                TaskFactory = taskFactory;

                FailSafeLogFactory.SetFailOverLoggingBehaviour(settings.ReThrowLogExceptions == SwitchOptions.On);
            }
            catch (LogConfigurationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new LogConfigurationException(LogResources.LogConfigurationError_Generic, ex);
            }
        }

        /// <summary>
        /// Gets the configuration via config as well as a custom provider (<see cref="IConfigurationProvider"/>) 
        /// if so configured.
        /// </summary>
        /// <returns>The final static configuration for the framework either from the config or via a custom provider.</returns>
        protected static IApplicationLogSettings GetConfiguration()
        {
            var section = (IApplicationLogSettings)ConfigurationManager.GetSection("ApplicationLog");

            if (!string.IsNullOrEmpty(section.LogConfigurationProvider))
            {
                var type = Type.GetType(section.LogConfigurationProvider);

                if (type == null)
                    throw new LogConfigurationException(string.Format(LogResources.LogConfigurationProvider_NotFound, section.LogConfigurationProvider));

                IConfigurationProvider provider;
                try
                {
                    provider = (IConfigurationProvider)Activator.CreateInstance(type);
                }
                catch (Exception ex)
                {
                    throw new LogConfigurationException(string.Format(LogResources.LogConfigurationProvider_InstanceCreation, section.LogConfigurationProvider), ex);
                }

                section = provider.GetConfiguration();

                if (section == null)
                    throw new LogConfigurationException(LogResources.LogConfigurationProvider_MissingConfiguration);
            }

            return section;
        }

        /// <summary>
        /// Creates a service locator (<see cref="IServiceLocator"/>) delegate based 
        /// upon the assembly qualified name provided.
        /// </summary>
        /// <param name="provider">The assembly qualified name of the provider to be created.</param>
        /// <returns>A delegate which returns the provider, the delegate usage is for future flexibility for control of creation.</returns>
        protected static ServiceLocatorProvider GetServiceLocatorDelegate(string provider)
        {
            var type = Type.GetType(provider);

            if (type == null)
                throw new LogConfigurationException(string.Format(LogResources.Configuration_CouldNotResolveCustomLocator, provider));

            var locatorObj = Activator.CreateInstance(type) as IServiceLocator;

            if (locatorObj == null)
                throw new LogConfigurationException(LogResources.Configuration_IncorrectCustomLocatorProvider);

            return () => locatorObj;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// default constructor
        /// </summary>
        public Logger() { }

        /// <summary>
        /// Constructor with the option of setting override filters.
        /// </summary>
        /// <param name="overriddeEntryFilters">See <seealso cref="ILogger.OverriddeEntryFilters"/></param>
        public Logger(bool overriddeEntryFilters)
        {
            OverriddeEntryFilters = overriddeEntryFilters;
        }

        #endregion

        #region ILogFactory Members

        public bool OverriddeEntryFilters { get; set; }

        public void WriteAsync(ILogEntry entry, string category)
        {
            try
            {
                if (entry == null)
                    throw new ArgumentNullException("entry");

                if (!CheckSectionStatus(entry))
                    return;

                var clone = entry.CopyTo();

                TaskFactory.StartNew(() => this.Write(clone, category));
            }
            catch (LogException)
            {
                throw;
            }
            catch (Exception ex)
            {
                FailSafeLogFactory.Log(ex);
            }
        }

        public void WriteAsync(ILogEntry entry, string category, Action<Task> continueWith)
        {
            try
            {
                if (entry == null)
                    throw new ArgumentNullException("entry");

                if (continueWith == null)
                    throw new ArgumentNullException("continueWith");

                if (!CheckSectionStatus(entry))
                {
                    continueWith(null);
                    return;
                }

                var clone = entry.CopyTo();

                TaskFactory.StartNew(() => this.Write(clone, category))
                    .ContinueWith(continueWith);
            }
            catch (LogException)
            {
                throw;
            }
            catch (Exception ex)
            {
                FailSafeLogFactory.Log(ex);
            }
        }

        public void WriteExceptionAsync(Exception exception, string category)
        {
            try
            {
                if (exception == null)
                    throw new ArgumentNullException("exception");

                var entry = exception.ToEntry();

                if (!CheckSectionStatus(entry))
                    return;

                TaskFactory.StartNew(() => this.Write(entry, category));
            }
            catch (LogException)
            {
                throw;
            }
            catch (Exception ex)
            {
                FailSafeLogFactory.Log(ex);
            }
        }

        public void WriteExceptionAsync(Exception exception, string category, Action<Task> continueWith)
        {
            try
            {
                if (exception == null)
                    throw new ArgumentNullException("exception");

                if (continueWith == null)
                    throw new ArgumentNullException("continueWith");

                var entry = exception.ToEntry();

                if (!CheckSectionStatus(entry))
                {
                    continueWith(null);
                    return;
                }

                TaskFactory.StartNew(() => this.Write(entry, category))
                    .ContinueWith(continueWith);
            }
            catch (LogException)
            {
                throw;
            }
            catch (Exception ex)
            {
                FailSafeLogFactory.Log(ex);
            }
        }

        public virtual void WriteException(Exception exception, string category)
        {
            try
            {
                var entry = exception.ToEntry();

                if (!CheckSectionStatus(entry))
                    return;

                entry = FormattingFactory.CurrentFormatter.FormatException(entry);

                var loggers = GetLoggers(category);

                foreach (var logger in loggers)
                {
                    logger.Write(entry);
                }
            }
            catch (LogException)
            {
                throw;
            }
            catch (Exception ex)
            {
                FailSafeLogFactory.Log(ex);
            }
        }

        public virtual void Write(ILogEntry entry, string category)
        {
            try
            {
                if (!CheckSectionStatus(entry))
                    return;

                var loggers = GetLoggers(category);

                var eventEntry = entry as ITransactionEntry;
                if (eventEntry != null)
                {
                    entry = FormattingFactory.CurrentFormatter.FormatEvent(eventEntry);
                }
                else
                {
                    entry = FormattingFactory.CurrentFormatter.FormatException((IExceptionEntry)entry);
                }

                foreach (var logger in loggers)
                {
                    logger.Write(entry);
                }
            }
            catch (LogException)
            {
                throw;
            }
            catch (Exception ex)
            {
                FailSafeLogFactory.Log(ex);
            }
        }

        public void RefreshSettings(IApplicationLogSettings settings)
        {
            LoadConfiguration(settings);
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// Gets the loggers for the given category as per the current configuration.
        /// </summary>
        /// <param name="category">The category key.</param>
        /// <returns>An enumerable of <see cref="ISink"/></returns>
        protected virtual IEnumerable<ISink> GetLoggers(string category)
        {
            LoggerElementCollection loggers = null;

            var hasLoggers = category != null;

            if (hasLoggers)
                hasLoggers = Categories.TryGetValue(category, out loggers);

            if (!hasLoggers)
            {
                var defaultLogger = GetDefaultLogger(category);

                if (defaultLogger != null)
                    yield return defaultLogger;

                yield break;
            }

            foreach (LoggerElement loggerElement in loggers)
            {
                yield return GetLoggerByName(loggerElement.Name);
            }
        }

        /// <summary>
        /// Gets the default logger configured in the system.
        /// </summary>
        /// <returns><see cref="ISink"/></returns>
        protected virtual ISink GetDefaultLogger(string category = null)
        {
            return string.IsNullOrWhiteSpace(LogSection.DefaultLogger) ? null :
                GetLoggerByName(LogSection.DefaultLogger);
        }

        /// <summary>
        /// Gets an <see cref="ISink"/> by name using the DI container configured in the system.
        /// </summary>
        /// <param name="name">The name (key) of the logger.</param>
        /// <returns>An instance of an <see cref="ISink"/></returns>
        protected virtual ISink GetLoggerByName(string name)
        {
            return LocatorProvider.GetContainer().GetInstance<ISink>(name);
        }

        /// <summary>
        /// Checks wether the given log entry type is configured to be switched "on" or otherwise.
        /// </summary>
        /// <param name="entry">The entry to be logged.</param>
        /// <returns>A boolean value indicating wether the entry should be logged.</returns>
        protected virtual bool CheckSectionStatus(ILogEntry entry)
        {
            if (OverriddeEntryFilters)
                return true;

            if (entry is ITransactionEntry)
            {
                if (LogSection.EventSwitch == SwitchOptions.Off)
                    return false;
            }
            else if (entry is IExceptionEntry)
            {
                if (LogSection.ExceptionSwitch == SwitchOptions.Off)
                    return false;
            }
            return true;
        }

        #endregion
    }
}
