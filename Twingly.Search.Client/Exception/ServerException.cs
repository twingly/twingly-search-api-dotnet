namespace Twingly.Search.Client.Exception
{
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
