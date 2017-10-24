using System.Xml.Serialization;

namespace Twingly.Search.Client.Domain
{
    [XmlRoot("coordinates")]
    public class Coordinate
    {
        [XmlElement("latitude")]
        public double? Latitude { get; set; }

        [XmlElement("longitude")]
        public double? Longitude { get; set; }
    }
}
