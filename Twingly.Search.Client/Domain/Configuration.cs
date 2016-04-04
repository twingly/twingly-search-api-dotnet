using System;

namespace Twingly.Search.Client.Domain
{
    /// <summary>
    /// Allows easy access to the configuration settings.
    /// </summary>
    internal abstract class Configuration
    {
        private readonly string apiKey = null;
        private readonly int requestTimeoutMilliseconds = 10000;

        /// <summary>
        /// Gets the Twingly API key from configuration store
        /// </summary>
        public string ApiKey
        {
            get
            {
                return this.apiKey;
            }
        }

        /// <summary>
        /// Gets the configured request timeout
        /// </summary>
        public int RequestTimeoutMilliseconds
        {
            get
            {
                return this.requestTimeoutMilliseconds;
            }
        }

        public Configuration()
        {
            this.apiKey = ReadApiKeyFromConfig();
            this.requestTimeoutMilliseconds = 
                ReadRequestTimeoutFromConfig() ?? requestTimeoutMilliseconds;
        }

        private string ReadApiKeyFromConfig()
        {
            string returnValue = null;

            try
            {
                returnValue = ReadConfigValue(Constants.ApiConfigSettingName);
            }

            catch (Exception ex)
            {
                throw new ApiKeyNotConfiguredException(ex);
            }

            if (returnValue == null)
                throw new ApiKeyNotConfiguredException();

            return returnValue;
        }

        private int? ReadRequestTimeoutFromConfig()
        {
            int? returnValue = null;

            try
            {
                int convertedValue = 0;
                if (Int32.TryParse(ReadConfigValue(Constants.TimeoutConfigSettingName), out convertedValue))
                    returnValue = convertedValue;
            }

            catch (Exception)
            {
                // handle gracefully, we'll use a default value.
            }

            return returnValue;
        }

        protected abstract string ReadConfigValue(string key);

    }
}