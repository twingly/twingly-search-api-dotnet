using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace Twingly.Search.Client.Domain
{
    /// <summary>
    /// Represents a single blog post.
    /// </summary>
    [XmlRoot(ElementName = "post")]
    public class Post
    {
        /// <summary>
        /// The url to the post.
        /// </summary>
        [XmlElement(ElementName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// The title of the post, limited to 256 characters.
        /// </summary>
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// The content of the post, stripped of HTML.
        /// Note that the contents of the element may be large (several kilobytes).
        /// If the content is excessively large we may shorten it to ensure the stability of the service.
        /// </summary>
        [XmlElement(ElementName = "summary")]
        public string Summary { get; set; }

        /// <summary>
        /// The ISO language code that represents the language the post was written in.
        /// </summary>
        [XmlElement(ElementName = "languageCode")]
        public string LanguageCode { get; set; }

        /// <summary>
        /// When the post was published, in UTC. 
        /// If no publication date could be found in the post,
        /// the date will be set to when the post was indexed.
        /// </summary>
        [XmlIgnore]
        public DateTime Published
        {
            get
            {
                return DateTime.Parse(SerializerBackingStringFieldPublished, null, DateTimeStyles.AdjustToUniversal);
            }
        }

        [XmlElement(ElementName = "published")]
        public string SerializerBackingStringFieldPublished { get; set; }

        /// <summary>
        /// When Twingly indexed the post, in UTC.
        /// </summary>
        [XmlIgnore]
        public DateTime Indexed
        {
            get
            {
                return DateTime.Parse(SerializerBackingStringFieldIndexed, null, DateTimeStyles.AdjustToUniversal);
            }
        }

        [XmlElement(ElementName = "indexed")]
        public string SerializerBackingStringFieldIndexed { get; set; }

        /// <summary>
        /// The url to the blog.
        /// </summary>
        [XmlElement(ElementName = "blogUrl")]
        public string BlogUrl { get; set; }

        /// <summary>
        /// The name of the blog.
        /// </summary>
        [XmlElement(ElementName = "blogName")]
        public string BlogName { get; set; }

        /// <summary>
        /// Indicates the authority of the blog, at time of indexing.
        /// <see cref="https://developer.twingly.com/resources/ranking#authority"/>
        /// </summary>
        [XmlElement(ElementName = "authority")]
        public double Authority { get; set; }

        /// <summary>
        /// Indicates how influential the blog is, at time of indexing,
        /// <see cref="https://developer.twingly.com/resources/ranking#blogrank"/>.
        /// </summary>
        [XmlElement(ElementName = "blogRank")]
        public double BlogRank { get; set; }

        /// <summary>
        /// The categories and tags used to describe the blog post.
        /// </summary>
        [XmlArray("tags")]
        [XmlArrayItem("tag")]
        public List<string> Tags { get; set; }

        [XmlAttribute(AttributeName = "contentType")]
        public string ContentType { get; set; }
    }

}
