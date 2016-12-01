using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Options;
using Microsoft.Practices.ServiceLocation;
using Tavisca.Frameworks.Logging.Configuration;
using Tavisca.Frameworks.Logging.Extensions.Settings;
using Tavisca.Frameworks.Logging.Extensions.Caching;

namespace Tavisca.Frameworks.Logging.Extensions.DependencyInjection.Adapters
{
    /// <summary>
    /// A reflection based adapter, recommended only for tiny to small applications. Requires
    /// assembly qualified names instead of just keys for ISink configuration in the logging
    /// framework configuration.
    /// </summary>
    public class ReflectionAdapter : ServiceLocatorImplBase
    {
        private IOptions<ApplicationLogSection> Settings;

        public ReflectionAdapter(IOptions<ApplicationLogSection> configuration)
        {
            Settings = configuration;
        }

        #region ServiceLocatorImplBase Members

        protected override object DoGetInstance(Type serviceType, string key)
        {
            var type = GetFromCache(serviceType, key);

            if (type != null)
                return CreateInstance(type, Settings);

            if (!string.IsNullOrWhiteSpace(key))
            {
                type = Type.GetType(key);

                if (type != null)
                {
                    SetToCache(serviceType, key, type);

                    return CreateInstance(type, Settings);
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
            var targetAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => !Exclusions.Any(y => x.FullName.StartsWith(y)));

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
            //<CRITICAL .net core> /HttpRuntime.Cache does not supported in .net core
            CacheHandler.Set(GetKey(type, key), value);
        }

        protected virtual Type GetFromCache(Type type, string key)
        {
            //<CRITICAL .net core> /HttpRuntime.Cache does not supported in .net core
            return CacheHandler.Get<Type>(GetKey(type, key));
        }

        protected virtual string GetKey(Type type, string key)
        {
            const string nullVal = "null";

            var typeKey = type == null ? nullVal : type.AssemblyQualifiedName;

            typeKey = typeKey ?? nullVal;

            var nameKey = key ?? nullVal;

            return string.Format(KeyStorage.CacheKeys.ReflectionAdapterKey, typeKey, nameKey);
        }

        protected virtual object CreateInstance(Type type, params object[] data)
        {
            return Activator.CreateInstance(type, data);
        }

        #endregion
    }

    public class AppDomain
    {
        public static AppDomain CurrentDomain { get; private set; }

        static AppDomain()
        {
            CurrentDomain = new AppDomain();
        }

        public Assembly[] GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in dependencies)
            {
                if (IsCandidateCompilationLibrary(library))
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
            }
            return assemblies.ToArray();
        }

        private static bool IsCandidateCompilationLibrary(RuntimeLibrary compilationLibrary)
        {
            return compilationLibrary.Name == ("Specify")
                || compilationLibrary.Dependencies.Any(d => d.Name.StartsWith("Specify"));
        }
    }
}