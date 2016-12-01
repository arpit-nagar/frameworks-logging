﻿using System;
using System.Collections.Generic;
using Tavisca.Frameworks.Logging.Configuration;
using Tavisca.Frameworks.Logging.Extensions.DependencyInjection.Adapters;
using Tavisca.Frameworks.Logging.Extensions.Sinks;
using Tavisca.Frameworks.Logging.Infrastructure;
using Tavisca.Frameworks.Logging.Tests.Mock;

namespace Tavisca.Frameworks.Logging.Tests.Custom
{
    public class TestCustomConfigProvider : IConfigurationProvider
    {
        public IApplicationLogSettings GetConfiguration()
        {
            return GetCustomConfiguration();
        }

        public IApplicationLogSettings GetCustomConfiguration(Type adapter = null, Type formatter = null, PriorityOptions priority = PriorityOptions.Low)
        {
            adapter = adapter ?? typeof(DummyLocatorAdapter);

            return new ApplicationLogSection()
            {
                Categories = GetCategoryElementCollection(adapter == typeof(ReflectionAdapter)),
                CustomLocatorAdapter = adapter.AssemblyQualifiedName,
                CustomFormatter = formatter == null ? typeof(DummyFormatter).AssemblyQualifiedName : formatter.AssemblyQualifiedName,
                EventSwitch = SwitchOptions.On,
                ExceptionSwitch = SwitchOptions.On,
                MinPriority = priority,
                MaxThreads = 5,
                ReThrowLogExceptions = SwitchOptions.On,
                TraceLoggers = new List<LoggerElement>
                    {
                        new LoggerElement()
                            {
                                Name = KeyStore.Loggers.FileLogger
                            }
                    }
            };
        }

        private List<CategoryElement> GetCategoryElementCollection(bool reflectionMode = false)
        {
            var col = new List<CategoryElement>() {
                            new CategoryElement()
                            {
                                Name = KeyStore.Categories.Default,
                                Loggers = new List<LoggerElement>()
                                    {
                                        new LoggerElement()
                                            {
                                                Name = reflectionMode ? typeof(EventViewerLogger).AssemblyQualifiedName : KeyStore.Loggers.EventViewerLoggerMock
                                            }
                                    }
                            },
                            new CategoryElement()
                            {
                                Name = KeyStore.Categories.EventViewer,
                                Loggers = new List<LoggerElement>
                                    {
                                        new LoggerElement()
                                            {
                                                Name = reflectionMode ? typeof(EventViewerSink).AssemblyQualifiedName : KeyStore.Loggers.EventViewerLogger
                                            }
                                    }
                            }
                        };

            return col;
        }
    }
}
