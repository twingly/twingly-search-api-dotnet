using System;
using Twingly.Search.Client;
using Twingly.Search.Client.Domain;
using Twingly.Search.Client.Exception;

namespace Twingly.Search.Samples
{
    /// <summary>
    /// Demos more advanced query language features,
    /// such as logical operators and sorting.
    /// </summary>
    /// <remarks>
    /// Learn more about the search language - https://app.twingly.com/blog_search?tab=documentation
    /// </remarks>
    public class AdvancedQueryLanguageExample
    {
        public static void RunExample()
        {
            // get all posts about 'Slack' since yesterday, in English.
            // limit to 10 posts.
            Console.WriteLine("-------Running the 'Advanced Query Language' example-------");
            Query theQuery = QueryBuilder.Create(GetSearchQueryWith("(Pasta Bolognese) OR (Pasta Carbonara)"))
                                                .Build();

            ITwinglySearchClient client = new TwinglySearchClient();
            client.UserAgent = "Willy Wonka Chocolate Factory";

            try
            {
                QueryResult matchingDocs = client.Query(theQuery);
                foreach (var doc in matchingDocs.Posts)
                {
                    Console.WriteLine("Title: '{0}', Date: '{1}', Url: '{2}'",
                                        doc.Title, doc.PublishedAt, doc.Url);
                }
            }
            catch (RequestException ex)
            {
                Console.WriteLine
                    ("Something went wrong while performing the request. Here's the error: '{0}'", ex);
            }

            Console.WriteLine("-------Done-------");
            Console.WriteLine();
        }

        private static string GetSearchQueryWith(string keywords)
        {
            // sort by publishing date, oldest posts come first, return 15 results per page.
            string searchQueryTemplate = "sort-order:asc sort:published page-size:15 {0}";

            return string.Format(searchQueryTemplate, keywords);
        }
    }
}
