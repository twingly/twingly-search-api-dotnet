namespace Twingly.Search.Client.Exception
{
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
