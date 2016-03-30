using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Twingly.Search.Client
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
