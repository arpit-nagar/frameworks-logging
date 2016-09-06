using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using Tavisca.Frameworks.Logging.Extensions.Settings;
using Tavisca.Frameworks.Logging.Formatting;


namespace Tavisca.Frameworks.Logging.Extensions.Formatters
{
    /// <summary>
    /// A formatter which extends the <see cref="DefaultFormatter"/> functionality and adds masking
    /// of credit card numbers in the request and response fields.
    /// </summary>
    public class CreditCardMaskFormatter : DefaultFormatter
    {
        #region private state

        private static ConcurrentDictionary<string, Regex> _regexStore
            = new ConcurrentDictionary<string, Regex>();
        private const string _ccMatchedPattern = @"\d";
        private const string _ccNumberPattern = @"(\d[\s|-]?){12,15}\d";

        #endregion
        #region Properties

        protected static ICreditCardMaskDataProvider Provider { get; set; }

        #endregion

        #region Configuration

        static CreditCardMaskFormatter()
        {
            Provider = GetProvider();
        }

        protected static ICreditCardMaskDataProvider GetProvider()
        {
            var providerType = ConfigurationManager.AppSettings[KeyStorage.AppSettingKeys.CreditCardMaskProvider];

            if (string.IsNullOrWhiteSpace(providerType))
                return null;

            var type = Type.GetType(providerType);

            if (type == null)
                return null;

            try
            {
                return (ICreditCardMaskDataProvider)Activator.CreateInstance(type);
            }
            catch
            {
                return null; //if type is incorrect ignore the provider.
            }
        }

        protected virtual ICollection<CreditCardMaskFormatterConfiguration> GetConfiguration()
        {
            var settings = HttpRuntime.Cache.Get(KeyStorage.CacheKeys.CreditCardMaskSettingsKey) as ICollection<CreditCardMaskFormatterConfiguration>;

            if (settings != null)
                return settings;

            if (Provider != null)
            {
                settings = Provider.GetConfiguration();
            }
            else
            {
                settings = new Collection<CreditCardMaskFormatterConfiguration>();
            }

            HttpRuntime.Cache.Add(KeyStorage.CacheKeys.CreditCardMaskSettingsKey, settings, null,
                                  Cache.NoAbsoluteExpiration, new TimeSpan(0, 1, 0, 0), CacheItemPriority.Default, null);

            return settings;
        }

        #endregion

        #region DefaultFormatter Overridden Methods

        public override ITransactionEntry FormatEvent(ITransactionEntry transactionEntry)
        {
            var configurations = GetConfiguration();

            var request = transactionEntry.Request;
            var response = transactionEntry.Response;

            if (configurations.Count > 0)
            {
                var requestChanged = false;
                var responseChanged = false;

                foreach (var configuration in configurations
                    .Where(x => !string.IsNullOrWhiteSpace(x.RegEx)
                        && !string.IsNullOrWhiteSpace(x.Replacement)))
                {
                    if (configuration.EventPredicate != null
                        && configuration.EventPredicate.Invoke(transactionEntry))
                    {
                        if (!string.IsNullOrEmpty(request))
                        {
                            request = MaskCreditCardInfo(request, configuration);
                            requestChanged = true;
                        }
                        if (!string.IsNullOrEmpty(response))
                        {
                            response = MaskCreditCardInfo(response, configuration);
                            responseChanged = true;
                        }
                    }
                }
                if (requestChanged)
                    transactionEntry.SetRequestString(request);

                if (responseChanged)
                    transactionEntry.SetResponseString(response);
            }
            else
            {
                if (!string.IsNullOrEmpty(request))
                {
                    request = MaskCreditCardInfo(request, null);

                    transactionEntry.SetRequestString(request);
                }

                if (!string.IsNullOrEmpty(response))
                {
                    response = MaskCreditCardInfo(response, null);

                    transactionEntry.SetResponseString(response);
                }
            }

            return base.FormatEvent(transactionEntry);
        }

        #endregion

        #region Protected Methods

        protected virtual string MaskCreditCardInfo(string value, CreditCardMaskFormatterConfiguration configuration)
        {
            Regex regex;
            if (configuration != null)
            {
                regex = _regexStore.GetOrAdd(configuration.RegEx, new Regex(configuration.RegEx));
                return regex.Replace(value, configuration.Replacement);
            }

            regex = _regexStore.GetOrAdd(_ccNumberPattern, new Regex(_ccNumberPattern));
            return regex.Replace(value, MaskCCMatch); //default in case no config is present.
        }

        #endregion

        #region Private Methods

        private static string MaskCCMatch(Match match)
        {
            var ccnumber = match.ToString();

            var regex = _regexStore.GetOrAdd(_ccMatchedPattern, new Regex(_ccMatchedPattern));
            return regex.Replace(ccnumber.Substring(0, ccnumber.Length - 4), "*") +
                   ccnumber.Substring(ccnumber.Length - 4);
        }

        #endregion
    }
}
