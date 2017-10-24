using System;
using System.Runtime.Serialization;
using Twingly.Search.Client.Domain;

namespace Twingly.Search.Client.Exception
{
    /// <summary>
    /// Occurs when an API key is not found neither in appSettings/TWINGLY_SEARCH_KEY
    /// nor in the 'TWINGLY_SEARCH_KEY ' environment variable.
    /// </summary>
    [Serializable]
    public class ApiKeyNotConfiguredException : System.Exception
    {
        private static readonly string ErrorMessage =
            "Couldn't find an API key. " +
            "Please place a valid API key in the appSettings section of the configuration file " +
            "OR create an environment variable, " + $"using '{Constants.ApiConfigSettingName}' as the key.";

        public ApiKeyNotConfiguredException() : base(ErrorMessage) { }

        public ApiKeyNotConfiguredException(string message) : base(message) { }

        public ApiKeyNotConfiguredException(string message, System.Exception innerException) : base(message, innerException) { }

        public ApiKeyNotConfiguredException(System.Exception innerException) : base(ErrorMessage, innerException) { }

        protected ApiKeyNotConfiguredException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
