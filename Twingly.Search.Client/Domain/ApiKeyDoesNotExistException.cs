using System;

namespace Twingly.Search.Client.Domain
{
    /// <summary>
    /// Thrown when the API couldn't recognize the supplied API key.
    /// </summary>
    public class ApiKeyDoesNotExistException : TwinglyRequestException
    {
        public ApiKeyDoesNotExistException() : base(message: Constants.ApiKeyDoesNotExist)
        {

        }

        public ApiKeyDoesNotExistException(Exception inner) : base(message: Constants.ApiKeyDoesNotExist, innerException: inner)
        {

        }
    }
}
