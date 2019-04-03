using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Twingly.Search.Client.Domain;
using Twingly.Search.Client.Exception;
using Twingly.Search.Client.Infrastructure;

namespace Twingly.Search.Client
{
    /// <summary>
    /// Enables querying the Twingly Search API.
    /// </summary>
    public class TwinglySearchClient : ITwinglySearchClient
    {
        private readonly HttpClient _internalClient;
        private readonly TwinglyConfiguration _config;
        private static readonly string RequestFormat = "?apikey={0}&" + Constants.SearchQuery + "={1}";

        private static readonly string UserAgentTemplate = "{0}/.NET v." + Assembly.GetExecutingAssembly().GetName().Version;

        private string _userAgent;

        /// <summary>
        /// Gets or sets the HTTP request User-Agent property.
        /// </summary>
        public string UserAgent
        {
            get => _userAgent;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _userAgent = string.Format(UserAgentTemplate, value);

                    _internalClient.DefaultRequestHeaders.UserAgent.Clear();
                    _internalClient.DefaultRequestHeaders.UserAgent.ParseAdd(_userAgent);
                }
            }
        }


        /// <summary>
        /// Initializes a new instance of <see cref="TwinglySearchClient"/>
        /// with the API key configured in the application configuration file.
        /// </summary>
        public TwinglySearchClient() : this(new LocalConfiguration())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TwinglySearchClient"/>
        /// with the given <paramref name="config"/>
        /// </summary>
        public TwinglySearchClient(TwinglyConfiguration config)
        {
            _config = config;
            _internalClient = new HttpClient(new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate                         
            }
            );
            InitializeClient();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TwinglySearchClient"/>.
        /// Intended for automated testing only.
        /// </summary>
        internal TwinglySearchClient(TwinglyConfiguration clientConfig, HttpClient client)
        {
            _config = clientConfig;
            _internalClient = client;
            InitializeClient();
        }

        private void InitializeClient()
        {
            _internalClient.BaseAddress = new Uri(Constants.ApiBaseAddress);
            _internalClient.Timeout = TimeSpan.FromMilliseconds(_config.RequestTimeoutMilliseconds);
            UserAgent = "Twingly Search API Client";
        }

        /// <summary>
        /// Executes <paramref name="theQuery"/> and returns the result asynchronously.
        /// </summary>
        /// <param name="theQuery">The blog search query that will be sent to the API.</param>
        /// <returns>
        /// Result of <paramref name="theQuery"/>.
        /// </returns>
        public async Task<QueryResult> QueryAsync(Query theQuery)
        {
            if (theQuery == null)
            {
                throw new ArgumentNullException(nameof(theQuery));
            }
            theQuery.ThrowIfInvalid();

            string requestUri = BuildRequestUriFrom(theQuery);
            string result = string.Empty;
            QueryResult returnValue;
            try
            {
                using (var response = await _internalClient.GetAsync(requestUri).ConfigureAwait(false))
                {
                    result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw MapResponseToException(result);
                    }

                    returnValue = result.DeserializeXml<QueryResult>();
                }
            }
            catch (TaskCanceledException e)
            {
                throw new RequestException("The request has timed out", e);
            }

            return returnValue;
        }

        /// <summary>
        /// Executes <paramref name="theQuery"/> and returns the result synchronously.
        /// </summary>
        /// <param name="theQuery">The blog search query that will be sent to the API.</param>
        /// <returns>
        /// Result of <paramref name="theQuery"/>.
        /// </returns>
        public QueryResult Query(Query theQuery)
        {
            QueryResult returnValue = null;

            try
            {
                returnValue = QueryAsync(theQuery).Result;
            }
            catch (AggregateException asyncEx)
            {
                // throw the underlying exception without messing up the stack trace
                ExceptionDispatchInfo.Capture(asyncEx.InnerException).Throw();
            }

            return returnValue;
        }

        private static System.Exception MapResponseToException(string responseString)
        {
            if (string.IsNullOrWhiteSpace(responseString))
            {
                return new RequestException("Twingly Search API returned an empty response");
            }

            Error errorResponse;
            try
            {
                errorResponse = responseString.DeserializeXml<Error>();
            }
            catch (System.Exception ex)
            {
                return new RequestException("Couldn't deserialize API response. See the inner exception for details", ex);
            }

            var errorCode = errorResponse.Code;
            if (errorCode.StartsWith("4"))
            {
                if (errorCode.StartsWith("400") || errorCode.StartsWith("404"))
                {
                    return new QueryException($"{errorResponse.Message} (code:{errorCode})");
                }
                if (errorCode.StartsWith("401") || errorCode.StartsWith("402"))
                {
                    return new AuthException($"{errorResponse.Message} (code:{errorCode})");
                }
            }
            if (errorCode.StartsWith("5"))
            {
                return new ServerException($"{errorResponse.Message} (code:{errorCode})");
            }

            return new RequestException($"Twingly Search API returned an unknown error: {responseString}");
        }

        private string BuildRequestUriFrom(Query query)
        {
            var fullSearchQuery = new StringBuilder(query.SearchQuery);
            if (!string.IsNullOrWhiteSpace(query.Language))
            {
                fullSearchQuery.AppendFormat(" {0}:{1}", Constants.DocumentLanguage, query.Language);
            }
            if (query.StartTime.HasValue)
            {
                fullSearchQuery.AppendFormat(" {0}:{1}", Constants.StartTime, query.StartTime.Value.ToString(Constants.ApiDateFormat));
            }
            if (query.EndTime.HasValue)
            {
                fullSearchQuery.AppendFormat(" {0}:{1}", Constants.EndTime, query.EndTime.Value.ToString(Constants.ApiDateFormat));
            }

            return string.Format(RequestFormat, _config.ApiKey, Uri.EscapeDataString(fullSearchQuery.ToString()));
        }
    }
}
