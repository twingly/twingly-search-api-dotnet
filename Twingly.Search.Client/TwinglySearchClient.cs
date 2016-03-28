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
        private static readonly int millisecondsTimeout = 10000;


        /// <summary>
        /// Initializes a new instance of <see cref="TwinglySearchClient"/>
        /// with the API key configured in the application configuration file.
        /// </summary>
        public TwinglySearchClient()
        {
            this.config = new FileConfiguration();
            this.internalClient = new HttpClient()
            {
                BaseAddress = new Uri(Constants.ApiBaseAddress)
            };
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
            if (theQuery == null)
                throw new ArgumentNullException("theQuery", "Hey, there's no way this argument can be null :(");
            theQuery.ThrowIfInvalid();

            string requestUri = BuildRequestUriFrom(theQuery);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Task<Stream> queryStreamTask = this.internalClient.GetStreamAsync(requestUri);
            queryStreamTask.Wait(millisecondsTimeout);
            sw.Stop();
            verboseTracer.TraceInformation("Received server response in {0} ms", sw.ElapsedMilliseconds);

            // TODO: check & handle faulted state.
            Stream queryStream = queryStreamTask.Result;
            sw.Restart();

            var returnValue = Extensions.XmlDeserialize(queryStream, typeof(QueryResult)) as QueryResult;

            sw.Stop();
            verboseTracer.TraceInformation("Deserialized server response in {0} ms", sw.ElapsedMilliseconds);

            return returnValue;
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
