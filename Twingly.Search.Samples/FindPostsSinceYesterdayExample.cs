using System;
using Twingly.Search.Client;
using Twingly.Search.Client.Domain;
using Twingly.Search.Client.Exception;

namespace Twingly.Search.Samples
{
    /// <summary>
    /// Showcases how to retrieve all matching posts
    /// in a specific language
    /// that were published since yesterday
    /// </summary>
    public static class FindPostsSinceYesterdayExample
    {
        public static void RunExample()
        {
            // get all posts about 'Slack' since yesterday, in English.
            // limit to 10 posts.
            Console.WriteLine("-------Running the 'Find Posts Since Yesterday' example-------");
            Query theQuery = QueryBuilder.Create("Slack page-size:10 lang:en")
                                                .StartTime(DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)))
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
    }
}
