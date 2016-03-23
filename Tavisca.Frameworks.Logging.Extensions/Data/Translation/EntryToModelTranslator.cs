using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tavisca.Frameworks.Logging.DependencyInjection;
using Tavisca.Frameworks.Logging.Extensions.Data.Models;
using Tavisca.Frameworks.Logging.Infrastructure;

namespace Tavisca.Frameworks.Logging.Extensions.Data.Translation
{
    public class EntryToModelTranslator
    {
        public EntryToModelTranslator() { }

        #region To Model

        public Log ToModel(IEventEntry entry)
        {
            var model = new Log()
                {
                    AppDomainName = entry.AppDomainName,
                    ApplicationName = entry.ApplicationName,
                    CallType = entry.CallType,
                    ContextIdentifier = entry.ContextIdentifier,
                    IpAddress = entry.IpAddress,
                    MachineName = entry.MachineName,
                    Message = entry.Message,
                    Priority = entry.Priority,
                    ProcessID = entry.ProcessId,
                    ProcessName = entry.ProcessName,
                    ProviderId = entry.ProviderId != int.MinValue ? new int?(entry.ProviderId) : null,
                    SessionId = string.IsNullOrWhiteSpace(entry.SessionId) ? null : TryParseGuid(entry.SessionId),
                    Severity = entry.Severity,
                    Status = entry.Status,
                    ThreadName = entry.ThreadName,
                    TimeTaken = entry.TimeTaken != double.MinValue ? new double?(entry.TimeTaken) : null,
                    Timestamp = entry.Timestamp,
                    Title = entry.Title,
                    UserIdentifier = entry.UserIdentifier,
                    UsersessionId = entry.UserSessionId,
                    Win32ThreadId = entry.Win32ThreadId
                };

            if (!string.IsNullOrWhiteSpace(entry.Request) || !string.IsNullOrWhiteSpace(entry.Response))
            {
                model.LogRequestResponses.Add(new LogRequestResponse()
                    {
                        Request = entry.Request,
                        Response = entry.Response
                    });
            }

            if (entry.AdditionalInfo != null && entry.AdditionalInfo.Count > 0)
            {
                foreach (var pair in entry.AdditionalInfo)
                {
                    model.LogDatas.Add(new LogData()
                        {
                            Key = pair.Key,
                            Value = pair.Value
                        });
                }
            }

            return model;
        }

        public ExceptionLog ToModel(IExceptionEntry entry)
        {
            var model = new ExceptionLog()
            {
                AppDomainName = entry.AppDomainName,
                ApplicationName = entry.ApplicationName,
                ContextIdentifier = entry.ContextIdentifier,
                IpAddress = entry.IpAddress,
                MachineName = entry.MachineName,
                Message = entry.Message,
                Priority = entry.Priority,
                ProcessID = entry.ProcessId,
                ProcessName = entry.ProcessName,
                SessionId = entry.SessionId,
                Severity = entry.Severity,
                ThreadName = entry.ThreadName,
                Timestamp = entry.Timestamp,
                Title = entry.Title,
                UserIdentifier = entry.UserIdentifier,
                UsersessionId = entry.UserSessionId,
                Win32ThreadId = entry.Win32ThreadId,
                InnerExceptions = entry.InnerExceptions,
                Source = entry.Source,
                StackTrace = entry.StackTrace,
                TargetSite = entry.TargetSite,
                ThreadIdentity = entry.ThreadIdentity,
                Type = entry.Type
            };

            if (entry.AdditionalInfo != null && entry.AdditionalInfo.Count > 0)
            {
                foreach (var pair in entry.AdditionalInfo)
                {
                    model.ExceptionDatas.Add(new ExceptionData()
                    {
                        Key = pair.Key,
                        Value = pair.Value
                    });
                }
            }

            return model;
        }

        #endregion

        #region To Entry

        public IEventEntry ToEntry(Log model)
        {
            var entry = new EventEntry()
                {
                    AppDomainName = model.AppDomainName,
                    ApplicationName = model.ApplicationName,
                    CallType = model.CallType,
                    ContextIdentifier = model.ContextIdentifier,
                    IpAddress = model.IpAddress,
                    LogId = model.LogID,
                    MachineName = model.MachineName,
                    PriorityType = TryParsePriority(model.Priority),
                    ProviderId = model.ProviderId.HasValue ? model.ProviderId.Value : int.MinValue,
                    ProcessName = model.ProcessName,
                    ProcessId = model.ProcessID,
                    SessionId = model.SessionId.HasValue ? model.SessionId.Value.ToString() : null,
                    SeverityType = TryParseSeverity(model.Severity),
                    Title = model.Title,
                    StatusType = TryParseStatus(model.Status),
                    ThreadName = model.ThreadName,
                    TimeTaken = model.TimeTaken.HasValue ? model.TimeTaken.Value : 0.0,
                    Timestamp = model.Timestamp,
                    UserIdentifier = model.UserIdentifier,
                    UserSessionId = model.UsersessionId,
                    Win32ThreadId = model.Win32ThreadId
                };

            entry.AddMessage(model.Message);

            foreach (var logData in model.LogDatas)
            {
                entry.AddAdditionalInfo(logData.Key, logData.Value);
            }

            if (model.LogRequestResponses.Count > 0)
            {
                var reqRes = model.LogRequestResponses.First();

                var request = reqRes.Request;
                var response = reqRes.Response;

                try
                {
                    if (!string.IsNullOrWhiteSpace(request))
                        request = request.DeCompress();

                    if (!string.IsNullOrWhiteSpace(response))
                        response = response.DeCompress();
                }
                catch
                {
                    //suppress this exception.
                }

                entry.SetRequestString(request);
                entry.SetResponseString(response);
            }

            return entry;
        }

        public IExceptionEntry ToEntry(ExceptionLog model)
        {
            var entry = new ExceptionEntry()
            {
                AppDomainName = model.AppDomainName,
                ApplicationName = model.ApplicationName,
                ContextIdentifier = model.ContextIdentifier,
                IpAddress = model.IpAddress,
                LogId = model.LogID,
                MachineName = model.MachineName,
                PriorityType = TryParsePriority(model.Priority),
                ProcessName = model.ProcessName,
                ProcessId = model.ProcessID,
                SessionId = model.SessionId,
                SeverityType = TryParseSeverity(model.Severity),
                Title = model.Title,
                ThreadName = model.ThreadName,
                Timestamp = model.Timestamp,
                UserIdentifier = model.UserIdentifier,
                UserSessionId = model.UsersessionId,
                Win32ThreadId = model.Win32ThreadId,
                Source = model.Source,
                StackTrace = model.StackTrace,
                Type = model.Type,
                TargetSite = model.TargetSite,
                ThreadIdentity = model.ThreadIdentity
            };

            entry.AddMessage(model.Message);

            entry.AddInnerExceptionMessage(model.InnerExceptions, true);
            
            foreach (var logData in model.ExceptionDatas)
            {
                entry.AddAdditionalInfo(logData.Key, logData.Value);
            }

            return entry;
        }

        #endregion

        #region Helpers

        protected PriorityOptions TryParsePriority(int val)
        {
            var vals = Enum.GetValues(typeof(PriorityOptions));

            if (vals.Cast<int>().Any(value => val == value))
            {
                return (PriorityOptions)val;
            }

            return PriorityOptions.Undefined;
        }

        protected SeverityOptions TryParseSeverity(string val)
        {
            SeverityOptions severity;

            if (Enum.TryParse(val, true, out severity))
                return severity;

            return SeverityOptions.Undefined;
        }

        protected StatusOptions TryParseStatus(string val)
        {
            StatusOptions status;

            if (Enum.TryParse(val, true, out status))
                return status;

            return StatusOptions.Success;
        }

        protected Guid? TryParseGuid(string text)
        {
            Guid retVal;
            if (Guid.TryParse(text, out retVal))
                return retVal;

            return new Guid?();
        }

        #endregion
    }
}
