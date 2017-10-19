using System;

namespace Twingly.Search.Client.Exception
{
    [Serializable]
    public class QueryException : RequestException
    {
        public QueryException()
        {
        }

        public QueryException(string message)
            : base(message)
        {
        }

        public QueryException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }
}
