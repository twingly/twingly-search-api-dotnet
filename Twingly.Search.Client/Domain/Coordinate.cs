using System.Globalization;
using System.Xml.Serialization;

namespace Twingly.Search.Client.Domain
{
    [XmlRoot("coordinates")]
    public class Coordinate
    {
        public Coordinate()
        {
        }

        public Coordinate(string latitude, string longitude)
        {
            Latitude = double.Parse(latitude, CultureInfo.InvariantCulture);
            Longitude = double.Parse(longitude, CultureInfo.InvariantCulture);
        }

        public double? Latitude { get; }
        public double? Longitude { get; }
    }
}
