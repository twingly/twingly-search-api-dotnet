using System.Collections.Generic;
using System.Xml.Serialization;

namespace Twingly.Search.Client.Domain
{
    [XmlRoot(ElementName = "post")]
    public class Post
    {

        [XmlElement(ElementName = "url")]
        public string Url { get; set; }
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        [XmlElement(ElementName = "summary")]
        public string Summary { get; set; }
        [XmlElement(ElementName = "languageCode")]
        public string LanguageCode { get; set; }
        [XmlElement(ElementName = "published")]
        public string Published { get; set; }
        [XmlElement(ElementName = "indexed")]
        public string Indexed { get; set; }
        [XmlElement(ElementName = "blogUrl")]
        public string BlogUrl { get; set; }
        [XmlElement(ElementName = "blogName")]
        public string BlogName { get; set; }
        [XmlElement(ElementName = "authority")]
        public string Authority { get; set; }
        [XmlElement(ElementName = "blogRank")]
        public string BlogRank { get; set; }

        [XmlArray("tags")]
        [XmlArrayItem("tag")]
        public List<string> Tags { get; set; }

        [XmlAttribute(AttributeName = "contentType")]
        public string ContentType { get; set; }
    }

}
