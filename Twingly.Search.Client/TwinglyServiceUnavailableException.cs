using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twingly.Search.Client
{
    public class TwinglyServiceUnavailableException : TwinglyRequestException
    {

        public TwinglyServiceUnavailableException() : base(message: Constants.ServiceUnavailable)
        {

        }

    }
}
