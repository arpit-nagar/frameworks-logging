using System;
using Tavisca.Frameworks.Logging.Infrastructure;
using Tavisca.Frameworks.Logging.Resources;

namespace Tavisca.Frameworks.Logging
{
    /// <summary>
    /// Default implementation of <see cref="ITransactionEntry"/>, this class can be inherited and extended.
    /// </summary>
    public class TransactionEntry : LogEntryBase, ITransactionEntry
    {
        #region ITransactionEntry Members

        private bool _requestChanged;

        private string _request;
        public string Request
        {
            get
            {
                if (_requestChanged)
                {
                    _request = Serialize(RequestObject, ReqResSerializerType);

                    _requestChanged = false;

                    return _request;
                }
                return _request;
            }
        }

        private bool _responseChanged;

        private string _response;

        public string Response
        {
            get
            {
                if (_responseChanged)
                {
                    _response = Serialize(ResponseObject, ReqResSerializerType);

                    _responseChanged = false;

                    return _response;
                }
                return _response;
            }
        }

        private object _requestObj;
        private object _responseObject;

        public object RequestObject
        {
            get
            {
                return _requestObj;
            }
            set
            {
                _requestChanged = true;
                _requestObj = value;
            }
        }
        public object ResponseObject
        {
            get
            {
                return _responseObject;
            }
            set
            {
                _responseChanged = true;
                _responseObject = value;
            }
        }

        public SerializerType ReqResSerializerType { set; get; }
        public string ServiceUrl { get; set; }
        public string MethodName { get; set; }
        public string Status
        {
            get { return Enum.GetName(typeof(StatusOptions), this.StatusType); }
        }

        public StatusOptions StatusType { get; set; }
        public double TimeTaken { get; set; }

        public void SetRequestString(string request)
        {
            RequestObject = null;
            _requestChanged = false;
            _request = request;
        }

        public void SetResponseString(string response)
        {
            ResponseObject = null;
            _responseChanged = false;
            _response = response;
        }

        public override ILogEntry Clone()
        {
            var entry = new TransactionEntry();

            this.Clone(entry);

            entry.ServiceUrl = this.ServiceUrl;
            entry.MethodName = this.MethodName;
            entry.ReqResSerializerType = this.ReqResSerializerType;

            if (this.RequestObject != null)
                entry.RequestObject = this.RequestObject;
            else if (!string.IsNullOrEmpty(this.Request))
                entry.SetRequestString(this.Request);

            if (this.ResponseObject != null)
                entry.ResponseObject = this.ResponseObject;
            else if (!string.IsNullOrEmpty(this.Response))
                entry.SetResponseString(this.Response);

            entry.StatusType = this.StatusType;
            entry.TimeTaken = this.TimeTaken;

            return entry;
        }

        #endregion

        #region Constructors

        public TransactionEntry()
        {
            this.InitializeIntrinsicProperties();
            this.SeverityType = SeverityOptions.Information;
            this.Title = this.Severity;
            this.TimeTaken = double.MinValue;
        }

        #endregion

        #region Protected Members

        protected static string Serialize(object val, SerializerType serializerType)
        {
            if (val == null)
                return null;

            var serialized = val as string;
            if (serialized != null)
                return serialized;

            switch (serializerType)
            {
                case SerializerType.DataContractSerializer:
                    return SerializationFactory.DataContractSerialize(val);
                case SerializerType.DataContractJsonSerializer:
                    return SerializationFactory.DataContractJsonSerialize(val);
                case SerializerType.JsonNetSerializer:
                    return SerializationFactory.JsonNetSerialize(val);
                case SerializerType.XmlSerializer:
                default:
                    return SerializationFactory.XmlSerialize(val);
            }
        }

        #endregion
    }
}
