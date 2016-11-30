using System;
using System.Collections.Concurrent;
using System.Globalization;
using ServiceStack.Redis;
using Tavisca.Frameworks.Logging.Configuration;

namespace Tavisca.Frameworks.Logging.Extensions.Redis
{
    public static class RedisClientManager
    {

        private static ConcurrentDictionary<string, RedisManagerPool> _redisManagerPoolStore =
            new ConcurrentDictionary<string, RedisManagerPool>(StringComparer.OrdinalIgnoreCase);

        static int _maxPoolSize = 5;

        static RedisClientManager()
        {
            int maxPoolSize;
            string mpz = ApplicationLogSetting.GetCustomConfiguration("RedisMaxPoolSize");
            
            _maxPoolSize = int.TryParse(mpz, out maxPoolSize) ? maxPoolSize : 5;
        }

        public static IRedisClient GetClient(string hostName, int port = 6379)
        {

            return _redisManagerPoolStore.GetOrAdd(string.Join(":", hostName, port.ToString(CultureInfo.InvariantCulture)),
                                                   f => new RedisManagerPool(new String[] { f }, new RedisPoolConfig() { MaxPoolSize = _maxPoolSize })).GetClient();

        }




    }
}
