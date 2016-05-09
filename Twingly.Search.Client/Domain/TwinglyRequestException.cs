using System;
using System.Runtime.Serialization;

namespace Twingly.Search.Client.Domain
{
    /// <summary>
    /// Occurs when there is a server communication error during a request.
    /// </summary>
    public class TwinglyRequestException : Exception
    {
        private static readonly string errorMessage =
            String.Format("An error occured while communicating with the API");

        public TwinglyRequestException(): base(errorMessage) { }

        public TwinglyRequestException(string message): base(message) { }

        public TwinglyRequestException(string message, Exception innerException): base (message, innerException) { }

        public TwinglyRequestException(Exception innerException) : base(errorMessage, innerException) { }

        protected TwinglyRequestException(SerializationInfo info,
           StreamingContext context) : base(info, context) { }

    }
}
