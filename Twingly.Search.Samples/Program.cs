using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twingly.Search.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            FindPostsSinceYesterdayExample.RunExample();
            AdvancedQueryLanguageExample.RunExample();
            PageOverBlogPostsExample.RunExample();

            Console.WriteLine("Press any key to continue...");
            Console.Read();
        }
    }
}
