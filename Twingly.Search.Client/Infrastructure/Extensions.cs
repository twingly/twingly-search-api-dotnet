using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Twingly.Search.Client.Domain;

namespace Twingly.Search.Client.Infrastructure
{
    internal static class Extensions
    {
        private static readonly ConcurrentDictionary<Type, XmlSerializer> SerializerCache =
            new ConcurrentDictionary<Type, XmlSerializer>();

        public static T DeserializeXml<T>(this string xmlString)
        {
            T returnValue = default(T);
            Type objType = typeof(T);
            XmlSerializer serializer;
            if (SerializerCache.ContainsKey(objType))
            {
                serializer = SerializerCache[objType];
            }
            else
            {
                serializer = new XmlSerializer(objType);
                SerializerCache[objType] = serializer;
            }

            using (StringReader reader = new StringReader(xmlString))
            {
                object result = serializer.Deserialize(reader);

                if (result is T variable)
                {
                    returnValue = variable;
                }
            }

            return returnValue;
        }

        public static string ReadStreamIntoString(this Stream sourceStream)
        {
            string returnValue;
            using (StreamReader reader = new StreamReader(sourceStream))
            {
                returnValue = reader.ReadToEnd();
            }

            return returnValue;
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

            return string.Empty;
        }
    }
}
