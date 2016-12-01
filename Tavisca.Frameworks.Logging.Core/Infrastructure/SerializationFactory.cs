using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Tavisca.Frameworks.Logging.Infrastructure
{
    /// <summary>
    /// The serialization factory, contains methods which are responsible to serialize a 
    /// managed object into a re-creatable cross platform version of the same.
    /// </summary>
    internal static class SerializationFactory
    {
        private static readonly Dictionary<Type, DataContractSerializer> DataContractSerializerCache = new Dictionary<Type, DataContractSerializer>();
        private static readonly ReaderWriterLockSlim SyncDataContractObject = new ReaderWriterLockSlim();

        private static readonly Dictionary<Type, XmlSerializer> XMLSerializerCache = new Dictionary<Type, XmlSerializer>();
        private static readonly ReaderWriterLockSlim SyncXmlObject = new ReaderWriterLockSlim();


        private static readonly Dictionary<Type, DataContractJsonSerializer> JsonSerializerCache = new Dictionary<Type, DataContractJsonSerializer>();
        private static readonly ReaderWriterLockSlim SyncJsonObject = new ReaderWriterLockSlim();


        public static int Timeout = 60000;

        public static string GetXmlElementORAttributeName(Type objectType, string property)
        {
            PropertyInfo info = objectType.GetTypeInfo().GetProperty(property);
            var props = info.GetCustomAttributes(true);
            if (props != null)
            {
                foreach (object attr in props)
                {
                    var attribute = attr as XmlElementAttribute;
                    if (attribute != null)
                    {
                        return attribute.ElementName;
                    }
                    var attribute2 = attr as XmlAttributeAttribute;

                    if (attribute2 != null)
                    {
                        return attribute2.AttributeName;
                    }
                }
            }
            return info.Name;
        }

        public static string XmlSerialize(object obj)
        {
            if (obj == null)
                return null;

            try
            {
                XmlSerializer reqSerializer = GetXmlSerializer(obj.GetType());

                using (TextWriter reqWriter = new StringWriter())
                {
                    reqSerializer.Serialize(reqWriter, obj);
                    reqWriter.Flush();
                    return reqWriter.ToString();
                }
            }
            catch
            {
                return null;
            }
        }

        private static XmlSerializer GetXmlSerializer(Type type)
        {
            SyncXmlObject.EnterReadLock();
            try
            {
                if (XMLSerializerCache.ContainsKey(type))
                {
                    return XMLSerializerCache[type];
                }
            }
            finally
            {
                SyncXmlObject.ExitReadLock();
            }

            var serializer = new XmlSerializer(type);
            SyncXmlObject.EnterWriteLock();
            try
            {

                if (!XMLSerializerCache.ContainsKey(type))
                {
                    XMLSerializerCache.Add(type, serializer);
                }
                return serializer;
            }
            finally
            {
                SyncXmlObject.ExitWriteLock();
            }
        }

        public static T XmlDeserialize<T>(string xml)
        {
            var type = typeof(T);
            XmlSerializer reqSerializer = GetXmlSerializer(type);
            try
            {
                using (TextReader reader = new StringReader(xml))
                {
                    return (T)reqSerializer.Deserialize(reader);
                }
            }
            catch (Exception)
            {
                var ser = new XmlSerializer(type);
                using (TextReader reader = new StringReader(xml))
                {
                    lock (XMLSerializerCache)
                    {
                        XMLSerializerCache.Remove(type);
                        XMLSerializerCache.Add(type, ser);
                    }
                    return (T)ser.Deserialize(reader);
                }

            }
        }

        public static string DataContractSerialize(object obj)
        {
            try
            {
                var type = obj.GetType();

                var serializer = GetDataContractSerializer(type);
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        serializer.WriteObject(memoryStream, obj);
                        var data = new byte[memoryStream.Length];
                        ArraySegment<byte> buffer;
                        memoryStream.TryGetBuffer(out buffer);
                        Array.Copy(buffer.Array, data, data.Length);
                        return Encoding.UTF8.GetString(data);
                    }
                }
                catch
                {
                    var ser = new DataContractSerializer(type);
                    using (var memoryStream = new MemoryStream())
                    {
                        ser.WriteObject(memoryStream, obj);
                        var data = new byte[memoryStream.Length];
                        ArraySegment<byte> buffer;
                        memoryStream.TryGetBuffer(out buffer);
                        Array.Copy(buffer.Array, data, data.Length);
                        SyncDataContractObject.EnterWriteLock();
                        try
                        {
                            DataContractSerializerCache.Remove(type);
                            DataContractSerializerCache.Add(type, ser);
                        }
                        finally
                        {
                            SyncDataContractObject.ExitWriteLock();
                        }
                        return Encoding.UTF8.GetString(data);
                    }
                }
            }
            catch
            {
            }
            return string.Empty;
        }

        public static DataContractSerializer GetDataContractSerializer(Type type)
        {
            SyncDataContractObject.EnterReadLock();
            try
            {
                if (DataContractSerializerCache.ContainsKey(type))
                {
                    return DataContractSerializerCache[type];
                }
            }
            finally
            {
                SyncDataContractObject.ExitReadLock();
            }

            SyncDataContractObject.EnterWriteLock();

            try
            {
                var serializer = new DataContractSerializer(type);
                DataContractSerializerCache.Add(type, serializer);
                return serializer;
            }
            finally
            {
                SyncDataContractObject.ExitWriteLock();
            }
        }

        public static T DataContractDeSerialize<T>(string data)
        {
            try
            {
                var type = typeof(T);

                using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
                {
                    var dcs = GetDataContractSerializer(type);//new DataContractSerializer(type);
                    return (T)dcs.ReadObject(memoryStream);

                }
            }
            catch { }
            return default(T);
        }


        public static T ParseEnum<T>(string value, T defaultValue)
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            if (Enum.IsDefined(typeof(T), value))
            {
                return (T)Enum.Parse(typeof(T), value);
            }
            return defaultValue;
        }


        public static string DataContractJsonSerialize(object t)
        {
            var ser = GetJsonSerializer(t.GetType());
            using (var ms = new MemoryStream())
            {
                ser.WriteObject(ms, t);

                var jsonString = Encoding.UTF8.GetString(ms.ToArray());

                return jsonString;
            }
        }

        public static T DataContractJsonDeserialize<T>(string data)
        {
            var ser = GetJsonSerializer(typeof(T));

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                var retVal = ser.ReadObject(ms);

                return (T)retVal;
            }
        }

        private static DataContractJsonSerializer GetJsonSerializer(Type type)
        {
            SyncJsonObject.EnterReadLock();
            try
            {
                if (JsonSerializerCache.ContainsKey(type))
                {
                    return JsonSerializerCache[type];
                }
            }
            finally
            {
                SyncJsonObject.ExitReadLock();
            }

            var serializer = new DataContractJsonSerializer(type);
            SyncJsonObject.EnterWriteLock();
            try
            {

                if (!JsonSerializerCache.ContainsKey(type))
                {
                    JsonSerializerCache.Add(type, serializer);
                }
                return serializer;
            }
            finally
            {
                SyncJsonObject.ExitWriteLock();
            }
        }

        public static string JsonNetSerialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T JsonNetDeserialize<T>(string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }
    }
}
