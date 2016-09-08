using System.Collections.Generic;

namespace Tavisca.Frameworks.Logging.Extensions.Formatters
{
    /// <summary>
    /// An interface which can be plugged into the <see cref="CreditCardMaskFormatter"/> class to provide
    /// conditional formatting.
    /// </summary>
    public interface ICreditCardMaskDataProvider
    {
        ICollection<CreditCardMaskFormatterConfiguration> GetConfiguration();
    }
}