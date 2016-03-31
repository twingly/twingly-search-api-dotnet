using System.Xml.Serialization;

namespace Twingly.Search.Client.Infrastructure
{
    [XmlRoot(ElementName = "blogstream", Namespace = "http://www.twingly.com")]
    public class BlogStream
    {
        [XmlElement(ElementName = "operationResult")]
        public OperationResult Result
        {
            get;
            set;
        }
    }
}
