using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
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

        public override IEventEntry FormatEvent(IEventEntry eventEntry)
        {
            var configurations = GetConfiguration();

            var request = eventEntry.Request;
            var response = eventEntry.Response;

            if (configurations.Count > 0)
            {
                var requestChanged = false;
                var responseChanged = false;

                foreach (var configuration in configurations
                    .Where(x => !string.IsNullOrWhiteSpace(x.RegEx)
                        && !string.IsNullOrWhiteSpace(x.Replacement)))
                {
                    if (configuration.EventPredicate != null 
                        && configuration.EventPredicate.Invoke(eventEntry))
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
                    eventEntry.SetRequestString(request);

                if (responseChanged)
                    eventEntry.SetResponseString(response);
            }
            else
            {
                if (!string.IsNullOrEmpty(request))
                {
                    request = MaskCreditCardInfo(request, null);

                    eventEntry.SetRequestString(request);
                }

                if (!string.IsNullOrEmpty(response))
                {
                    response = MaskCreditCardInfo(response, null);

                    eventEntry.SetResponseString(response);
                }
            }

            return base.FormatEvent(eventEntry);
        }

        #endregion

        #region Protected Methods

        protected virtual string MaskCreditCardInfo(string value, CreditCardMaskFormatterConfiguration configuration)
        {
            if (configuration != null)
                return Regex.Replace(value, configuration.RegEx, configuration.Replacement);

            return Regex.Replace(value, @"(\d[\s|-]?){12,15}\d", MaskCCMatch); //default in case no config is present.
        }

        #endregion

        #region Private Methods

        private static string MaskCCMatch(Match match)
        {
            var ccnumber = match.ToString();

            return Regex.Replace(ccnumber.Substring(0, ccnumber.Length - 4), @"\d", "*") +
                   ccnumber.Substring(ccnumber.Length - 4);
        }

        #endregion
    }
}
