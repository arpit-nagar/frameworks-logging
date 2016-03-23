namespace Tavisca.Frameworks.Logging.Extensions.Data.Models
{
    public class AuditTrail
    {
        public long Id { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public string ChainId { get; set; }
        public string ReferenceNumber { get; set; }
        public string EventType { get; set; }
        public string IPAddress { get; set; }
        public string Username { get; set; }
        public string Application { get; set; }
        public string Module { get; set; }
        public string Status { get; set; }
        public string Tags { get; set; }
    }
}
