using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Twingly.Search.Client
{
    /// <summary>
    /// Occurs when an API key is not found in appSettings/TWINGLY_API_KEY
    /// </summary>
    public class ApiKeyNotConfiguredException : Exception
    {
        private static readonly string errorMessage =
            String.Format("Oops, couldn't find an API key in the application configuration file."
                            + "Please place a valid API key in the appSettings section, "
                            + "using '{0}' as the setting name.", Constants.ApiConfigSettingName);

        public ApiKeyNotConfiguredException(): base(errorMessage) { }

        public ApiKeyNotConfiguredException(string message): base(message) { }

        public ApiKeyNotConfiguredException(string message, Exception innerException): base (message, innerException) { }

        public ApiKeyNotConfiguredException(Exception innerException) : base(errorMessage, innerException) { }

        protected ApiKeyNotConfiguredException(SerializationInfo info,
           StreamingContext context) : base(info, context) { }

    }
}
