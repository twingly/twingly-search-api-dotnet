using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twingly.Search.Client.Domain;

namespace Twingly.Search.Client
{
    /// <summary>
    /// Allows fluent construction of a <see cref="Query"/>.
    /// </summary>
    public class QueryBuilder
    {
        private Query internalQuery = null;

        private QueryBuilder(string searchPattern)
        {
            internalQuery = new Query(searchPattern);
        }

        /// <summary>
        /// Creates a new query builder with the specified search pattern.
        /// </summary>
        /// <param name="searchPattern">The pattern to use when searching for blog posts.</param>
        /// <returns>A new query builder.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the specified search pattern is a null or empty string.
        /// </exception>
        public static QueryBuilder Create(string searchPattern)
        {
            return new QueryBuilder(searchPattern);
        }

        /// <summary>
        /// Sets the pattern to use when searching for blog posts.
        /// </summary>
        /// <param name="searchPattern">The pattern to use when searching for blog posts.</param>
        /// <returns>A half-baked query with the given <paramref name="searchPattern"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the specified search pattern is a null or empty string.
        /// </exception>
        /// <see cref="https://developer.twingly.com/resources/search-language/"/>
        public QueryBuilder SearchPattern(string searchPattern)
        {
            internalQuery.SearchPattern = searchPattern;
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
            internalQuery.ThrowIfNotAValidStartTime(targetDate);
            internalQuery.StartTime = targetDate;

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
            internalQuery.EndTime = targetDate;

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
        public QueryBuilder Language(Language language)
        {
            internalQuery.Language = language;
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
            internalQuery.ThrowIfInvalid();

            return internalQuery;
        }

    }
}
