namespace Twingly.Search.Client.Domain
{
    internal static class Constants
    {
        public static readonly string ApiConfigSettingName = "TWINGLY_SEARCH_KEY";
        public static readonly string TimeoutConfigSettingName = "TWINGLY_TIMEOUT_MS";
        public static readonly string ApiBaseAddress = "https://api.twingly.com/blog/search/api/v3/search";
        public static readonly string ApiDateFormat = "yyyy-MM-ddTHH:mm:ss";
        public static readonly string DocumentLanguage = "lang";
        public static readonly string StartTime = "start-date";
        public static readonly string EndTime = "end-date";
        public static readonly string SearchQuery = "q";
        public static readonly int DefaultTimeoutInMilliseconds = 10000;
    }
}
