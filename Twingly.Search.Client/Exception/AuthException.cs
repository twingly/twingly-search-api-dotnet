using System;

namespace Twingly.Search.Client.Exception
{
    [Serializable]
    public class AuthException : RequestException
    {
        public AuthException()
        {
        }

        public AuthException(string message)
            : base(message)
        {
        }

        public AuthException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }
}
