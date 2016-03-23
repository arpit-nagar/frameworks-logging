using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.Frameworks.Logging.Tests.Mock;

namespace Tavisca.Frameworks.Logging.Tests.Custom
{
    [TestClass]
    public class CustomProviderTest
    {
        private volatile bool _hasReturned;

        [TestMethod]
        public void TestCustomConfigurationTest()
        {
            var factory = new Logger();

            factory.WriteExceptionAsync(new Exception("Root Level Test"), KeyStore.Categories.Default, Callback);

            var count = 0;
            while (!_hasReturned)
            {
                System.Threading.Thread.Sleep(500);

                count++;

                if (count > 500)
                    break;
            }
        }

        private void Callback(Task task)
        {
            _hasReturned = true;
        }
    }
}
