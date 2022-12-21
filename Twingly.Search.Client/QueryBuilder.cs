using System;
using Twingly.Search.Client.Domain;
using Twingly.Search.Client.Infrastructure;

namespace Twingly.Search.Client
{
    /// <summary>
    /// Allows fluent construction of a <see cref="Query"/>.
    /// </summary>
    public class QueryBuilder
    {
        private readonly Query _internalQuery;

        private QueryBuilder(string searchQuery)
        {
            _internalQuery = new Query(searchQuery);
        }

        /// <summary>
        /// Creates a new query builder with the specified search query.
        /// </summary>
        /// <param name="searchQuery">The query to use when searching for blog posts.</param>
        /// <returns>A new query builder.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the specified search query is a null or empty string.
        /// </exception>
        public static QueryBuilder Create(string searchQuery)
        {
            return new QueryBuilder(searchQuery);
        }

        /// <summary>
        /// Sets the query to use when searching for blog posts.
        /// </summary>
        /// <param name="query">The query to use when searching for blog posts.</param>
        /// <returns>A half-baked query with the given <paramref name="query"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the specified search query is a null or empty string.
        /// </exception>
        /// <see cref="https://app.twingly.com/blog_search?tab=documentation"/>
        public QueryBuilder SearchQuery(string query)
        {
            _internalQuery.SearchQuery = query;

            return this;
        }

        /// <summary>
        /// Set to only include posts published after the given UTC timestamp.
        /// </summary>
        /// <param name="targetDate">UTC time to be used as a starting point for search.</param>
        /// <returns>
        /// A half-baked query with the given <paramref name="targetDate"/> as start time.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the <paramref name="targetDate"/> is a date in the future.
        /// </exception>
        public QueryBuilder StartTime(DateTime targetDate)
        {
            _internalQuery.ThrowIfNotAValidStartTime(targetDate);
            _internalQuery.StartTime = targetDate;

            return this;
        }

        /// <summary>
        /// Allows excluding posts older than the <paramref name="targetDate"/>.
        /// </summary>
        /// <param name="targetDate">The UTC time that will be used to exclude older posts.</param>
        /// <returns>
        /// A half-baked query with the given <paramref name="targetDate"/> as end time.
        /// </returns>
        public QueryBuilder EndTime(DateTime targetDate)
        {
            // we don't throw exceptions here, because an invalid end date
            // may be due to the property setting order.
            _internalQuery.EndTime = targetDate;

            return this;
        }

        /// <summary>
        /// Restricts the query to a specific language, for example 'en' or 'sv',
        /// Note that some API keys only grant access to specific language(s).
        /// </summary>
        /// <param name="language">Document language</param>
        /// <returns>
        /// A half-baked query with the given <paramref name="language"/>.
        /// </returns>
        [Obsolete("Use lang: operator in search query instead")]
        public QueryBuilder Language(Language language)
        {
            _internalQuery.Language = language.GetLanguageValue();

            return this;
        }

        /// <summary>
        /// Please resort to this method only if the overload accepting <see cref="Language(Twingly.Search.Client.Domain.Language)"/>
        /// doesn't cover the language you're interested in.
        /// Restricts the query to a specific language, for example 'en' or 'sv',
        /// Note that some API keys only grant access to specific language(s).
        /// </summary>
        /// <param name="language">Document language</param>
        /// <returns>
        /// A half-baked query with the given <paramref name="language"/>.
        /// </returns>
        [Obsolete("Use lang: operator in search query instead")]
        public QueryBuilder Language(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                throw new ArgumentException("Please set this argument to a non-empty string.", nameof(language));
            }

            _internalQuery.Language = language;

            return this;
        }

        /// <summary>
        /// Constructs a <see cref="Query"/> ready to be sent to the Twingly Search API.
        /// </summary>
        /// <returns>
        /// A <see cref="Query"/> ready to be sent to the Twingly Search API.
        /// </returns>
        public Query Build()
        {
            _internalQuery.ThrowIfInvalid();

            return _internalQuery;
        }
    }
}
