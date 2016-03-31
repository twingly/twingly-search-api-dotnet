using System.Collections.Generic;
using System.Xml.Serialization;

namespace Twingly.Search.Client.Domain
{

    [XmlRoot(ElementName = "twinglydata")]
    public class QueryResult
    {
        [XmlElement(ElementName = "post")]
        public List<Post> Post { get; set; }

        [XmlAttribute(AttributeName = "numberOfMatchesReturned")]
        public int NumberOfMatchesReturned { get; set; }

        [XmlAttribute(AttributeName = "secondsElapsed")]
        public double SecondsElapsed { get; set; }

        [XmlAttribute(AttributeName = "numberOfMatchesTotal")]
        public long NumberOfMatchesTotal { get; set; }
    }
}
