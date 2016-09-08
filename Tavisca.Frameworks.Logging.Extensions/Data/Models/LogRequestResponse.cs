namespace Tavisca.Frameworks.Logging.Extensions.Data.Models
{
    public class LogRequestResponse
    {
        public int Id { get; set; }
        public int LogId { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public virtual Log Log { get; set; }
    }
}
