using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tavisca.Frameworks.Logging.Extensions.Formatters
{
    /// <summary>
    /// A base implementation of <see cref="ICreditCardMaskDataProvider"/>, provides helper methods
    /// to ease the final implementation.
    /// </summary>
    public abstract class CreditCardMaskDataProviderBase : ICreditCardMaskDataProvider
    {
        #region ICreditCardMaskDataProvider Members

        public ICollection<CreditCardMaskFormatterConfiguration> GetConfiguration()
        {
            return GetMaskingData().Select(x => new CreditCardMaskFormatterConfiguration()
            {
                RegEx = x.RegEx,
                Replacement = x.ReplacementExpression,
                EventPredicate = GetPredicate(x)
            }).ToArray();
        }

        #endregion

        #region Sub Classes

        public class MaskExpressions
        {
            public string ServiceUrl { get; set; }
            public string MethodName { get; set; }
            public string RegEx { get; set; }
            public string ReplacementExpression { get; set; }
        }

        #endregion

        #region Protected Methods

        protected abstract IEnumerable<MaskExpressions> GetMaskingData();

        protected virtual Predicate<ITransactionEntry> GetPredicate(MaskExpressions configuration)
        {
            if (string.IsNullOrWhiteSpace(configuration.ServiceUrl) && string.IsNullOrEmpty(configuration.MethodName))
                return x => true;

            if (string.IsNullOrWhiteSpace(configuration.MethodName))
                return x => x.ServiceUrl.Equals(configuration.ServiceUrl, StringComparison.CurrentCultureIgnoreCase);

            if (string.IsNullOrWhiteSpace(configuration.ServiceUrl))
                return x => x.MethodName.Equals(configuration.MethodName, StringComparison.CurrentCultureIgnoreCase);

            return x => x.ServiceUrl.Equals(configuration.ServiceUrl, StringComparison.CurrentCultureIgnoreCase) &&
                        x.MethodName.Equals(configuration.MethodName, StringComparison.CurrentCultureIgnoreCase);
        }

        #endregion
    }
}
