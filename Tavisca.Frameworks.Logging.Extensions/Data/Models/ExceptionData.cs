namespace Tavisca.Frameworks.Logging.Extensions.Data.Models
{
    public class ExceptionData
    {
        public int LogId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public virtual ExceptionLog ExceptionLog { get; set; }
    }
}
