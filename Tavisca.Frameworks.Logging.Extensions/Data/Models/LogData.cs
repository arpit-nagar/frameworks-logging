namespace Tavisca.Frameworks.Logging.Extensions.Data.Models
{
    public class LogData
    {
        public int LogId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public virtual Log Log { get; set; }
    }
}
