using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twingly.Search.Client
{
    /// <summary>
    /// Encapsulates parameters of a Twingly search.
    /// </summary>
    public class Query
    {
        /// <summary>
        /// The pattern to use when searching for blog posts
        /// </summary>
        public string SearchPattern
        {
            get;
            internal set;
        }
    }
}
