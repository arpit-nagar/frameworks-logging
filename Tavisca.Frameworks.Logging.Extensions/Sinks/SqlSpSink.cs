using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavisca.Frameworks.Logging.Extensions.Data;
using Tavisca.Frameworks.Logging.Extensions.Settings;

namespace Tavisca.Frameworks.Logging.Extensions.Sinks
{
    /// <summary>
    /// A database sink implementation, pushes the <see cref="ILogEntry"/> objects into the database 
    /// in a specific schema. The class can be inherited, override the <seealso cref="GetConnectionString"/>
    /// method to specify a different connection string. Default connection string name: "log".
    /// </summary>
    public class SqlSpSink : SinkBase
    {
        #region SinkBase Members

        protected override void WriteEvent(ITransactionEntry transactionEntry)
        {
            using (var con = new SqlConnection(GetConnectionString()))
            {
                using (var cmd = new SqlCommand(EventLoggingSp, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var parameters = GetEventParams(transactionEntry);

                    foreach (var sqlParameter in parameters)
                    {
                        cmd.Parameters.Add(sqlParameter);
                    }

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected override void WriteException(IExceptionEntry exceptionEntry)
        {
            using (var con = new SqlConnection(GetConnectionString()))
            {
                using (var cmd = new SqlCommand(ExceptionLoggingSp, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var parameters = GetExceptionParams(exceptionEntry);

                    foreach (var sqlParameter in parameters)
                    {
                        cmd.Parameters.Add(sqlParameter);
                    }

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Helper Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        protected IList<SqlParameter> GetEventParams(ITransactionEntry transactionEntry)
        {
            var parameters = new List<SqlParameter>(27);

            parameters.Add(new SqlParameter("severity", DbType.String)
            {
                Value = transactionEntry.Severity
            });

            parameters.Add(new SqlParameter("correlationId", DbType.String)
            {
                Value = transactionEntry.CorrelationId
            });

            parameters.Add(new SqlParameter("transactionId", DbType.String)
            {
                Value = transactionEntry.TransactionId
            });

            parameters.Add(new SqlParameter("stackId", DbType.String)
            {
                Value = transactionEntry.StackId
            });

            parameters.Add(new SqlParameter("tenantId", DbType.String)
            {
                Value = transactionEntry.TenantId
            });

            parameters.Add(new SqlParameter("instanceId", DbType.String)
            {
                Value = transactionEntry.InstanceId
            });

            parameters.Add(new SqlParameter("machineName", DbType.String)
            {
                Value = FormatParam(transactionEntry.MachineName)
            });

            parameters.Add(new SqlParameter("AppDomainName", DbType.String)
            {
                Value = FormatParam(transactionEntry.AppDomainName)
            });

            parameters.Add(new SqlParameter("ProcessID", DbType.String)
            {
                Value = FormatParam(transactionEntry.ProcessId)
            });

            parameters.Add(new SqlParameter("ProcessName", DbType.String)
            {
                Value = FormatParam(transactionEntry.ProcessName)
            });

            parameters.Add(new SqlParameter("message", DbType.String)
            {
                Value = FormatParam(transactionEntry.Message)
            });

            parameters.Add(new SqlParameter("xmlrequest", DbType.String)
            {
                Value = FormatParam(transactionEntry.Request)
            });

            parameters.Add(new SqlParameter("xmlresponse", DbType.String)
            {
                Value = FormatParam(transactionEntry.Response)
            });

            parameters.Add(new SqlParameter("additionalInfo", SqlDbType.Structured)
            {
                Value = GetAdditionalInfo(transactionEntry.AdditionalInfo)
            });

            parameters.Add(new SqlParameter("serviceUrl", DbType.String)
            {
                Value = FormatParam(transactionEntry.ServiceUrl)
            });

            parameters.Add(new SqlParameter("methodName", DbType.String)
            {
                Value = FormatParam(transactionEntry.MethodName)
            });

            parameters.Add(new SqlParameter("status", DbType.String)
            {
                Value = FormatParam(transactionEntry.Status)
            });

            parameters.Add(new SqlParameter("timetaken", DbType.String)
            {
                Value = transactionEntry.TimeTaken
            });

            parameters.Add(new SqlParameter("useridentifier", DbType.String)
            {
                Value = FormatParam(transactionEntry.UserIdentifier)
            });

            parameters.Add(new SqlParameter("application", DbType.String)
            {
                Value = FormatParam(transactionEntry.ApplicationName)
            });

            parameters.Add(new SqlParameter("ipAddress", DbType.String)
            {
                Value = FormatParam(transactionEntry.IpAddress)
            });

            parameters.Add(new SqlParameter("accountid", DbType.String)
            {
                Value = FormatParam(transactionEntry.UserIdentifier)
            });

            parameters.Add(new SqlParameter("timestamp", DbType.DateTime)
            {
                Value = transactionEntry.Timestamp
            });

            // out param
            parameters.Add(new SqlParameter("LogId", DbType.Int32)
            {
                Direction = ParameterDirection.Output
            });

            return parameters;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        protected IList<SqlParameter> GetExceptionParams(IExceptionEntry exceptionEntry)
        {
            var parameters = new List<SqlParameter>(27);

            parameters.Add(new SqlParameter("severity", DbType.String)
            {
                Value = FormatParam(exceptionEntry.Severity)
            });

            parameters.Add(new SqlParameter("correlationId", DbType.String)
            {
                Value = FormatParam(exceptionEntry.CorrelationId)
            });

            parameters.Add(new SqlParameter("transactionId", DbType.String)
            {
                Value = FormatParam(exceptionEntry.TransactionId)
            });

            parameters.Add(new SqlParameter("stackId", DbType.String)
            {
                Value = FormatParam(exceptionEntry.StackId)
            });

            parameters.Add(new SqlParameter("tenantId", DbType.String)
            {
                Value = FormatParam(exceptionEntry.TenantId)
            });

            parameters.Add(new SqlParameter("instanceId", DbType.String)
            {
                Value = FormatParam(exceptionEntry.InstanceId)
            });

            parameters.Add(new SqlParameter("machineName", DbType.String)
            {
                Value = FormatParam(exceptionEntry.MachineName)
            });

            parameters.Add(new SqlParameter("AppDomainName", DbType.String)
            {
                Value = FormatParam(exceptionEntry.AppDomainName)
            });

            parameters.Add(new SqlParameter("ProcessID", DbType.String)
            {
                Value = FormatParam(exceptionEntry.ProcessId)
            });

            parameters.Add(new SqlParameter("ProcessName", DbType.String)
            {
                Value = FormatParam(exceptionEntry.ProcessName)
            });

            parameters.Add(new SqlParameter("Type", DbType.String)
            {
                Value = FormatParam(exceptionEntry.Type)
            });

            parameters.Add(new SqlParameter("message", DbType.String)
            {
                Value = FormatParam(exceptionEntry.Message)
            });

            parameters.Add(new SqlParameter("Source", DbType.String)
            {
                Value = FormatParam(exceptionEntry.Source)
            });

            parameters.Add(new SqlParameter("TargetSite", DbType.String)
            {
                Value = FormatParam(exceptionEntry.TargetSite)
            });

            parameters.Add(new SqlParameter("StackTrace", DbType.String)
            {
                Value = FormatParam(exceptionEntry.StackTrace)
            });

            parameters.Add(new SqlParameter("AdditionalInfo", SqlDbType.Structured)
            {
                Value = GetAdditionalInfo(exceptionEntry.AdditionalInfo)
            });

            parameters.Add(new SqlParameter("InnerExceptions", DbType.String)
            {
                Value = FormatParam(exceptionEntry.InnerExceptions)
            });

            parameters.Add(new SqlParameter("useridentifier", DbType.String)
            {
                Value = FormatParam(exceptionEntry.UserIdentifier)
            });

            parameters.Add(new SqlParameter("application", DbType.String)
            {
                Value = FormatParam(exceptionEntry.ApplicationName)
            });

            parameters.Add(new SqlParameter("ipAddress", DbType.String)
            {
                Value = FormatParam(exceptionEntry.IpAddress)
            });

            parameters.Add(new SqlParameter("accountid", DbType.String)
            {
                Value = FormatParam(exceptionEntry.UserIdentifier)
            });

            parameters.Add(new SqlParameter("timestamp", DbType.DateTime)
            {
                Value = exceptionEntry.Timestamp
            });

            // out param
            parameters.Add(new SqlParameter("LogId", DbType.Int32)
            {
                Direction = ParameterDirection.Output
            });

            return parameters;
        }

        protected object FormatParam<T>(T input)
            where T : class
        {
            if (input == null)
                return DBNull.Value;

            return input;
        }

        protected virtual DataTable GetAdditionalInfo(IEnumerable<KeyValuePair<string, string>> additionalInfo)
        {
            var table = new DataTable("AdditionalInfo");

            table.Columns.Add("Key", typeof(string));
            table.Columns.Add("Value", typeof(string));

            foreach (var pair in additionalInfo)
            {
                table.Rows.Add(pair.Key, pair.Value);
            }

            return table;
        }

        private static string _eventLoggingSP;
        protected virtual string EventLoggingSp
        {
            get
            {
                return _eventLoggingSP ??
                    (_eventLoggingSP = (ConfigurationManager.AppSettings[KeyStorage.AppSettingKeys.EventLoggingSp] ?? "dbo.spWriteLog2"));
            }
        }

        private static string _exceptionLoggingSP;
        protected virtual string ExceptionLoggingSp
        {
            get
            {
                return _exceptionLoggingSP ??
                (_exceptionLoggingSP = (ConfigurationManager.AppSettings[KeyStorage.AppSettingKeys.ExceptionLoggingSp] ??
                    "dbo.spWriteExceptionLog2"));
            }
        }

        protected const string ConStringName = "log";
        private static string _connectionString;

        protected virtual string GetConnectionString()
        {
            return _connectionString ??
                   (_connectionString = ConfigurationManager.ConnectionStrings[ConStringName].ConnectionString);
        }

        #endregion
    }
}
