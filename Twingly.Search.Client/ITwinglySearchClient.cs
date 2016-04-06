using System.Threading.Tasks;
using Twingly.Search.Client.Domain;

namespace Twingly.Search.Client
{

    /// <summary>
    /// Enables querying the Twingly Search API.
    /// </summary>
    public interface ITwinglySearchClient
    {
        /// <summary>
        /// Gets or sets the HTTP request User-Agent property.
        /// </summary>
        string UserAgent { get; set; }

        /// <summary>
        /// Executes <paramref name="theQuery"/> and returns the result synchronously.
        /// </summary>
        /// <param name="theQuery">The blog search query that will be sent to the API.</param>
        /// <returns>
        /// Result of <paramref name="theQuery"/>.
        /// </returns>
        /// <exception cref="TwinglyServiceUnavailableException">Thrown when the Twingly API reports that service is unavailable.</exception>
        /// <exception cref="ApiKeyDoesNotExistException">Thrown when the supplied API key was not recognized by the remote server.</exception>
        /// <exception cref="UnauthorizedApiKeyException">
        /// Thrown when the supplied API key can't be used to service the query. 
        /// E.g. when the API key is limited to English language only, while the query was aimed at french blog posts.
        /// </exception>
        /// <exception cref="TwinglyRequestException">Thrown when any other communication error occurs.</exception>
        QueryResult Query(Query theQuery);

        /// <summary>
        /// Executes <paramref name="theQuery"/> and returns the result asynchronously.
        /// </summary>
        /// <param name="theQuery">The blog search query that will be sent to the API.</param>
        /// <returns>
        /// Result of <paramref name="theQuery"/>.
        /// </returns>
        /// <exception cref="TwinglyServiceUnavailableException">Thrown when the Twingly API reports that service is unavailable.</exception>
        /// <exception cref="ApiKeyDoesNotExistException">Thrown when the supplied API key was not recognized by the remote server.</exception>
        /// <exception cref="UnauthorizedApiKeyException">
        /// Thrown when the supplied API key can't be used to service the query. 
        /// E.g. when the API key is limited to English language only, while the query was aimed at french blog posts.
        /// </exception>
        /// <exception cref="TwinglyRequestException">Thrown when any other communication error occurs.</exception>
        Task<QueryResult> QueryAsync(Query theQuery);
    }
}