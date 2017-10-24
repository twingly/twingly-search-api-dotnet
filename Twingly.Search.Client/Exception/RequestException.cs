using System;
using System.Runtime.Serialization;

namespace Twingly.Search.Client.Exception
{
    /// <summary>
    /// Occurs when there is a server communication error during a request.
    /// </summary>
    [Serializable]
    public class RequestException : System.Exception
    {
        private static readonly string ErrorMessage = "An error occured while communicating with the API";

        public RequestException() : base(ErrorMessage) { }

        public RequestException(string message) : base(message) { }

        public RequestException(string message, System.Exception innerException) : base(message, innerException) { }

        public RequestException(System.Exception innerException) : base(ErrorMessage, innerException) { }

        protected RequestException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
