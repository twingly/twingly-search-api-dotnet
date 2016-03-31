using System.Configuration;
using Configuration = Twingly.Search.Client.Domain.Configuration;

namespace Twingly.Search.Client.Infrastructure
{
    /// <summary>
    /// Allows easy access to the configuration settings
    /// stored in the config file.
    /// </summary>
    internal class FileConfiguration : Configuration
    {
       public FileConfiguration() : base() { }

        protected override string ReadConfigValue(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }
    }
}
