using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Twingly.Search.Client
{
    internal static class Extensions
    {
        private static ConcurrentDictionary<Type, XmlSerializer> serializerCache = 
            new ConcurrentDictionary<Type, XmlSerializer>();


        public static object XmlDeserialize(Stream xml, Type objType)
        {
            StreamReader stream = null;
            XmlTextReader reader = null;
            try
            {
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

                stream = new StreamReader(xml); 
                reader = new XmlTextReader(stream);
                return serializer.Deserialize(reader);
            }

            finally
            {
                if (stream != null) stream.Close();
                if (reader != null) reader.Close();
            }
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
