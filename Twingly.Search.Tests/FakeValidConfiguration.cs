using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Twingly.Search.Client;

namespace Twingly.Search.Tests
{
    internal class FakeValidConfiguration : Configuration
    {
        protected override string ReadConfigValue(string key)
        {
            return "";
        }
    }
}
