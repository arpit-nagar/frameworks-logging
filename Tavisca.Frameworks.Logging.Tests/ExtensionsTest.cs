using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.Frameworks.Logging.Infrastructure;
using Tavisca.Frameworks.Logging.Tests.Mock;

namespace Tavisca.Frameworks.Logging.Tests
{
    [TestClass]
    public class ExtensionsTest
    {
        private volatile bool _hasReturnedFile;
        private volatile bool _hasRedisReturnedHandle;
        private volatile bool _hasReturnedEventVwrException;
        private volatile bool _hasReturnedEventVwrEvent;
        private volatile bool _hasReturnedDBException;
        private volatile bool _hasReturnedDBEvent;
        private volatile bool _hasReturnedspDBException;
        private volatile bool _hasReturnedspDBEvent;
        private volatile bool _hasReturnedDefaultEvent;

        [TestMethod, Priority(-1)]
        public void ToEntryBeforeLoggerTest()
        {
            var entry = new Exception().ToEntry();

            new Logger().Write(entry, null);
        }

        [TestMethod, Priority(0)]
        public void IPAddressInAdditionalInfoTest()
        {
            var ipAdresses = new List<string>();
            var ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            var addrList = ipEntry.AddressList;
            if (addrList != null)
            {
                foreach (IPAddress addr in addrList)
                {
                    ipAdresses.Add(addr.ToString());
                }
            }

            var exceptionEntry = GetExceptionEntry();
            Assert.IsFalse(string.IsNullOrWhiteSpace(exceptionEntry.IpAddress));

            foreach (var x in exceptionEntry.IpAddress.Split(','))
            {
                Assert.IsTrue(ipAdresses.Contains(x));
            }

            var eventEntry = GetEventEntry();
            Assert.IsFalse(string.IsNullOrWhiteSpace(eventEntry.IpAddress));

            foreach (var x in eventEntry.IpAddress.Split(','))
            {
                Assert.IsTrue(ipAdresses.Contains(x));
            }

        }

        [TestMethod]
        public void FileLoggerTest()
        {
            var factory = new Logger();

            factory.WriteExceptionAsync(new Exception("Root Level Test"), KeyStore.Categories.FileLogger, x => _hasReturnedFile = true);

            var count = 0;
            while (!_hasReturnedFile)
            {
                System.Threading.Thread.Sleep(500);

                count++;

                if (count > 500)
                    break;
            }
        }

        [TestMethod]
        public void EventViewerLoggerExceptionTest()
        {
            var factory = new Logger();

            var entry = GetExceptionEntry();

            factory.WriteAsync(entry, KeyStore.Categories.EventViewer, x => _hasReturnedEventVwrException = true);

            var count = 0;
            while (!_hasReturnedEventVwrException)
            {
                System.Threading.Thread.Sleep(500);

                count++;

                if (count > 500)
                    break;
            }
        }

        [TestMethod]
        public void EventViewerLoggerEventTest()
        {
            var factory = new Logger();

            var entry = GetEventEntry();

            entry.RequestObject = new Exception("test serialization");

            factory.WriteAsync(entry, KeyStore.Categories.EventViewer, x => _hasReturnedEventVwrEvent = true);

            var count = 0;
            while (!_hasReturnedEventVwrEvent)
            {
                System.Threading.Thread.Sleep(500);

                count++;

                if (count > 500)
                    break;
            }
        }


        [TestMethod]
        public void RedisLoggerSyncTest()
        {
            var factory = new Logger();

            var entry1 = GetEventEntry();

            factory.Write(entry1, KeyStore.Categories.RedisLogger);


            var entry2 = GetExceptionEntry();

            factory.Write(entry2, KeyStore.Categories.RedisLogger);
        }

        [TestMethod]
        public void RedisLoggerAsyncTest()
        {
            var factory = new Logger();
            var entry = GetExceptionEntry();
            factory.WriteAsync(entry, KeyStore.Categories.RedisLogger, x => _hasRedisReturnedHandle = true);

            var count = 0;
            while (!_hasRedisReturnedHandle)
            {
                System.Threading.Thread.Sleep(500);

                count++;

                if (count > 500)
                    break;
            }


            var entry2 = GetEventEntry();
            factory.WriteAsync(entry2, KeyStore.Categories.RedisLogger, x => _hasRedisReturnedHandle = true);

            count = 0;
            while (!_hasRedisReturnedHandle)
            {
                System.Threading.Thread.Sleep(500);

                count++;

                if (count > 500)
                    break;
            }
        }

        [TestMethod]
        public void DefaultLoggerTest()
        {
            var factory = new Logger();

            var entry = GetEventEntry();

            factory.WriteAsync(entry, null, x => _hasReturnedDefaultEvent = true);

            var count = 0;
            while (!_hasReturnedDefaultEvent)
            {
                System.Threading.Thread.Sleep(500);

                count++;

                if (count > 500)
                    break;
            }
        }

        #region Private Methods

        private ITransactionEntry GetEventEntry()
        {
            var entry = new TransactionEntry();

            entry.ServiceUrl = "www.testserviceurl.com/";
            entry.MethodName = "testmethodname";

            entry.SetRequestString("<?xml version=\"1.0\" encoding=\"utf-16\"?><OTA_AirPriceRQ xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" Version=\"2003A.TsabreXML1.13.1\"><POS xmlns=\"http://webservices.sabre.com/sabreXML/2003/07\"><Source PseudoCityCode=\"F1HF\" /></POS><TravelerInfoSummary xmlns=\"http://webservices.sabre.com/sabreXML/2003/07\"><PriceRequestInformation CurrencyCode=\"USD\"><TPA_Extensions /></PriceRequestInformation><TPA_Extensions><BargainFinder Ind=\"false\"><Rebook Ind=\"false\" /></BargainFinder><PassengerType Quantity=\"2\" Code=\"ADT\" /><PassengerType Quantity=\"2\" Code=\"C10\" /><PublicFare Ind=\"false\" /><PrivateFare Ind=\"false\" /><PriceRetention Default=\"true\" /></TPA_Extensions></TravelerInfoSummary></OTA_AirPriceRQ>");
            entry.SetResponseString("<?xml version=\"1.0\" encoding=\"utf-16\"?><OTA_AirPriceRS xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" EchoToken=\"String\" TimeStamp=\"2012-06-08T14:33:24\" Version=\"2003A.TsabreXML1.13.1\" SequenceNmbr=\"1\" PrimaryLangID=\"en-us\" AltLangID=\"en-us\"><Success xmlns=\"http://webservices.sabre.com/sabreXML/2003/07\" /><PricedItineraries xmlns=\"http://webservices.sabre.com/sabreXML/2003/07\"><PricedItinerary><AirItineraryPricingInfo PricingSource=\"F1HF/GSP\"><TPA_Extensions><AlternateBooking /><PurchaseText>VALIDATING CARRIER - US</PurchaseText><PurchaseText>VALIDATING CARRIER - US</PurchaseText><PurchaseText> 8JUL DEPARTURE DATE-----LAST DAY TO PURCHASE  9JUN</PurchaseText><PurchaseText>BAG ALLOWANCE     -LASLAX-02P/US</PurchaseText><PurchaseText>ADDITIONAL ALLOWANCES AND/OR DISCOUNTS MAY APPLY</PurchaseText><PurchaseText>BAG ALLOWANCE     -LASLAX-02P/US</PurchaseText><PurchaseText>ADDITIONAL ALLOWANCES AND/OR DISCOUNTS MAY APPLY</PurchaseText><LastTicketingDate>2012-06-09T23:59:00</LastTicketingDate><ValidatingCarrier Code=\"US\" /></TPA_Extensions><ItinTotalFare><TotalFare Amount=\"1394.40\" CurrencyCode=\"USD\" /></ItinTotalFare><PTC_FareBreakdown PricingSource=\"F1HF/GSP\"><PassengerTypeQuantity Code=\"ADT\" Quantity=\"2\" /><FareBasis Code=\"AAVUPNV\" RPH=\"1\" /><FareBasis Code=\"AAVUPNV\" Market=\"LASLAX\" Date=\"2012-07-08T00:00:00\" RPH=\"2\" /><PassengerFare><BaseFare Amount=\"304.19\" CurrencyCode=\"USD\" /><Taxes><Tax TaxCode=\"US\" Amount=\"22.81\" TaxName=\"US DOMESTIC TRANSPORTATION TAX\" DecimalPlaces=\"2\" /><Tax TaxCode=\"ZP\" Amount=\"7.60\" TaxName=\"SEGMENT TAX\" DecimalPlaces=\"2\" /><Tax TaxCode=\"AY\" Amount=\"5.00\" TaxName=\"US SECURITY FEE\" DecimalPlaces=\"2\" /><Tax TaxCode=\"XF\" Amount=\"9.00\" TaxName=\"PASSENGER FACILITY CHARGES\" DecimalPlaces=\"2\" /></Taxes><TPA_Extensions><Endorsements><Text>STNDBY/CHG FEE/NO RFND/CXL BY FLT DT/</Text></Endorsements><FareCalculation><Text>LAS US X/PHX US LAX304.19AAVUPNV USD304.19END ZPLASPHX XFLAS4.5PHX4.5</Text></FareCalculation><Text>BAG ALLOWANCE     -LASLAX-02P/USADDITIONAL ALLOWANCES AND/OR DISCOUNTS MAY APPLY</Text><Commission Amount=\"0.00\" /></TPA_Extensions><TotalFare Amount=\"348.60\" CurrencyCode=\"USD\" DecimalPlaces=\"2\" /></PassengerFare></PTC_FareBreakdown><PTC_FareBreakdown PricingSource=\"F1HF/GSP\"><PassengerTypeQuantity Code=\"C10\" Quantity=\"2\" /><FareBasis Code=\"AAVUPNV\" RPH=\"1\" /><FareBasis Code=\"AAVUPNV\" Market=\"LASLAX\" Date=\"2012-07-08T00:00:00\" RPH=\"2\" /><PassengerFare><BaseFare Amount=\"304.19\" CurrencyCode=\"USD\" /><Taxes><Tax TaxCode=\"US\" Amount=\"22.81\" TaxName=\"US DOMESTIC TRANSPORTATION TAX\" DecimalPlaces=\"2\" /><Tax TaxCode=\"ZP\" Amount=\"7.60\" TaxName=\"SEGMENT TAX\" DecimalPlaces=\"2\" /><Tax TaxCode=\"AY\" Amount=\"5.00\" TaxName=\"US SECURITY FEE\" DecimalPlaces=\"2\" /><Tax TaxCode=\"XF\" Amount=\"9.00\" TaxName=\"PASSENGER FACILITY CHARGES\" DecimalPlaces=\"2\" /></Taxes><TPA_Extensions><Endorsements><Text>STNDBY/CHG FEE/NO RFND/CXL BY FLT DT/</Text></Endorsements><FareCalculation><Text>LAS US X/PHX US LAX304.19AAVUPNV USD304.19END ZPLASPHX XFLAS4.5PHX4.5</Text></FareCalculation><Text>CNN NOT APPLICABLE - ADT FARE USED - VERIFY RESTRICTIONSBAG ALLOWANCE     -LASLAX-02P/USADDITIONAL ALLOWANCES AND/OR DISCOUNTS MAY APPLY</Text><Commission Amount=\"0.00\" /></TPA_Extensions><TotalFare Amount=\"348.60\" CurrencyCode=\"USD\" DecimalPlaces=\"2\" /></PassengerFare></PTC_FareBreakdown><FareRuleInfo><FareBasis Code=\"AAVUPNV\" RPH=\"1\" /><FilingAirline Code=\"US\" /><DepartureAirport LocationCode=\"LAS\" /><ArrivalAirport LocationCode=\"PHX\" /></FareRuleInfo><FareRuleInfo><FareBasis Code=\"AAVUPNV\" Market=\"LASLAX\" Date=\"2012-07-08T00:00:00\" RPH=\"2\" /><FilingAirline Code=\"US\" /><DepartureAirport LocationCode=\"PHX\" /><ArrivalAirport LocationCode=\"LAX\" /></FareRuleInfo><FareRuleInfo><FareBasis Code=\"AAVUPNV\" RPH=\"1\" /><FilingAirline Code=\"US\" /><DepartureAirport LocationCode=\"LAS\" /><ArrivalAirport LocationCode=\"PHX\" /></FareRuleInfo><FareRuleInfo><FareBasis Code=\"AAVUPNV\" Market=\"LASLAX\" Date=\"2012-07-08T00:00:00\" RPH=\"2\" /><FilingAirline Code=\"US\" /><DepartureAirport LocationCode=\"PHX\" /><ArrivalAirport LocationCode=\"LAX\" /></FareRuleInfo></AirItineraryPricingInfo></PricedItinerary></PricedItineraries><TPA_Extensions xmlns=\"http://webservices.sabre.com/sabreXML/2003/07\"><HostCommand>??????A????RS01S093??WPMUSD??NCB??P2ADT/2C10??RQ</HostCommand></TPA_Extensions></OTA_AirPriceRS>");

            entry.AddAdditionalInfo("someKey", "SomeValue");
            entry.AddAdditionalInfo("someotherKey", null);

            entry.PriorityType = PriorityOptions.Low;

            for (int i = 0; i < 10; i++)
            {
                entry.AddAdditionalInfo(i.ToString(), (i + 100).ToString());
            }

            return entry;
        }

        private IExceptionEntry GetExceptionEntry()
        {
            var entry = new ArgumentException("Root Level Test").ToEntry();

            entry.AddAdditionalInfo("someKey", "SomeValue");
            entry.AddAdditionalInfo("someotherKey", null);

            for (int i = 0; i < 10; i++)
            {
                entry.AddAdditionalInfo(i.ToString(), (i + 100).ToString());
            }

            return entry;
        }

        #endregion
    }
}
