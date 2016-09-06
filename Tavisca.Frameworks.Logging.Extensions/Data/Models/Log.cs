using System;
using System.Collections.Generic;

namespace Tavisca.Frameworks.Logging.Extensions.Data.Models
{
    public class Log
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Log()
        {
            this.LogDatas = new List<LogData>();
            this.LogRequestResponses = new List<LogRequestResponse>();
        }
        public string CorrelationId { get; set; }
        public string TransactionId { get; set; }
        public string StackId { get; set; }
        public string TenantId { get; set; }
        public string InstanceId { get; set; }
        public int LogID { get; set; }
        public int Priority { get; set; }
        public string Severity { get; set; }
        public string Title { get; set; }
        public DateTime Timestamp { get; set; }
        public string MachineName { get; set; }
        public string AppDomainName { get; set; }
        public string ProcessID { get; set; }
        public string ProcessName { get; set; }
        public string ThreadName { get; set; }
        public string Win32ThreadId { get; set; }
        public string Message { get; set; }
        public Guid? SessionId { get; set; }
        public string CallType { get; set; }
        public int? ProviderId { get; set; }
        public string Status { get; set; }
        public double? TimeTaken { get; set; }
        public string UsersessionId { get; set; }
        public string UserIdentifier { get; set; }
        public string ApplicationName { get; set; }
        public string ContextIdentifier { get; set; }
        public string IpAddress { get; set; }
        public virtual ICollection<LogData> LogDatas { get; set; }
        public virtual ICollection<LogRequestResponse> LogRequestResponses { get; set; }
    }
}
