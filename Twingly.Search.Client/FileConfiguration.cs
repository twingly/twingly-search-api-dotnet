using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twingly.Search.Client
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
