using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Twingly.Search.Client;

namespace Twingly.Search.Tests
{
    [TestClass]
    public class TwinglySearchClientTests
    {
        [TestMethod]
        [ExpectedException(typeof(ApiKeyNotFoundException), "Failed to throw an exception when API key is missing from config file.")]
        public void When_NoApiKeyConfigured_Then_ShouldThrow()
        {
            /// Arrange, Act, Assert, all in one line!
            var client = new TwinglySearchClient(new FakeInvalidConfiguration(), new HttpClient());
        }

        [TestMethod]
        public void When_ApiKeyConfigured_Then_ShouldReadSuccessfully()
        {
            /// Arrange, Act, Assert, all in one line!
            var client = new TwinglySearchClient(new FakeValidConfiguration(), new HttpClient());
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void When_SendingCorrectQuery_ShouldGetResultSuccessfully()
        {
            // Arrange 
            var client = new TwinglySearchClient(new FakeValidConfiguration(), new HttpClient());
            var theQuery = QueryBuilder.Create("twingly page-size:5000").Build();

            // Act
            QueryResult response = client.Query(theQuery);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.NumberOfMatchesTotal > 0);
        }
    }
}
