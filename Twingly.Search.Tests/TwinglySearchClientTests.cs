﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Twingly.Search.Client;
using Twingly.Search.Client.Domain;
using Twingly.Search.Client.Infrastructure;

namespace Twingly.Search.Tests
{
    [TestClass]
    [DeploymentItem(@"TestData")]
    public class TwinglySearchClientUnitTests
    {

        [TestMethod]
        public void When_ApiKeyConfigured_Then_ShouldReadSuccessfully()
        {
            // Arrange, Act, Assert, all in one line!
            var client = new TwinglySearchClient(new FakeValidConfiguration(), new HttpClient());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
            "Failed to throw an exception when a null Query was supplied")]
        public void When_QueryIsNull_Then_ShouldThrow()
        {
            // Arrange
            var client = new TwinglySearchClient(new FakeValidConfiguration(), new HttpClient());

            // Act & Assert
            client.Query(null);
        }

        [TestMethod]
        public void When_SearchPatternSet_Then_ShouldSendRequest()
        {
            // Arrange 
            bool isServiceCalled = false;
            TwinglySearchClient client = 
                SetupTwinglyClientWithResponseFile("SuccessfulApiResponse.5posts.xml", request => isServiceCalled = true);
            Query validQuery = QueryBuilder.Create("A valid query").Build();

            // Act
            client.Query(validQuery);

            // Assert
            Assert.IsTrue(isServiceCalled);
        }

        [TestMethod]
        public void When_AllArgumentsSetCorrectly_Then_ShouldSerializeToRequestString()
        {
            // Arrange 
            HttpRequestMessage requestMessage = null;
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("SuccessfulApiResponse.5posts.xml", request => requestMessage = request);
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .StartTime(DateTime.UtcNow)
                                .EndTime(DateTime.UtcNow.AddDays(1))
                                .Language(Language.Russian)
                                .Build();

            // Act
            client.Query(validQuery);
            NameValueCollection serializedParameters = System.Web.HttpUtility.ParseQueryString(requestMessage.RequestUri.Query);

            // Assert
            Assert.AreEqual(validQuery.SearchPattern, serializedParameters[Constants.SearchPattern]);
            Assert.AreEqual(validQuery.StartTime.Value.ToString(Constants.ApiDateFormat), serializedParameters[Constants.StartTime]);
            Assert.AreEqual(validQuery.EndTime.Value.ToString(Constants.ApiDateFormat), serializedParameters[Constants.EndTime]);
            Assert.AreEqual(validQuery.Language, serializedParameters[Constants.DocumentLanguage]);
            Assert.IsNotNull(client.UserAgent);
            Assert.AreEqual(client.UserAgent, requestMessage.Headers.UserAgent.ToString());
        }

        [TestMethod]
        public void When_ResponseIsSuccessful_Then_ShouldDeserialize()
        {
            // Arrange 
            HttpRequestMessage requestMessage = null;
            int expectedPostCount = 5;
            double expectedSecondsElapsed = 0.236;
            int expectedNumberOfMatchesTotal = 190596;
            string expectedLanguageCode = "en";
            DateTime expectedPublished = DateTime.Parse("2016-04-04 12:39:09Z", null, DateTimeStyles.AdjustToUniversal);
            DateTime expectedIndexed = DateTime.Parse("2016-04-04 12:42:23Z",null, DateTimeStyles.AdjustToUniversal);
            int expectedTagCount = 2;
            int expectedRank = 1;
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("SuccessfulApiResponse.5posts.xml", request => requestMessage = request);
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();

            // Act
            QueryResult result = client.Query(validQuery);
            Post secondPost = result.Posts.ElementAt(1);

            // Assert
            Assert.AreEqual(expectedPostCount, result.Posts.Count);
            Assert.AreEqual(expectedSecondsElapsed, result.SecondsElapsed);
            Assert.AreEqual(expectedNumberOfMatchesTotal, result.NumberOfMatchesTotal);
            Assert.AreEqual(expectedLanguageCode, secondPost.LanguageCode);
            Assert.AreEqual(expectedPublished, secondPost.Published);
            Assert.AreEqual(expectedIndexed, secondPost.Indexed);
            Assert.AreEqual(expectedRank, secondPost.BlogRank);
            Assert.AreEqual(expectedRank, secondPost.Authority);
            Assert.AreEqual(expectedTagCount, secondPost.Tags.Count);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(secondPost.Url));
            Assert.IsTrue(!String.IsNullOrWhiteSpace(secondPost.Title));
            Assert.IsTrue(!String.IsNullOrWhiteSpace(secondPost.Summary));
            Assert.IsTrue(!String.IsNullOrWhiteSpace(secondPost.BlogUrl));
            Assert.IsTrue(!String.IsNullOrWhiteSpace(secondPost.BlogName));
        }

        [TestMethod]
        public void When_UserAgentIsSet_Then_ShouldSerializeToRequestHeader()
        {
            // Arrange 
            HttpRequestMessage requestMessage = null;
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("SuccessfulApiResponse.5posts.xml", request => requestMessage = request);
            string expectedUserAgent = "Hey, I'm a .NET client!/.NET v." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();

            // Act
            client.UserAgent = "Hey, I'm a .NET client!";
            client.Query(validQuery);

            // Assert
            Assert.AreEqual(expectedUserAgent, requestMessage.Headers.UserAgent.ToString());
        }

        [TestMethod]
        public void When_UserAgentNotSet_Then_ShouldSerializeDefaultToRequestHeader()
        {
            // Arrange 
            HttpRequestMessage requestMessage = null;
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("SuccessfulApiResponse.5posts.xml", request => requestMessage = request);
            string expectedUserAgent = "Twingly API Client/.NET v." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();

            // Act
            client.Query(validQuery);

            // Assert
            Assert.AreEqual(expectedUserAgent, requestMessage.Headers.UserAgent.ToString());
        }


        [TestMethod]
        [ExpectedException(typeof(ApiKeyDoesNotExistException),
            "Failed to throw a proper exception when an API key is not recognized by the server.")]
        public void When_ApiKeyDoesNotExist_Then_ShouldSaySo()
        {
            // Arrange
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("ApiKeyDoesNotExistResponse.xml", request => { });
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();
            // Act & Assert
            client.Query(validQuery);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedApiKeyException),
           "Failed to throw a proper exception when an API key is not authorized for this request.")]
        public void When_ApiKeyNotAuthorized_Then_ShouldSaySo()
        {
            // Arrange
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("UnauthorizedApiKeyResponse.xml", request => { });
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();
            // Act & Assert
            client.Query(validQuery);
        }

        [TestMethod]
        [ExpectedException(typeof(TwinglyServiceUnavailableException),
          "Failed to throw a proper exception when the service is unavailable. ")]
        public void When_ServiceUnavailable_Then_ShouldSaySo()
        {
            // Arrange
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("ServiceUnavailableResponse.xml", request => { });
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();
            // Act & Assert
            client.Query(validQuery);
        }

        [TestMethod]
        [ExpectedException(typeof(TwinglyRequestException),
         "Failed to throw a proper exception when the request has timed out ")]
        public void When_RequestTimesOut_Then_ShouldThrow()
        {
            // Arrange
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("ServiceUnavailableResponse.xml", request => { Thread.Sleep(750); });
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();

            // Act & Assert
            client.Query(validQuery);
        }

        [TestMethod]
        [ExpectedException(typeof(TwinglyRequestException),
        "Failed to throw a proper exception when the request has timed out ")]
        public void When_RequestFormatNotRecognized_Then_ShouldThrow()
        {
            // Arrange
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("UnknownFormatResponse.xml", request => { });
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();
            // Act & Assert
            client.Query(validQuery);
        }

        private static TwinglySearchClient SetupTwinglyClientWithResponseFile(string fileName, Action<HttpRequestMessage> delegateAction)
        {
            string responseContents = File.ReadAllText(fileName);
            var response = DelegatingHttpClientHandler.GetStreamHttpResponseMessage(responseContents);
            var messageHandler = new DelegatingHttpClientHandler(delegateAction, response);

            return new TwinglySearchClient(new FakeValidConfiguration(), new HttpClient(messageHandler));
        }
    }


    [TestClass]
    public class TwinglySearchClientIntegrationTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void When_SendingCorrectQuery_ShouldGetResultSuccessfully()
        {
            // Arrange 
            var client = new TwinglySearchClient(new LocalConfiguration(), new HttpClient());
            var theQuery = QueryBuilder.Create("audi page-size:100").Build();

            // Act
            QueryResult response = client.Query(theQuery);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.NumberOfMatchesTotal > 0);
        }

        [TestMethod]
        [TestCategory("Integration")]
        [ExpectedException(typeof (ApiKeyDoesNotExistException),
            "Failed to recognize and throw an error when using a non-existing API key.")]
        public void When_UsingInvalidKey_ShouldThrow()
        {
            // Arrange 
            var client = new TwinglySearchClient(new FakeValidConfiguration(), new HttpClient()); 
            var theQuery = QueryBuilder.Create("twingly page-size:100").Build();

            // Act, Assert (should throw).
            QueryResult response = client.Query(theQuery);
        }
    }
}
