using System;

namespace Twingly.Search.Client.Domain
{
    /// <summary>
    /// Allows easy access to the configuration settings.
    /// </summary>
    public class TwinglyConfiguration
    {
        /// <summary>
        /// Initializes a new configuration with the specified <paramref name="apiKey"/>
        /// and a 10 second request timeout.
        /// </summary>
        /// <param name="apiKey">
        /// Twingly Search API key. See <see cref="https://developer.twingly.com/resources/search/"/> for more details.
        /// </param>
        public TwinglyConfiguration(string apiKey) : this(apiKey, Constants.DefaultTimeout)
        {

        }

        /// <summary>
        /// Initializes a new configuration with the specified <paramref name="apiKey"/>
        /// and <paramref name="requestTimeoutMilliseconds"/>.
        /// </summary>
        /// <param name="apiKey">
        /// Twingly Search API key. See <see cref="https://developer.twingly.com/resources/search/"/> for more details.
        /// </param>
        /// <param name="requestTimeoutMilliseconds">
        /// The configured request timeout in milliseconds.
        /// </param>
        public TwinglyConfiguration(string apiKey, int requestTimeoutMilliseconds)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey), "Please supply a valid API key.");
            }
            if (requestTimeoutMilliseconds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(requestTimeoutMilliseconds), "Please supply a positive timeout value.");
            }

            ApiKey = apiKey;
            RequestTimeoutMilliseconds = requestTimeoutMilliseconds;
        }

        /// <summary>
        /// Gets the Twingly Search API key.
        /// </summary>
        public virtual string ApiKey { get; }

        /// <summary>
        /// Gets the configured request timeout.
        /// </summary>
        public virtual int RequestTimeoutMilliseconds { get; }
    }
}