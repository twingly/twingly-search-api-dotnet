using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Twingly.Search.Client.Domain
{
    /// <summary>
    /// Represents a single blog post.
    /// </summary>
    [XmlRoot("post")]
    public class Post
    {
        /// <summary>
        /// The url to the post.
        /// </summary>
        [XmlElement("url")]
        public string Url { get; set; }

        /// <summary>
        /// The title of the post, limited to 256 characters.
        /// </summary>
        [XmlElement("title")]
        public string Title { get; set; }

        /// <summary>
        /// The content of the post, stripped of HTML.
        /// Note that the contents of the element may be large (several kilobytes).
        /// If the content is excessively large we may shorten it to ensure the stability of the service.
        /// </summary>
        [XmlElement("text")]
        public string Text { get; set; }

        [Obsolete("Use Text instead"), XmlIgnore]
        public string Summary => Text;

        /// <summary>
        /// The ISO language code that represents the language the post was written in.
        /// </summary>
        [XmlElement("languageCode")]
        public string LanguageCode { get; set; }

        /// <summary>
        /// When the post was published, in UTC.
        /// If no publication date could be found in the post,
        /// the date will be set to when the post was indexed.
        /// </summary>
        [XmlIgnore]
        public DateTime PublishedAt => DateTime.Parse(SerializerBackingStringFieldPublished);

        [Obsolete("Use PublishedAt instead"), XmlIgnore]
        public DateTime Published => PublishedAt;

        [XmlElement("publishedAt")]
        public string SerializerBackingStringFieldPublished { get; set; }

        /// <summary>
        /// When Twingly indexed the post, in UTC.
        /// </summary>
        [XmlIgnore]
        public DateTime IndexedAt => DateTime.Parse(SerializerBackingStringFieldIndexed);

        [Obsolete("Use IndexedAt instead"), XmlIgnore]
        public DateTime Indexed => IndexedAt;

        [XmlElement("indexedAt")]
        public string SerializerBackingStringFieldIndexed { get; set; }

        /// <summary>
        /// The url to the blog.
        /// </summary>
        [XmlElement("blogUrl")]
        public string BlogUrl { get; set; }

        /// <summary>
        /// The name of the blog.
        /// </summary>
        [XmlElement("blogName")]
        public string BlogName { get; set; }

        /// <summary>
        /// The ID of the blog
        /// </summary>
        [XmlElement("blogId")]
        public string BlogId { get; set; }

        /// <summary>
        /// Indicates the authority of the blog, at time of indexing.
        /// <see cref="https://developer.twingly.com/resources/ranking#authority"/>
        /// </summary>
        [XmlElement("authority")]
        public double Authority { get; set; }

        /// <summary>
        /// Indicates how influential the blog is, at time of indexing,
        /// <see cref="https://developer.twingly.com/resources/ranking#blogrank"/>.
        /// </summary>
        [XmlElement("blogRank")]
        public double BlogRank { get; set; }

        /// <summary>
        /// The categories and tags used to describe the blog post.
        /// </summary>
        [XmlArray("tags"), XmlArrayItem("tag")]
        public List<string> Tags { get; set; }

        /// <summary>
        /// The blog post ID
        /// </summary>
        [XmlElement("id")]
        public string Id { get; set; }

        /// <summary>
        /// The blog post author
        /// </summary>
        [XmlElement("author")]
        public string Author { get; set; }

        [XmlElement("locationCode")]
        public string LocationCode { get; set; }

        [XmlElement("inLinksCount")]
        public int InLinksCount { get; set; }

        /// <summary>
        /// When Twingly reindexed the post, in UTC.
        /// </summary>
        [XmlIgnore]
        public DateTime ReindexedAt => DateTime.Parse(SerializerBackingStringFieldReIndexed);

        [XmlElement("reindexedAt")]
        public string SerializerBackingStringFieldReIndexed { get; set; }

        /// <summary>
        /// All links from the blog post to other resources.
        /// </summary>
        [XmlArray("links"), XmlArrayItem("link")]
        public List<string> Links { get; set; }

        /// <summary>
        /// Image URLs from the posts
        /// </summary>
        [XmlArray("images"), XmlArrayItem("image")]
        public List<string> Images { get; set; }

        [XmlElement("coordinates")]
        public Coordinate Coordinates { get; set; }
    }
}
