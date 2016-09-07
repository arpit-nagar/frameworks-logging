using System;
using Tavisca.Frameworks.Logging.Infrastructure;

namespace Tavisca.Frameworks.Logging
{
    public class EventEntry : LogEntryBase, IEventEntry
    {
        #region IEventEntry Members

        public SerializerType ReqResSerializerType { set; get; }

        public string EventType { get; set; }

        private object _payloadObject;
        private bool _isPayloadChanged;
        private string _payload;

        public string Payload
        {
            get
            {
                if (_isPayloadChanged)
                {
                    _payload = Serialize(PayloadObject, ReqResSerializerType);

                    _isPayloadChanged = false;

                    return _payload;
                }
                return _payload;
            }
        }

        public object PayloadObject
        {
            get { return _payloadObject; }
            set
            {
                _isPayloadChanged = true;
                _payloadObject = value;
            }
        }

        public string Source { get; set; }

        public void SetPayloadString(string payload)
        {
            PayloadObject = null;
            _isPayloadChanged = false;
            _payload = payload;
        }

        public override ILogEntry CopyTo()
        {
            var entry = new EventEntry();

            this.CopyTo(entry);

            entry.Source = this.Source;
            entry.EventType = this.EventType;
            entry.ReqResSerializerType = this.ReqResSerializerType;

            if (PayloadObject != null)
                entry.PayloadObject = this.PayloadObject;
            else if (!string.IsNullOrEmpty(Payload))
                entry.SetPayloadString(this.Payload);

            return entry;
        }

        #endregion

        #region Constructors

        public EventEntry()
        {
            this.InitializeIntrinsicProperties();
            this.SeverityType = SeverityOptions.Information;
            this.Title = this.Severity;
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

            #endregion

        }
    }
}
