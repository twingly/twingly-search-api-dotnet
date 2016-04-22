namespace Twingly.Search.Client.Domain
{
    internal static class Constants
    {
        public static readonly string ApiConfigSettingName = "TWINGLY_SEARCH_KEY";
        public static readonly string TimeoutConfigSettingName = "TWINGLY_TIMEOUT_MS"; 
        public static readonly string ApiBaseAddress = "https://api.twingly.com/analytics/Analytics.ashx";
        public static readonly string ApiDateFormat = "yyyy-MM-dd HH:mm:ss";
        public static readonly string ApiKeyDoesNotExist = "The API key does not exist."; 
        public static readonly string UnauthorizedApiKey = "The API key does not grant access to the Search API.";
        public static readonly string ServiceUnavailable = "Authentication service unavailable.";
        public static readonly string DocumentLanguage = "documentlang";
        public static readonly string StartTime = "ts";
        public static readonly string EndTime = "tsTo"; 
        public static readonly string SearchPattern = "searchpattern";
        public static readonly int DefaultTimeout = 10000; // 10 seconds.
    }
}
