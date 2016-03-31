using System;

namespace Twingly.Search.Client.Domain
{
    public class ApiKeyDoesNotExistException : TwinglyRequestException
    {
        public ApiKeyDoesNotExistException() : base(message: Constants.ApiKeyDoesNotExist)
        {

        }

        public ApiKeyDoesNotExistException(Exception inner) : base(message: Constants.UnauthorizedApiKey, innerException: inner)
        {

        }
    }
}
