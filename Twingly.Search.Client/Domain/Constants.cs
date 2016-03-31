namespace Twingly.Search.Client.Domain
{
    internal static class Constants
    {
        public static readonly string ApiConfigSettingName = "TWINGLY_API_KEY";
        public static readonly string ApiBaseAddress = "https://api.twingly.com/analytics/Analytics.ashx";
        public static readonly string ApiDateFormat = "YYYY-MM-dd HH:mm:ss";
        public static readonly string ApiKeyDoesNotExist = "The API key does not exist.";
        public static readonly string UnauthorizedApiKey = "The API key does not grant access to the Search API.";
        public static readonly string ServiceUnavailable = "Authentication service unavailable.";
    }
}
