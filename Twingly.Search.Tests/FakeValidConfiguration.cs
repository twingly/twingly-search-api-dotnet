using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Twingly.Search.Client;
using Twingly.Search.Client.Domain;

namespace Twingly.Search.Tests
{
    internal class FakeValidConfiguration : TwinglyConfiguration
    {
        public FakeValidConfiguration() : base("som3r@ndomK3y", 700)
        {
            
        }
    }
}
