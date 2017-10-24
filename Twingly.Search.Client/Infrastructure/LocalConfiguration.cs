using System;
using System.Configuration;
using Twingly.Search.Client.Domain;
using Twingly.Search.Client.Exception;

namespace Twingly.Search.Client.Infrastructure
{
    /// <summary>
    /// Allows easy access to the configuration settings
    /// stored in the application config file or environment variables.
    /// </summary>
    public class LocalConfiguration : TwinglyConfiguration
    {
        /// <summary>
        /// Initializes a new configuration with the settings
        /// configured in the application config file (primary source) or in environment variables (fallback source).
        /// </summary>
        public LocalConfiguration() : base(ReadApiKeyFromConfig(), ReadRequestTimeoutFromConfig())
        {

        }

        private static string ReadApiKeyFromConfig()
        {
            string returnValue;

            try
            {
                returnValue = ReadConfigValue(Constants.ApiConfigSettingName);
            }

            catch (System.Exception ex)
            {
                throw new ApiKeyNotConfiguredException(ex);
            }

            if (string.IsNullOrWhiteSpace(returnValue))
            {
                throw new ApiKeyNotConfiguredException();
            }

            return returnValue;
        }

        private static int ReadRequestTimeoutFromConfig()
        {
            int? returnValue;

            try
            {
                string timeoutValue = ReadConfigValue(Constants.TimeoutConfigSettingName);
                returnValue = int.TryParse(timeoutValue, out var convertedValue)
                    ? convertedValue
                    : Constants.DefaultTimeoutInMilliseconds;
            }

            catch (System.Exception)
            {
                // handle gracefully, we'll use a default value.
                returnValue = Constants.DefaultTimeoutInMilliseconds;
            }

            return returnValue.Value;
        }

        private static string ReadConfigValue(string key)
        {
            string returnValue = ConfigurationManager.AppSettings.Get(key);
            if (string.IsNullOrWhiteSpace(returnValue))
            {
                returnValue = Environment.GetEnvironmentVariable(key);
            }

            return returnValue;
        }
    }
}
