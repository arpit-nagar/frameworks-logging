using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using Tavisca.Frameworks.Helper;
using Tavisca.Frameworks.Helper.Net;
using Tavisca.Frameworks.Logging.Exceptions;
using Tavisca.Frameworks.Logging.Extensions.Settings;

namespace Tavisca.Frameworks.Logging.Extensions.Sinks
{
    public sealed class RedisSink : SinkBase
    {
        #region Static Members

        private static string _redisHost;
        private static int _redisPort = 6379;
        private static string _listId;

        static RedisSink()
        {
            ResolveConfigurations();
        }

        private static void ResolveConfigurations()
        {
            var connString = ConfigurationManager.AppSettings[KeyStorage.AppSettingKeys.RedisServerConnString];

            if (string.IsNullOrEmpty(connString))
            {
                throw new LogException("Redis connection string is not in correct format :" + connString);
            }

            var portedConn = connString.Split(':');

            var port = 6379;
            if (portedConn.Length > 1 && !int.TryParse(portedConn[1], out port))
            {
                throw new LogException("Redis connection string is not in correct format :" + connString);
            }

            _redisHost = portedConn[0];
            _redisPort = port;

            _listId = ConfigurationManager.AppSettings[KeyStorage.AppSettingKeys.RedisListIdKey];
        }

        #endregion

        #region Members

        private static void PushData(string data)
        {
            if (_listId == null)
                throw new LogException(string.Format("The key {0} must be defined in the settings. This is the name of the queue where another component (e.g. redis-logstash) will pick up things for further processing.", KeyStorage.AppSettingKeys.RedisListIdKey));

            var retry = 0;

            for (var i = 0; i < 2; i++)
            {
                using (var client = Tavisca.Frameworks.Helper.Redis.RedisClientManager.GetClient(_redisHost, _redisPort))
                {
                    {
                        try
                        {
                            client.AddItemToList(_listId, FormatData(data));
                        }
                        catch (Exception)
                        {
                            if (retry == 0) //two tries, as the DNS resolution to get ips might have expired.
                            {
                                retry++;
                                continue;
                            }

                            throw;
                        }
                        break;
                    }
                }
            }

        }

        private static string FormatData(string data)
        {
            var compressed = ByteArrayProcesser.Compress(Encoding.UTF8.GetBytes(data));

            var retVal = Convert.ToBase64String(compressed);

            return retVal;
        }

        #endregion

        protected override void WriteEvent(IEventEntry eventEntry)
        {
            if (eventEntry != null)
            {
                if (eventEntry.Id == Guid.Empty)
                    eventEntry.Id = Guid.NewGuid();
                var str = Newtonsoft.Json.JsonConvert.SerializeObject(eventEntry);
                PushData(str);
            }
        }

        protected override void WriteException(IExceptionEntry eventEntry)
        {
            if (eventEntry != null)
            {
                if (eventEntry.Id == Guid.Empty)
                    eventEntry.Id = Guid.NewGuid();

                var str = Newtonsoft.Json.JsonConvert.SerializeObject(eventEntry);
                PushData(str);
            }
        }
    }
}
