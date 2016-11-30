using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tavisca.Frameworks.Logging.Configuration
{
    /// <summary>
    /// A custom configuration provider for providing logging framework configuration at runtime.
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary>
        /// Gets the configuration for the system.
        /// </summary>
        /// <returns>An instance of an implementation of <see cref="IApplicationLogSettings"/></returns>
        IApplicationLogSettings GetConfiguration();
    }
}
