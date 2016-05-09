using System;

namespace Twingly.Search.Client.Domain
{
    /// <summary>
    /// Thrown when the Search API is not available to service the request.
    /// </summary>
    public class TwinglyServiceUnavailableException : TwinglyRequestException
    {
        public TwinglyServiceUnavailableException() : base(message: Constants.ServiceUnavailable)
        {

        }

        public TwinglyServiceUnavailableException(Exception inner) : base(message: Constants.ServiceUnavailable, innerException: inner)
        {

        }
    }
}
