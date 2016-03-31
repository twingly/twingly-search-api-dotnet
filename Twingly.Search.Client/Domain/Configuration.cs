using System;

namespace Twingly.Search.Client.Domain
{
    /// <summary>
    /// Allows easy access to the configuration settings.
    /// </summary>
    internal abstract class Configuration
    {
        private readonly string apiKey = null;

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

        public Configuration()
        {
            this.apiKey = ReadApiKeyFromConfig();
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

        protected abstract string ReadConfigValue(string key);

    }
}