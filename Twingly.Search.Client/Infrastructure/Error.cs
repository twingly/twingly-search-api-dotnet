using System.Xml.Serialization;

namespace Twingly.Search.Client.Infrastructure
{
    [XmlRoot("error")]
    public class Error
    {
        [XmlAttribute("code")]
        public string Code { get; set; }

        [XmlElement("message")]
        public string Message { get; set; }
    }
}
