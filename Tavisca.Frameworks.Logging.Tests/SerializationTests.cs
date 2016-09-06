using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.Frameworks.Logging.Infrastructure;

namespace Tavisca.Frameworks.Logging.Tests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void XmlSerializerTest()
        {
            TestSerialization(SerializerType.XmlSerializer);
        }

        [TestMethod]
        public void JsonNetSerializerTest()
        {
            TestSerialization(SerializerType.JsonNetSerializer);
        }

        [TestMethod]
        public void DefaultSerializerTest()
        {
            TestSerialization(SerializerType.DataContractSerializer, true);
        }

        [TestMethod]
        public void DataContractSerializerTest()
        {
            TestSerialization(SerializerType.XmlSerializer);
        }

        [TestMethod]
        public void DataContractJsonSerializerTest()
        {
            TestSerialization(SerializerType.XmlSerializer);
        }

        private void TestSerialization(SerializerType serializerType, bool dontSetSerializerType = false)
        {
            const int intData = 123;
            const string stringData = "sjkdnfkjsdnf";

            var entry = new TransactionEntry();

            if (!dontSetSerializerType)
                entry.ReqResSerializerType = serializerType;

            entry.RequestObject = new TestSerializableData()
                {
                    IntData = intData,
                    StringData = stringData
                };

            var serialized = entry.Request;

            var deserialized = Deserialize<TestSerializableData>(serialized, serializerType);

            Assert.IsNotNull(deserialized);
            Assert.AreEqual(intData, deserialized.IntData);
            Assert.AreEqual(stringData, deserialized.StringData);
        }

        private T Deserialize<T>(string data, SerializerType serializerType)
        {
            switch (serializerType)
            {
                case SerializerType.DataContractJsonSerializer:
                    return SerializationFactory.DataContractJsonDeserialize<T>(data);
                case SerializerType.DataContractSerializer:
                    return SerializationFactory.DataContractDeSerialize<T>(data);
                case SerializerType.JsonNetSerializer:
                    return SerializationFactory.JsonNetDeserialize<T>(data);
                case SerializerType.XmlSerializer:
                    return SerializationFactory.XmlDeserialize<T>(data);
            }
            Assert.Fail("The serializer type {0} being tested is not supported in the test, add the respective test cases.", serializerType);
            return default(T);
        }
    }

    [Serializable]
    [DataContract]
    public class TestSerializableData
    {
        [DataMember]
        public string StringData { get; set; }
        [DataMember]
        public int IntData { get; set; }
    }
}
