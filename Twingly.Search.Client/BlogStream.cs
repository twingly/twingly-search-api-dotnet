using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Twingly.Search.Client
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
