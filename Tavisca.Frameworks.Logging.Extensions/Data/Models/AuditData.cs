namespace Tavisca.Frameworks.Logging.Extensions.Data.Models
{
    public class AuditData
    {
        public long Id { get; set; }
        public long LogId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
