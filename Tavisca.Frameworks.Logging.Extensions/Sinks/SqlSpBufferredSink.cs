using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tavisca.Frameworks.Logging.Exceptions;
using Tavisca.Frameworks.Logging.Extensions.Resources;
using Tavisca.Frameworks.Logging.Extensions.Settings;

namespace Tavisca.Frameworks.Logging.Extensions.Sinks
{
    public sealed class SqlSpBufferredSink : SqlSpSink
    {
        private static readonly ConcurrentBag<IList<SqlParameter>> EventBag;
        private static readonly ConcurrentBag<IList<SqlParameter>> ExceptionBag;

        private static readonly Timer Timer;

        private static readonly string _eventLoggingSP;
        private static readonly string _exceptionLoggingSP;
        private static readonly string _connectionString;

        static SqlSpBufferredSink()
        {
            EventBag = new ConcurrentBag<IList<SqlParameter>>();
            ExceptionBag = new ConcurrentBag<IList<SqlParameter>>();

            Timer = new Timer(Callback, null, 1000, 2000);

            _eventLoggingSP = ConfigurationManager.AppSettings[KeyStorage.AppSettingKeys.EventLoggingSp] ??
                                    "dbo.spWriteLog2";

            _exceptionLoggingSP = ConfigurationManager.AppSettings[KeyStorage.AppSettingKeys.ExceptionLoggingSp] ??
                                    "dbo.spWriteExceptionLog2";

            _connectionString = ConfigurationManager.ConnectionStrings[ConStringName].ConnectionString;
        }

        private static void Callback(object state)
        {
            try
            {
                var eventsToPut = new List<IList<SqlParameter>>(10);

                while (!EventBag.IsEmpty)
                {
                    IList<SqlParameter> parameters;

                    if (EventBag.TryTake(out parameters))
                    {
                        eventsToPut.Add(parameters);
                    }
                }
                eventsToPut.TrimExcess();

                var exceptionsToPut = new List<IList<SqlParameter>>(10);

                while (!ExceptionBag.IsEmpty)
                {
                    IList<SqlParameter> parameters;

                    if (ExceptionBag.TryTake(out parameters))
                    {
                        exceptionsToPut.Add(parameters);
                    }
                }
                exceptionsToPut.TrimExcess();

                PushEntries(eventsToPut, exceptionsToPut);

                if (Timer == null) //should never evaluate to true.
                    throw new LogException(LogExtensionResources.TimerGotNull);
            }
            catch (LogException logException)
            {
                FailSafeLogFactory.Log(logException, true);
            }
            catch (Exception ex)
            {
                var exception = new LogException(LogExtensionResources.Timer_UnknownError, ex);

                FailSafeLogFactory.Log(exception, true);
            }
        }

        private static void PushEntries(ICollection<IList<SqlParameter>> eventParams,
            ICollection<IList<SqlParameter>> exceptionParams)
        {
            if (eventParams.Count == 0 && exceptionParams.Count == 0)
                return;

            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();

                foreach (var eventParamBatch in eventParams)
                {
                    using (var cmd = new SqlCommand(_eventLoggingSP, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (var sqlParameter in eventParamBatch)
                        {
                            cmd.Parameters.Add(sqlParameter);
                        }

                        cmd.ExecuteNonQuery();
                    }
                }

                foreach (var exceptionParamBatch in exceptionParams)
                {
                    using (var cmd = new SqlCommand(_exceptionLoggingSP, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (var sqlParameter in exceptionParamBatch)
                        {
                            cmd.Parameters.Add(sqlParameter);
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        protected override void WriteEvent(IEventEntry eventEntry)
        {
            var parameters = GetEventParams(eventEntry);

            EventBag.Add(parameters);
        }

        protected override void WriteException(IExceptionEntry exceptionEntry)
        {
            var parameters = GetExceptionParams(exceptionEntry);

            ExceptionBag.Add(parameters);
        }
    }
}
