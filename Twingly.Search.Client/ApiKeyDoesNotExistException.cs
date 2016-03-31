using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twingly.Search.Client
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
