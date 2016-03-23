using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Caching;
using Microsoft.Practices.ServiceLocation;
using Tavisca.Frameworks.Logging.Extensions.Settings;
using Tavisca.Singularity;
using CacheItemPriority = System.Web.Caching.CacheItemPriority;

namespace Tavisca.Frameworks.Logging.Extensions.DependencyInjection.Adapters
{
    /// <summary>
    /// A reflection based adapter, recommended only for tiny to small applications. Requires
    /// assembly qualified names instead of just keys for ISink configuration in the logging
    /// framework configuration.
    /// </summary>
    public class ReflectionAdapter : ServiceLocatorImplBase
    {
        #region ServiceLocatorImplBase Members

        protected override object DoGetInstance(Type serviceType, string key)
        {
            var type = GetFromCache(serviceType, key);

            if (type != null)
                return CreateInstance(type);

            if (!string.IsNullOrWhiteSpace(key))
            {
                type = Type.GetType(key);

                if (type != null)
                {
                    SetToCache(serviceType, key, type);

                    return CreateInstance(type);
                }
            }

            if (serviceType.AssemblyQualifiedName == null)
                return null;

            if (serviceType.AssemblyQualifiedName.Equals(typeof(ISink).AssemblyQualifiedName))
                throw new NotSupportedException(Resources.LogExtensionResources.ReflectionAdapter_TypeBased_NotSupported);

            var retVal = GetMatchingTypes(serviceType).FirstOrDefault();

            if (retVal != null)
                SetToCache(serviceType, key, retVal.GetType());

            return retVal;
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return GetMatchingTypes(serviceType);
        }

        #endregion

        #region Protected Methods

        protected static string[] Exclusions = new[] { "Microsoft", "System", "mscorlib" };

        protected virtual IEnumerable<object> GetMatchingTypes(Type baseType)
        {
            var targetAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !Exclusions.Any(y => x.FullName.StartsWith(y)));

            var targets = targetAssemblies.SelectMany(x => x.GetTypes());

            foreach (var type in targets)
            {
                if (baseType.IsAssignableFrom(type))
                {
                    var parameterlessConstructor = type.GetConstructor(Type.EmptyTypes);

                    if (parameterlessConstructor != null)
                        yield return CreateInstance(type);
                }
            }
        }

        protected virtual void SetToCache(Type type, string key, Type value)
        {
            HttpRuntime.Cache.Add(GetKey(type, key), value, null, Cache.NoAbsoluteExpiration,
                                  new TimeSpan(0, 1, 0, 0), CacheItemPriority.Default, null);
        }

        protected virtual Type GetFromCache(Type type, string key)
        {
            return HttpRuntime.Cache.Get(GetKey(type, key)) as Type;
        }

        protected virtual string GetKey(Type type, string key)
        {
            const string nullVal = "null";

            var typeKey = type == null ? nullVal : type.AssemblyQualifiedName;

            typeKey = typeKey ?? nullVal;

            var nameKey = key ?? nullVal;

            return string.Format(KeyStorage.CacheKeys.ReflectionAdapterKey, typeKey, nameKey);
        }

        protected virtual object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        #endregion
    }
}