using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Twingly.Search.Client
{

    /// <summary>
    /// Enables querying the Twingly Search API.
    /// </summary>
    public class TwinglySearchClient
    {
        private readonly HttpClient internalClient = null;
        private readonly Configuration config = null;
        private readonly TraceSource verboseTracer = new TraceSource("TwinglySearchClient") { Switch = { Level = SourceLevels.Verbose} };
        private static readonly string requestFormat = "?key={0}&searchPattern={1}&xmloutputversion=2";
        private static readonly int millisecondsTimeout = 10000; // TODO: move to configuration file.

        private string userAgent =
            String.Format("Twingly Search .NET Client/{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

        /// <summary>
        /// Gets or sets the HTTP request User-Agent property.
        /// </summary>
        public string UserAgent
        {
            get
            {
                return this.userAgent;
            }
            set
            {
                if(!String.IsNullOrWhiteSpace(value))
                    this.userAgent = value;
            }
        }


        /// <summary>
        /// Initializes a new instance of <see cref="TwinglySearchClient"/>
        /// with the API key configured in the application configuration file.
        /// </summary>
        public TwinglySearchClient()
        {
            this.config = new FileConfiguration();
            this.internalClient = new HttpClient()
            {
                BaseAddress = new Uri(Constants.ApiBaseAddress),
            };

            internalClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TwinglySearchClient"/>.
        /// Intended for automated testing only.
        /// </summary>
        internal TwinglySearchClient(Configuration clientConfig, HttpClient client)
        {
            this.config = clientConfig;
            this.internalClient = client;
            this.internalClient.BaseAddress = new Uri(Constants.ApiBaseAddress);
            internalClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
        }

        /// <summary>
        /// Executes <paramref name="theQuery"/> and returns the result synchronously.
        /// </summary>
        /// <param name="theQuery">The blog search query that will be sent to the API.</param>
        /// <returns>
        /// Result of <paramref name="theQuery"/>.
        /// </returns>
        /// <exception cref="TwinglyRequestException">Thrown when a communication error occurs</exception>
        public QueryResult Query(Query theQuery)
        {
            if (theQuery == null)
                throw new ArgumentNullException("theQuery", "Hey, there's no way this argument can be null :(");
            theQuery.ThrowIfInvalid();

            string requestUri = BuildRequestUriFrom(theQuery);
            string result = String.Empty;
            QueryResult returnValue = null;
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                Task<Stream> queryStreamTask = this.internalClient.GetStreamAsync(requestUri);
                queryStreamTask.Wait(millisecondsTimeout);

                sw.Stop();
                verboseTracer.TraceInformation("Received server response in {0} ms", sw.ElapsedMilliseconds);

                sw.Restart();
                using (Stream resultStream = queryStreamTask.Result)
                {
                    result = resultStream.ReadStreamIntoString();
                }

                returnValue = result.DeserializeXml<QueryResult>();
                sw.Stop();
                verboseTracer.TraceInformation("Deserialized server response in {0} ms", sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                throw new TwinglyRequestException(ex);
            }

            if (returnValue == null)
            {
                var wrapperException = MapResponseToException(result);
                throw wrapperException;
            }

            return returnValue;
        }


        private Exception MapResponseToException(string responseString)
        {

            BlogStream errorResponse = null;

            if (String.IsNullOrWhiteSpace(responseString))
                return new TwinglyRequestException("Twingly API returned an empty response");
            try
            {
                errorResponse = responseString.DeserializeXml<BlogStream>();
            }
            catch (Exception ex)
            {
                return new TwinglyRequestException
                    ("Couldn't deserialize API response. See inner exception for details", ex);
            }

            if (errorResponse != null)
            {
                if (errorResponse.Result.Text.Equals(Constants.ServiceUnavailable, StringComparison.InvariantCultureIgnoreCase))
                    return new TwinglyServiceUnavailableException();
                else if (errorResponse.Result.Text.Equals(Constants.ApiKeyDoesNotExist, StringComparison.InvariantCultureIgnoreCase))
                    return new ApiKeyDoesNotExistException();
                else if (errorResponse.Result.Text.Equals(Constants.UnauthorizedApiKey, StringComparison.InvariantCultureIgnoreCase))
                    return new UnauthorizedApiKeyException();
                else
                    // since we managed to deserialize into BlogStream, we just don't know what the error is.
                    // This means that it's reasonable not to be afraid that the responseString is too large.
                    // It also shouldn't contain any sensitive data. Hence, including it into the error message is safe.
                    return new TwinglyRequestException
                        (String.Format("Twingly API returned an unknown error :( Here's what it says: {0}", responseString));
            }
            else
            {
                return new TwinglyRequestException();
            }
        }

        private string BuildRequestUriFrom(Query theQuery)
        {
            string returnValue = 
                String.Format(requestFormat, this.config.ApiKey, theQuery.SearchPattern);
            var builder = new StringBuilder(returnValue);
            if (theQuery.Language != null)
                builder.AppendFormat("documentlang={0}", theQuery.Language.Value.GetLanguageValue());
            if (theQuery.StartTime.HasValue)
                builder.AppendFormat("ts={0}", theQuery.StartTime.Value.ToString(Constants.ApiDateFormat));
            if (theQuery.EndTime.HasValue)
                builder.AppendFormat("tsTo={0}", theQuery.EndTime.Value.ToString(Constants.ApiDateFormat));

            return returnValue;
        }
    }
}
