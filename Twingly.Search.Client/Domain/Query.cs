using System;

namespace Twingly.Search.Client.Domain
{
    /// <summary>
    /// Encapsulates parameters of a Twingly search.
    /// </summary>
    public class Query
    {
        private string searchPattern = String.Empty;

        /// <summary>
        /// The pattern to use when searching for blog posts
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the property is set to a null or empty string
        /// </exception>
        public string SearchPattern
        {
            get
            {
                return searchPattern;
            }

            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentOutOfRangeException
                        ("SearchPattern", "Please provide a non-empty string as a search pattern.");
                this.searchPattern = value;
            }
        }

        /// <summary>
        /// Used to only include posts published after a certain UTC timestamp. 
        /// </summary>
        public DateTime? StartTime
        {
            get;
            set;
        }

        /// <summary>
        /// Used to exclude posts published after a certain UTC timestamp. 
        /// </summary>
        public DateTime? EndTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the language filter to be used during search.
        /// </summary>
        public string Language
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new instance of a <see cref="Query"/>
        /// with the given <paramref name="searchPattern"/>.
        /// </summary>
        /// <param name="searchPattern">The pattern to use when searching for blog posts</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the <paramref name="searchPattern"/> is set to a null or empty string
        /// </exception>
        public Query(string searchPattern)
        {
            this.SearchPattern = searchPattern;
        }

        internal void ThrowIfNotAValidStartTime(DateTime startTime)
        {
            // allow 'give me the freshest posts' queries by accounting for
            // the delay between forming a query and the server actually receiving it.
            if (startTime > DateTime.UtcNow.AddHours(1))
                throw new ArgumentOutOfRangeException
                    (nameof(startTime), "Start time must be a UTC point in the past.");
        }

        internal void ThrowIfNotAValidEndTime(DateTime endTime)
        {

            if (this.StartTime.HasValue && endTime < this.StartTime)
                throw new ArgumentOutOfRangeException
                    (nameof(endTime), "End time must be a UTC point in time that comes after the StartTime");
        }

        internal void ThrowIfInvalid()
        {
            if (this.StartTime.HasValue)
                ThrowIfNotAValidStartTime(this.StartTime.Value);
            if (this.EndTime.HasValue)
                ThrowIfNotAValidEndTime(this.EndTime.Value);
        }
    }
}
