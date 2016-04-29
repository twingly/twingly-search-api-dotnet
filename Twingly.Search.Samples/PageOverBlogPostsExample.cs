using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Twingly.Search.Client;
using Twingly.Search.Client.Domain;

namespace Twingly.Search.Samples
{
    /// <summary>
    /// Showcases how to retrieve all matching posts
    /// in a paged manner. Paging is done via the sliding window technique.
    /// <see cref="https://developer.twingly.com/resources/search/#pagination"/>
    /// </summary>
    public static class PageOverBlogPostsExample
    {
        public static void RunExample()
        {
            
            Console.WriteLine("-------Running the 'Page over blog posts' example-------");
            
            Query theQuery = QueryBuilder.Create("sort-order:asc sort:published page-size:500 (github) AND (hipchat OR slack)")
                                                .Build();

            ITwinglySearchClient client = new TwinglySearchClient();
            client.UserAgent = "Willy Wonka Chocolate Factory";
            int totalResultCount = 0;

            try
            {
                QueryResult matchingDocs = client.Query(theQuery);
                while (!matchingDocs.HasNoMoreResults)
                {
                    totalResultCount += matchingDocs.Posts.Count;
                    theQuery.StartTime = matchingDocs.Posts.Last().Published.Value.AddSeconds(1);
                    matchingDocs = client.Query(theQuery);
                }
            }
            catch (TwinglyRequestException ex)
            {
                Console.WriteLine
                    ("Something went wrong while performing the request. Here's the error: '{0}'", ex);
            }

            Console.WriteLine("--------------");
            Console.WriteLine("Processed {0} results", totalResultCount);
            Console.WriteLine("-------Done-------");
            Console.WriteLine();
        }
    }
}
