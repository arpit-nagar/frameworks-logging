using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tavisca.Frameworks.Logging.Tests
{
    public static class KeyStore
    {
        public static class Categories
        {
            public const string Default = "Default";
            public const string FileLogger = "FileLogger";
            public const string EventViewer = "EventViewerLogger";
            public const string DB = "DBLogger";
            public const string RedisLogger = "RedisLogger";
        }

        public static class Loggers
        {
            public const string DBLogger = "DBLogger";
            public const string RedisSink = "RedisSink";
            public const string EventViewerLoggerMock = "EventViewerLoggerMock";
            public const string FileLogger = "FileLogger";
            public const string EventViewerLogger = "EventViewerLogger";
        }
    }
}
