using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Tavisca.Frameworks.Logging.Resources;

namespace Tavisca.Frameworks.Logging.DependencyInjection
{
    /// <summary>
    /// Factory class meant for maintaining the dependency injection container for the application.
    /// </summary>
    public static class LocatorProvider
    {
        private static IServiceLocator _serviceLocator;

        /// <summary>
        /// Gets the container according to the delegate set in the <seealso cref="SetLocator"/>
        /// </summary>
        /// <returns>Dependency injection container</returns>
        public static IServiceLocator GetContainer()
        {
            if (_serviceLocator == null)
                throw new InvalidOperationException(LogResources.ServiceLocatorProvider_NotSet);

            return _serviceLocator;
        }

        /// <summary>
        /// Sets the container delegate, the delegate is invoked each time <seealso cref="GetContainer"/> is called.
        /// </summary>
        /// <param name="provider">The delegate which returns the container object.</param>
        internal static void SetLocator(ServiceLocatorProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            _serviceLocator = provider.Invoke();

            if (_serviceLocator == null)
                throw new ArgumentException(LogResources.ServiceLocatorProviderDelegate_NullReturn, "provider");
        }
    }
}
