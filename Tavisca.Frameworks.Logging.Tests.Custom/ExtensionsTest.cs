using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.Frameworks.Logging.Extensions.DependencyInjection.Adapters;
using Tavisca.Frameworks.Logging.Extensions.Formatters;
using Tavisca.Frameworks.Logging.Infrastructure;
using Tavisca.Frameworks.Logging.Tests.Mock;

namespace Tavisca.Frameworks.Logging.Tests.Custom
{
    [TestClass]
    public class ExtensionsTest
    {
        [TestMethod]
        public void SettingsRefresh()
        {
            RefreshSettings(typeof(ReflectionAdapter));
        }

        [TestMethod]
        public void ReflectionAdapterTest()
        {
            RefreshSettings(typeof(ReflectionAdapter));

            new Tests.ExtensionsTest().EventViewerLoggerExceptionTest();
        }

        [TestMethod]
        public void LogSpecificAdapterTest()
        {
            RefreshSettings(typeof(LogSpecificAdapter));

            new Tests.ExtensionsTest().EventViewerLoggerExceptionTest();
        }

        [TestMethod]
        public void CreditCardMaskFormatterTest()
        {
            RefreshSettings(typeof(ReflectionAdapter), typeof(CreditCardMaskFormatter));

            new Tests.ExtensionsTest().DBLoggerEventTest();
        }

        [TestMethod]
        public void MinPriorityTest()
        {
            RefreshSettings(typeof(DummyLocatorAdapter), typeof(CreditCardMaskFormatter), PriorityOptions.Low);

            new Tests.ExtensionsTest().EventViewerLoggerEventTest();

            RefreshSettings(typeof(DummyLocatorAdapter), typeof(CreditCardMaskFormatter), PriorityOptions.Undefined);

            new Tests.ExtensionsTest().EventViewerLoggerEventTest();

            RefreshSettings(typeof(DummyLocatorAdapter), typeof(CreditCardMaskFormatter), PriorityOptions.Medium);

            new Tests.ExtensionsTest().EventViewerLoggerEventTest();
        }

        private void RefreshSettings(Type adapter, Type formatter = null, PriorityOptions priority = PriorityOptions.Low)
        {
            var factory = new Logger();

            factory.RefreshSettings(new TestCustomConfigProvider().GetCustomConfiguration(adapter, formatter, priority));
        }


    }
}
