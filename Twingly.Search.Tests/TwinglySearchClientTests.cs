using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Twingly.Search.Client;
using Twingly.Search.Client.Domain;
using Twingly.Search.Client.Infrastructure;

namespace Twingly.Search.Tests
{
    [TestClass]
    [DeploymentItem(@"TestData")]
    public class TwinglySearchClientTests
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
            int expectedPostCount = 3;
            double expectedSecondsElapsed = 0.148;
            int expectedNumberOfMatchesTotal = 3;
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("minimal_valid_result.xml", request => requestMessage = request);
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();

            // Act
            QueryResult result = client.Query(validQuery);

            // Assert
            Assert.AreEqual(expectedPostCount, result.Posts.Count);
            Assert.AreEqual(expectedPostCount, result.NumberOfMatchesReturned);
            Assert.AreEqual(expectedSecondsElapsed, result.SecondsElapsed);
            Assert.AreEqual(expectedNumberOfMatchesTotal, result.NumberOfMatchesTotal);
            Assert.AreEqual(result.Posts[0].Url, "http://oppogner.blogg.no/1409602010_bare_m_ha.html");
            Assert.AreEqual(result.Posts[0].BlogName, "oppogner");
            Assert.AreEqual(result.Posts[0].BlogUrl, "http://oppogner.blogg.no/");
            Assert.AreEqual(result.Posts[0].Title, "Bare MÅ ha!");
            Assert.AreEqual(result.Posts[0].Summary, "Ja, velkommen til høsten...");
            Assert.AreEqual(result.Posts[0].LanguageCode, "no");
            Assert.AreEqual(result.Posts[0].Published, DateTime.Parse("2014-09-02 06:53:26Z", null, DateTimeStyles.AdjustToUniversal));
            Assert.AreEqual(result.Posts[0].Indexed, DateTime.Parse("2014-09-02 09:00:53Z", null, DateTimeStyles.AdjustToUniversal));
            Assert.AreEqual(result.Posts[0].Authority, 1);
            Assert.AreEqual(result.Posts[0].BlogRank, 1);
            Assert.AreEqual(result.Posts[0].Tags.Count, 1);
            Assert.AreEqual(result.Posts[0].Tags.First(), "Blogg");

            Assert.AreEqual(result.Posts[1].Url, "http://www.skvallernytt.se/hardtraning-da-galler-swedish-house-mafia");
            Assert.AreEqual(result.Posts[1].BlogName, "Skvallernytt.se");
            Assert.AreEqual(result.Posts[1].BlogUrl, "http://www.skvallernytt.se/");
            Assert.AreEqual(result.Posts[1].Title, "Hårdträning – då gäller Swedish House Mafia");
            Assert.AreEqual(result.Posts[1].Summary, "Träning.Och Swedish House Mafia.Det verkar vara ett lyckat koncept." +
                                                    " \"Don't you worry child\" och \"Greyhound\" är nämligen de två mest spelade" +
                                                    " träningslåtarna under januari 2013 på Spotify.\n\nRelaterade inlägg:\nSwedish House Mafia" +
                                                    " – ny låt!\nNy knivattack på Swedish House Mafia-konsert\nSwedish House Mafia gör succé i USA");
            Assert.AreEqual(result.Posts[1].LanguageCode, "sv");
            Assert.AreEqual(result.Posts[1].Published, DateTime.Parse("2013-01-29 15:21:56Z", null, DateTimeStyles.AdjustToUniversal));
            Assert.AreEqual(result.Posts[1].Indexed, DateTime.Parse("2013-01-29 15:22:52Z", null, DateTimeStyles.AdjustToUniversal));
            Assert.AreEqual(result.Posts[1].Authority, 38);
            Assert.AreEqual(result.Posts[1].BlogRank, 4);
            Assert.AreEqual(result.Posts[1].Tags.Count, 5);

            Assert.IsTrue(result.Posts[1].Tags.All(tag => new List<string>()
            {
                "Okategoriserat",
                "Träning",
                "greyhound",
                "koncept",
                "mafia"
            }.Contains(tag)));

            Assert.AreEqual(result.Posts[2].Url, "http://didriksinspesielleverden.blogg.no/1359472349_justin_bieber.html");
            Assert.AreEqual(result.Posts[2].BlogName, "Didriksinspesielleverden");
            Assert.AreEqual(result.Posts[2].BlogUrl, "http://didriksinspesielleverden.blogg.no/");
            Assert.AreEqual(result.Posts[2].Title, "Justin Bieber");
            Assert.AreEqual(result.Posts[2].Summary, "OMG! Justin Bieber Believe acoustic albumet" +
                                                     " er nå ute på spotify.Han er helt super.Love him." +
                                                     "Personlig liker jeg best beauty and a beat og as long" +
                                                     " as you love me, kommenter gjerne hva dere synes! <3 #sus YOLO");
            Assert.AreEqual(result.Posts[2].LanguageCode, "no");
            Assert.AreEqual(result.Posts[2].Published, DateTime.Parse("2013-01-29 15:12:29Z", null, DateTimeStyles.AdjustToUniversal));
            Assert.AreEqual(result.Posts[2].Indexed, DateTime.Parse("2013-01-29 15:14:37Z", null, DateTimeStyles.AdjustToUniversal));
            Assert.AreEqual(result.Posts[2].Authority, 0);
            Assert.AreEqual(result.Posts[2].BlogRank, 1);
            Assert.AreEqual(result.Posts[2].Tags.Count, 0);
        }

        [TestMethod]
        public void When_ANonBlogRequestIsEncountered_Then_ShouldHandleGracefully()
        {
            // Arrange 
            HttpRequestMessage requestMessage = null;
            int expectedPostCount = 2;
            double expectedSecondsElapsed = 0.022;
            int expectedNumberOfMatchesTotal = 2;
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("valid_non_blog_result.xml", request => requestMessage = request);
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();

            // Act
            QueryResult result = client.Query(validQuery);

            // Assert
            Assert.AreEqual(expectedPostCount, result.NumberOfMatchesReturned);
            Assert.AreEqual(expectedNumberOfMatchesTotal, result.NumberOfMatchesTotal);
            Assert.AreEqual(expectedSecondsElapsed, result.SecondsElapsed);
            Assert.AreEqual(result.Posts.Count, 1);
            Assert.AreEqual(result.Posts[0].Url, "http://www.someotherurl.com/post");
            Assert.AreEqual(result.Posts[0].BlogName, "Blog Name");
            Assert.AreEqual(result.Posts[0].BlogUrl, "http://www.someotherurl.com/");
            Assert.IsFalse(result.Posts[0].Tags.Any());
            Assert.AreEqual(result.Posts[0].Summary, "Summary");
            Assert.AreEqual(result.Posts[0].LanguageCode, "sv");
            Assert.AreEqual(result.Posts[0].Published, DateTime.Parse("2013-01-29 15:26:33Z", null, DateTimeStyles.AdjustToUniversal));
            Assert.AreEqual(result.Posts[0].Indexed, DateTime.Parse("2013-01-29 15:27:07Z", null, DateTimeStyles.AdjustToUniversal));
            Assert.AreEqual(result.Posts[0].Authority, 0);
            Assert.AreEqual(result.Posts[0].BlogRank, 1);
        }

        [TestMethod]
        public void When_UserAgentIsSet_Then_ShouldSerializeToRequestHeader()
        {
            // Arrange 
            HttpRequestMessage requestMessage = null;
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("SuccessfulApiResponse.5posts.xml", request => requestMessage = request);
            string expectedUserAgent = "Hey, I'm a .NET client!/.NET v." + typeof(TwinglySearchClient).Assembly.GetName().Version;
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
            string expectedUserAgent = "Twingly Search API Client/.NET v." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
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
        [ExpectedException(typeof(TwinglyRequestException),
           "Failed to recognize and throw the proper error when server returned an unknown error response.")]
        public void When_ReceivedAnUnknownErrorResult_ShouldThrow()
        {
            // Arrange
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("undefined_error_result.xml", request => { });
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
        [ExpectedException(typeof(ApiKeyDoesNotExistException),
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
