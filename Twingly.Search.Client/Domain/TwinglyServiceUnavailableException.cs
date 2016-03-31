using System;

namespace Twingly.Search.Client.Domain
{
    public class TwinglyServiceUnavailableException : TwinglyRequestException
    {

        public TwinglyServiceUnavailableException() : base(message: Constants.ServiceUnavailable)
        {

        }

        public TwinglyServiceUnavailableException(Exception inner) : base(message: Constants.UnauthorizedApiKey, innerException: inner)
        {

        }

    }
}
