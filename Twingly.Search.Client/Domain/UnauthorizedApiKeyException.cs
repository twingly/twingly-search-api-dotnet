using System;

namespace Twingly.Search.Client.Domain
{
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
