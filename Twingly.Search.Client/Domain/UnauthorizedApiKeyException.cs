using System;

namespace Twingly.Search.Client.Domain
{
    /// <summary>
    /// Thrown when the supplied API key cannot be used to service the request.
    /// </summary>
    public class UnauthorizedApiKeyException : TwinglyRequestException
    {
        public UnauthorizedApiKeyException() : base(message: Constants.UnauthorizedApiKey)
        {

        }

        public UnauthorizedApiKeyException(Exception inner) : base(message: Constants.UnauthorizedApiKey, innerException: inner)
        {

        }
    }
}
