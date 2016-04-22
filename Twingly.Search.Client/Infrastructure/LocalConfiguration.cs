using System;
using System.Configuration;
using Twingly.Search.Client.Domain;

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
        public LocalConfiguration():base(ReadApiKeyFromConfig(), ReadRequestTimeoutFromConfig())
        {
            
        }

        private static string ReadApiKeyFromConfig()
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

            if (String.IsNullOrWhiteSpace(returnValue))
                throw new ApiKeyNotConfiguredException();

            return returnValue;
        }

        private static int ReadRequestTimeoutFromConfig()
        {
            int? returnValue = null;

            try
            {
                int convertedValue = 0;
                string timeoutValue = ReadConfigValue(Constants.TimeoutConfigSettingName);
                returnValue = Int32.TryParse(timeoutValue, out convertedValue) 
                    ? convertedValue 
                    : Constants.DefaultTimeout;
            }

            catch (Exception)
            {
                // handle gracefully, we'll use a default value.
                returnValue = Constants.DefaultTimeout;
            }

            return returnValue.Value;
        }

        private static string ReadConfigValue(string key)
        {
            string returnValue =
                ConfigurationManager.AppSettings.Get(key);
            if (String.IsNullOrWhiteSpace(returnValue))
                returnValue = Environment.GetEnvironmentVariable(key);

            return returnValue;
        }

    }
}
