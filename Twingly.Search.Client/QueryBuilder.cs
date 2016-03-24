using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twingly.Search.Client
{
    /// <summary>
    /// Allows fluent construction of a <see cref="Query"/>
    /// </summary>
    public class QueryBuilder
    {
        private Query internalQuery = null;

        private QueryBuilder()
        {
            internalQuery = new Query();
        }

        /// <summary>
        /// Creates a new query builder with the specified search pattern
        /// </summary>
        /// <param name="searchPattern">The pattern to use when searching for blog posts</param>
        /// <returns>A new query builder</returns>
        public static QueryBuilder Create(string searchPattern)
        {
            return new QueryBuilder().SearchPattern(searchPattern);
        }

        /// <summary>
        /// Sets the pattern to use when searching for blog posts
        /// </summary>
        /// <param name="searchPattern">The pattern to use when searching for blog posts</param>
        /// <returns>A half-baked query with the given <paramref name="searchPattern"/></returns>
        public QueryBuilder SearchPattern(string searchPattern)
        {
            if (String.IsNullOrWhiteSpace(searchPattern))
                throw new ArgumentNullException("searchPattern", "Please set this argument to a non-empty string");
            internalQuery.SearchPattern = searchPattern;

            return this;
        }
    }
}
