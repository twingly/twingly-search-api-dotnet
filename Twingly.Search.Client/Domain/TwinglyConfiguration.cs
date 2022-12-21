﻿using System;

namespace Twingly.Search.Client.Domain
{
    /// <summary>
    /// Allows easy access to the configuration settings.
    /// </summary>
    public class TwinglyConfiguration
    {
        /// <summary>
        /// Initializes a new configuration with the specified <paramref name="apiKey"/>
        /// and the default timeout.
        /// </summary>
        /// <param name="apiKey">
        /// Twingly Search API key. See <see cref="https://app.twingly.com/blog_search?tab=documentation"/> for more details.
        /// </param>
        public TwinglyConfiguration(string apiKey) : this(apiKey, Constants.DefaultTimeoutInMilliseconds)
        {

        }

        /// <summary>
        /// Initializes a new configuration with the specified <paramref name="apiKey"/>
        /// and <paramref name="requestTimeoutMilliseconds"/>.
        /// </summary>
        /// <param name="apiKey">
        /// Twingly Search API key. See <see cref="https://app.twingly.com/blog_search?tab=documentation"/> for more details.
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
