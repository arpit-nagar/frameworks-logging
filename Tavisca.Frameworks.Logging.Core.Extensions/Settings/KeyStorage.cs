using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Logging.Extensions.Settings
{
    /// <summary>
    /// Central container for all keys in the project.
    /// </summary>
    public static class KeyStorage
    {
        /// <summary>
        /// Contains the appsetting key names.
        /// </summary>
        public static class AppSettingKeys
        {
            public const string FileLoggerFilePath = "Tavisca.Frameworks.Logging.Extensions.Loggers.FileLogger.FilePath";
            public const string MaxFileSize = "Tavisca.Frameworks.Logging.Extensions.Loggers.FileLogger.MaxFileSize";
            public const string EventVwrSource = "Tavisca.Frameworks.Logging.Extensions.Loggers.EventViewerLogger.Source";
            public const string CreditCardMaskProvider = "Tavisca.Frameworks.Logging.Extensions.Formatters.ICreditCardMaskDataProvider";
            public const string EventLoggingSp = "Tavisca.Frameworks.Logging.Extensions.Loggers.EventLoggingSP";
            public const string ExceptionLoggingSp = "Tavisca.Frameworks.Logging.Extensions.Loggers.ExceptionLoggingSP";

            public const string RedisListIdKey = "Logging.RedislistKey";
            public const string RedisServerConnString = "Logging.RedisServerConnString";

        }

        /// <summary>
        /// Contains the cache key names.
        /// </summary>
        public static class CacheKeys
        {
            public const string CreditCardMaskSettingsKey = "CreditCardMaskSettingsKey";
            public const string ReflectionAdapterKey = "ReflectionAdapter-{0}-{1}";
        }
    }
}
