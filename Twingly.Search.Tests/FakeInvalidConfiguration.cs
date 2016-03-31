using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Twingly.Search.Client;
using Twingly.Search.Client.Domain;

namespace Twingly.Search.Tests
{
    internal class FakeInvalidConfiguration : Configuration
    {
        protected override string ReadConfigValue(string key)
        {
            return null;
        }
    }
}
