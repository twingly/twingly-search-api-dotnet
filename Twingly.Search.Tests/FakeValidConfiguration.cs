using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Twingly.Search.Client;
using Twingly.Search.Client.Domain;

namespace Twingly.Search.Tests
{
    internal class FakeValidConfiguration : Configuration
    {
        protected override string ReadConfigValue(string key)
        {
            if(key.Equals(Constants.ApiConfigSettingName))
                return "S0M3rand0mk3y";
            if (key.Equals(Constants.TimeoutConfigSettingName))
                return "500";
            else
            {
                throw new InvalidOperationException("This key value is unsupported");
            }
        }
    }
}
