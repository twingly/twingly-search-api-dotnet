using System;

namespace Twingly.Search.Client.Exception
{
    [Serializable]
    public class ServerException : RequestException
    {
        public ServerException()
        {
        }

        public ServerException(string message)
            : base(message)
        {
        }

        public ServerException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }
}
