using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using Tavisca.Singularity;

namespace Tavisca.Frameworks.Logging.Extensions.DependencyInjection.Adapters
{
    /// <summary>
    /// A Singularity based adapter, uses the Tavisca singularity framework to resolve types.
    /// </summary>
    public class SingularityAdapter : ServiceLocatorImplBase
    {
        #region ServiceLocatorImplBase Members

        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return RuntimeContext.Resolver.Resolve(serviceType);

            return RuntimeContext.Resolver.Resolve(serviceType, key);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return RuntimeContext.Resolver.ResolveAll(serviceType);
        }

        #endregion
    }
}
