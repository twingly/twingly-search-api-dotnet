using System;
using System.Runtime.Serialization;

namespace Twingly.Search.Client.Domain
{
    /// <summary>
    /// Occurs when an API key is not found neither in appSettings/TWINGLY_SEARCH_KEY  
    /// nor in the 'TWINGLY_SEARCH_KEY ' environment variable.
    /// </summary>
    public class ApiKeyNotConfiguredException : Exception
    {
        private static readonly string errorMessage =
            String.Format("Oops, couldn't find an API key. "
                            + "Please place a valid API key in the appSettings section of the configuration file "
                            + "OR create an environment variable, "
                            + "using '{0}' as the key.", Constants.ApiConfigSettingName);

        public ApiKeyNotConfiguredException(): base(errorMessage) { }

        public ApiKeyNotConfiguredException(string message): base(message) { }

        public ApiKeyNotConfiguredException(string message, Exception innerException): base (message, innerException) { }

        public ApiKeyNotConfiguredException(Exception innerException) : base(errorMessage, innerException) { }

        protected ApiKeyNotConfiguredException(SerializationInfo info,
           StreamingContext context) : base(info, context) { }

    }
}
