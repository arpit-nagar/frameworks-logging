using System.Collections.Generic;

namespace Tavisca.Frameworks.Logging.Extensions.Caching
{
    public class CacheHandler
    {
        private static readonly Dictionary<string, object> LocalCache;

        static CacheHandler()
        {
            if (LocalCache == null)
                LocalCache = new Dictionary<string, object>();
        }

        public static T Get<T>(string key)
        {

            if (LocalCache.ContainsKey(key))
                return (T)LocalCache[key];
            return default(T);
        }

        public static void Set(string key, object value)
        {
            if (!LocalCache.ContainsKey(key))
                LocalCache.Add(key, value);
        }
    }
}
