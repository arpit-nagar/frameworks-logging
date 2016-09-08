using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Tavisca.Frameworks.Logging.Extensions.Formatters;

namespace Tavisca.Frameworks.Logging.Tests.Mock
{
    public class DummyCreditCardMaskProvider : ICreditCardMaskDataProvider
    {
        public ICollection<CreditCardMaskFormatterConfiguration> GetConfiguration()
        {
            return new Collection<CreditCardMaskFormatterConfiguration>()
                {
                    new CreditCardMaskFormatterConfiguration()
                        {
                            EventPredicate = x => x.CallType == "test" && x.ProviderId == 1,
                            RegEx = @"(\d[\s|-]?){12,15}\d",
                            Replacement = "Replaced!!!"
                        }
                };
        }
    }
}
