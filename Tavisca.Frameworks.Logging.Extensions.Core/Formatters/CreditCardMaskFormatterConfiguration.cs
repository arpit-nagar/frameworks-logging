using System;

namespace Tavisca.Frameworks.Logging.Extensions.Formatters
{
    /// <summary>
    /// A container class for the configuration of <see cref="CreditCardMaskFormatter"/> class.
    /// </summary>
    public class CreditCardMaskFormatterConfiguration
    {
        public Predicate<ITransactionEntry> EventPredicate { get; set; }

        public string RegEx { get; set; }
        public string Replacement { get; set; }
    }
}