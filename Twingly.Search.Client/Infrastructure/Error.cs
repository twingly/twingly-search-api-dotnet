using System.Xml.Serialization;

namespace Twingly.Search.Client.Infrastructure
{
    [XmlRoot(ElementName = "error")]
    public class Error
    {
        [XmlAttribute("code")]
        public string Code { get; set; }

        [XmlElement("message")]
        public string Message { get; set; }
    }
}
