using System;

namespace Twingly.Search.Client.Domain
{
    /// <summary>
    /// Encapsulates parameters of a Twingly search.
    /// </summary>
    public class Query
    {
        private string _searchQuery = string.Empty;

        [Obsolete("Use SearchQuery instead")]
        public string SearchPattern
        {
            get => _searchQuery;
            set => SearchQuery = value;
        }

        /// <summary>
        /// The query to use when searching for blog posts
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the property is set to a null or empty string
        /// </exception>
        public string SearchQuery
        {
            get => _searchQuery;

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentOutOfRangeException(nameof(SearchQuery), "Please provide a non-empty string as a search query.");
                }
                _searchQuery = value;
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
        [Obsolete("Use lang: operator in search query instead")]
        public string Language
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new instance of a <see cref="Query"/>
        /// with the given <paramref name="searchQuery"/>.
        /// </summary>
        /// <param name="searchQuery">The query to use when searching for blog posts</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the <paramref name="searchQuery"/> is set to a null or empty string
        /// </exception>
        public Query(string searchQuery)
        {
            SearchQuery = searchQuery;
        }

        internal void ThrowIfNotAValidStartTime(DateTime startTime)
        {
            // allow 'give me the freshest posts' queries by accounting for
            // the delay between forming a query and the server actually receiving it.
            if (startTime > DateTime.UtcNow.AddHours(1))
            {
                throw new ArgumentOutOfRangeException(nameof(startTime), "Start time must be a UTC point in the past.");
            }
        }

        internal void ThrowIfNotAValidEndTime(DateTime endTime)
        {

            if (StartTime.HasValue && endTime < StartTime)
            {
                throw new ArgumentOutOfRangeException(nameof(endTime), "End time must be a UTC point in time that comes after the StartTime");
            }
        }

        internal void ThrowIfInvalid()
        {
            if (StartTime.HasValue)
            {
                ThrowIfNotAValidStartTime(StartTime.Value);
            }
            if (EndTime.HasValue)
            {
                ThrowIfNotAValidEndTime(EndTime.Value);
            }
        }
    }
}
