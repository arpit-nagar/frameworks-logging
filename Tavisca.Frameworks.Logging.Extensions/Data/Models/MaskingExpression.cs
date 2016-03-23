using System;

namespace Tavisca.Frameworks.Logging.Extensions.Data.Models
{
    public class MaskingExpression
    {
        public string ProviderName { get; set; }
        public string CallType { get; set; }
        public string Regex { get; set; }
        public string ReplacementExpression { get; set; }
        public DateTime AddDate { get; set; }
    }
}
