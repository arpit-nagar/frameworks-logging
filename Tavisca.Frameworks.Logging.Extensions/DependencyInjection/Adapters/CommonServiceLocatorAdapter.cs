using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;

namespace Tavisca.Frameworks.Logging.Extensions.DependencyInjection.Adapters
{
    /// <summary>
    /// An implementation of "Microsoft.Practices.ServiceLocation", uses the "Current" static
    /// property of the ServiceLocator object to instantiate the objects.
    /// </summary>
    public class CommonServiceLocatorAdapter : ServiceLocatorImplBase
    {
        #region ServiceLocatorImplBase Members

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return ServiceLocator.Current.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return ServiceLocator.Current.GetAllInstances(serviceType);
        }

        #endregion
    }
}
