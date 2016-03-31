using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Twingly.Search.Client.Domain;

namespace Twingly.Search.Client.Infrastructure
{
    internal static class Extensions
    {
        private static ConcurrentDictionary<Type, XmlSerializer> serializerCache = 
            new ConcurrentDictionary<Type, XmlSerializer>();

        public static T DeserializeXml<T>(this string xmlString)
        {
            T returnValue = default(T);
            Type objType = typeof(T);
            XmlSerializer serializer = null;
            if (serializerCache.ContainsKey(objType))
            {
                serializer = serializerCache[objType];
            }
            else
            {
                serializer = new XmlSerializer(objType);
                serializerCache[objType] = serializer;
            }

            using (StringReader reader = new StringReader(xmlString))
            {
                object result = serializer.Deserialize(reader);

                if (result != null && result is T)
                {
                    returnValue = ((T)result);
                }
            }

            return returnValue;
        }

        public static string ReadStreamIntoString(this Stream sourceStream)
        {
            var returnValue = String.Empty;
            using (StreamReader reader = new StreamReader(sourceStream))
            {
                returnValue = reader.ReadToEnd();
            }

            return returnValue;
        }

        public static object XmlDeserialize(Stream xml, Type objType)
        {
            StreamReader stream = null;
            XmlTextReader reader = null;
            XmlSerializer serializer;
            if (serializerCache.ContainsKey(objType))
            {
                serializer = serializerCache[objType];
            }
            else
            {
                serializer = new XmlSerializer(objType);
                serializerCache[objType] = serializer;
            }

            // avoid any side effects by keeping the stream position intact.
            //long initialPosition = xml.Position;
            //xml.Position = 0;
            stream = new StreamReader(xml, Encoding.UTF8);
            reader = new XmlTextReader(stream);
            //xml.Position = initialPosition;

            // leaving it to the caller code to dispose the stream.
            // this shouldn't be an issue, because the readers
            // will be eventually garbage-collected anyway,
            // while the stream lifecycle management should be
            // courtesy of the caller.
            return serializer.Deserialize(reader);

        }

        public static string GetLanguageValue(this Language language)
        {
            var enumType = typeof(Language);
            var memInfo = enumType.GetMember(language.ToString());
            var targetAttribute = memInfo[0].GetCustomAttributes(false)
                                    .OfType<EnumMemberAttribute>()
                                    .FirstOrDefault();

            if (targetAttribute != null)
            {
                return targetAttribute.Value;
            }

            return String.Empty ;
        }
    }
}
