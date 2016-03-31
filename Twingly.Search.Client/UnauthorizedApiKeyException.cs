using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twingly.Search.Client
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
