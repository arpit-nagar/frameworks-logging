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
    public abstract class CreditCardMaskDataProviderBase: ICreditCardMaskDataProvider
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
            public string CallType { get; set; }
            public int? ProviderId { get; set; }
            public string RegEx { get; set; }
            public string ReplacementExpression { get; set; }
        }

        #endregion

        #region Protected Methods

        protected abstract IEnumerable<MaskExpressions> GetMaskingData();

        protected virtual Predicate<IEventEntry> GetPredicate(MaskExpressions configuration)
        {
            if (string.IsNullOrWhiteSpace(configuration.CallType) && !configuration.ProviderId.HasValue)
                return x => true;

            if (string.IsNullOrWhiteSpace(configuration.CallType))
                return x => x.ProviderId == configuration.ProviderId;

            if (!configuration.ProviderId.HasValue)
                return x => x.CallType.Equals(configuration.CallType, StringComparison.InvariantCultureIgnoreCase);

            return x => x.ProviderId == configuration.ProviderId &&
                        x.CallType.Equals(configuration.CallType, StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion
    }
}
