using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Logging.Extensions.Core.Redis
{
    public static class KeyStorage
    {
        /// <summary>
        /// Contains the appsetting key names.
        /// </summary>
        public static class AppSettingKeys
        {
            public const string RedisListIdKey = "Logging.RedislistKey";
            public const string RedisServerConnString = "Logging.RedisServerConnString";

        }
    }
}
