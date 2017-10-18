using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using Twingly.Search.Client;
using Twingly.Search.Client.Domain;
using Twingly.Search.Client.Exception;
using Twingly.Search.Client.Infrastructure;

namespace Twingly.Search.Tests
{
    [TestFixture]
    public class TwinglySearchClientTests
    {
        [Test]
        public void When_ApiKeyConfigured_Then_ShouldReadSuccessfully()
        {
            // Arrange, Act, Assert, all in one line!
            Assert.DoesNotThrow(() => new TwinglySearchClient(new FakeValidConfiguration(), new HttpClient()));
        }

        [Test]
        public void When_QueryIsNull_Then_ShouldThrow()
        {
            // Arrange
            var client = new TwinglySearchClient(new FakeValidConfiguration(), new HttpClient());

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => { client.Query(null); },
                "Failed to throw an exception when a null Query was supplied"
            );
        }

        [Test]
        public void When_SearchQuerySet_Then_ShouldSendRequest()
        {
            // Arrange
            bool isServiceCalled = false;
            TwinglySearchClient client =
                SetupTwinglyClientWithResponseFile("MinimalValidResponse.xml", request => isServiceCalled = true);
            Query validQuery = QueryBuilder.Create("A valid query").Build();

            // Act
            client.Query(validQuery);

            // Assert
            Assert.IsTrue(isServiceCalled);
        }

        [Test]
        public void When_AllArgumentsSetCorrectly_Then_ShouldSerializeToRequestString()
        {
            // Arrange
            HttpRequestMessage requestMessage = null;
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("MinimalValidResponse.xml", request => requestMessage = request);
            Query validQuery = QueryBuilder
                                .Create("A valid query lang:ru")
                                .StartTime(DateTime.UtcNow)
                                .EndTime(DateTime.UtcNow.AddDays(1))
                                .Build();

            // Act
            client.Query(validQuery);
            NameValueCollection serializedParameters = HttpUtility.ParseQueryString(requestMessage.RequestUri.Query);

            // Assert
            var expectedStartTime = validQuery.StartTime.Value.ToString(Constants.ApiDateFormat);
            var expectedEndTime = validQuery.EndTime.Value.ToString(Constants.ApiDateFormat);
            var expectedQuery = $"{validQuery.SearchQuery} start-date:{expectedStartTime} end-date:{expectedEndTime}";

            Assert.AreEqual(expectedQuery, serializedParameters[Constants.SearchQuery]);
            Assert.IsNotNull(client.UserAgent);
            Assert.AreEqual(client.UserAgent, requestMessage.Headers.UserAgent.ToString());
        }

        [Test]
        public void When_ResponseIsSuccessful_Then_ShouldDeserialize()
        {
            // Arrange
            HttpRequestMessage requestMessage;
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("MinimalValidResponse.xml", request => requestMessage = request);
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();

            // Act
            QueryResult result = client.Query(validQuery);

            // Assert
            var firstPost = result.Posts.First();

            Assert.AreEqual(3, result.Posts.Count);
            Assert.AreEqual(3, result.NumberOfMatchesReturned);
            Assert.AreEqual(0.369, result.SecondsElapsed);
            Assert.AreEqual(3122050, result.NumberOfMatchesTotal);
            Assert.AreEqual("16405819479794412880", firstPost.Id);
            Assert.AreEqual("klivinihemligheten", firstPost.Author);
            Assert.AreEqual("http://nouw.com/klivinihemligheten/planering---men-dalig-30016048", firstPost.Url);
            Assert.AreEqual("Planering - men dålig", firstPost.Title);
            Assert.AreEqual("Det vart en förmiddag på boxen med en brud som jag lärt känna där. Körde en egen WOD, bland annat SDHP, shoulder press, HSPU - bland annat. Hade planerat dagen in i minsta detalj, insåg under passet att jag glömt leggings. Så - det var bara att sluta lite tidigare för att röra sig hemåt för dusch och lunch. Har alltså släpat med mig ryggsäcken med allt för dagen i onödan. Riktigt riktigt klantigt! Har nu en timma på mig att duscha och göra mig ordning för föreläsning, innan det är dags att dra igen. Och jag som skulle plugga innan", firstPost.Text);
            Assert.AreEqual("sv", firstPost.LanguageCode);
            Assert.AreEqual("se", firstPost.LocationCode);
            Assert.AreEqual(null, firstPost.Coordinates.Longitude);
            Assert.AreEqual(null, firstPost.Coordinates.Latitude);
            Assert.AreEqual(0, firstPost.Links.Count);
            CollectionAssert.AreEqual(new [] { "Ätas & drickas", "Universitet & studentlivet", "Träning", "To to list"}, firstPost.Tags);
            Assert.AreEqual(firstPost.Images.Count, 0);
            Assert.AreEqual(DateTime.Parse("2017-05-04T06:51:23Z"), firstPost.IndexedAt);
            Assert.AreEqual(DateTime.Parse("2017-05-04T06:50:59Z"), firstPost.PublishedAt);
            Assert.AreEqual(DateTime.Parse("2017-05-04T08:51:23Z"), firstPost.ReindexedAt);
            Assert.AreEqual(0, firstPost.InLinksCount);
            Assert.AreEqual("5312283800049632348", firstPost.BlogId);
            Assert.AreEqual("Love life like a student", firstPost.BlogName);
            Assert.AreEqual("http://nouw.com/klivinihemligheten", firstPost.BlogUrl);
            Assert.AreEqual(1, firstPost.BlogRank);
            Assert.AreEqual(0, firstPost.Authority);
        }

        [Test]
        public void When_ResultWasIncomplete_Then_ShouldSetIncompleteCorrectly()
        {
            // Arrange
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("IncompleteResponse.xml", request => { });
            Query validQuery = QueryBuilder
                .Create("A valid query")
                .Build();

            // Act
            QueryResult result = client.Query(validQuery);

            // Assert
            Assert.AreEqual(true, result.IncompleteResult);
        }

        [Test]
        public void When_ResultHasCoordinates_Then_ShouldSetCoordinates()
        {
            // Arrange
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("ValidCoordinatesResponse.xml", request => { });
            Query validQuery = QueryBuilder
                .Create("A valid query")
                .Build();

            // Act
            QueryResult result = client.Query(validQuery);

            // Assert
            Assert.AreEqual(49.1, result.Posts.First().Coordinates.Latitude);
            Assert.AreEqual(10.75, result.Posts.First().Coordinates.Longitude);
        }

        [Test]
        public void When_ResultHasLinks_Then_ShouldSetLinks()
        {
            // Arrange
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("ValidLinksResponse.xml", request => { });
            Query validQuery = QueryBuilder
                .Create("A valid query")
                .Build();

            // Act
            QueryResult result = client.Query(validQuery);

            // Assert
            var expectedLinks = new[]
            {
                "https://1.bp.blogspot.com/-4uNjjiNQiug/WKguo1sBxwI/AAAAAAAAqKE/_eR7cY8Ft3cd2fYCx-2yXK8AwSHE_A2GgCLcB/s1600/aaea427ee3eaaf8f47d650f48fdf1242.jpg",
                "http://www.irsn.fr/EN/newsroom/News/Pages/20170213_Detection-of-radioactive-iodine-at-trace-levels-in-Europe-in-January-2017.aspx",
                "https://www.t.co/2P4IDmovzH",
                "https://www.twitter.com/Strat2Intel/status/832710701730844672"
            };

            CollectionAssert.AreEqual(expectedLinks, result.Posts.First().Links);
        }

        [Test]
        public void When_ResultIsEmpty_Then_ShouldDeserialize()
        {
            // Arrange
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("ValidEmptyResponse.xml", request => { });
            Query validQuery = QueryBuilder
                .Create("A valid query")
                .Build();

            // Act
            QueryResult result = client.Query(validQuery);

            // Assert
            Assert.AreEqual(0, result.Posts.Count);
            Assert.AreEqual(0, result.NumberOfMatchesReturned);
            Assert.AreEqual(0, result.NumberOfMatchesTotal);
        }

        [Test]
        public void When_UserAgentIsSet_Then_ShouldSerializeToRequestHeader()
        {
            // Arrange
            HttpRequestMessage requestMessage = null;
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("MinimalValidResponse.xml", request => requestMessage = request);
            string expectedUserAgent = "Hey I'm a .NET client!/.NET v." + typeof(TwinglySearchClient).Assembly.GetName().Version;
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();

            // Act
            client.UserAgent = "Hey I'm a .NET client!";
            client.Query(validQuery);

            // Assert
            Assert.AreEqual(expectedUserAgent, requestMessage.Headers.UserAgent.ToString());
        }

        [Test]
        public void When_UserAgentIsSetToInvalidValue_Then_ShouldThrow()
        {
            // Arrange
            HttpRequestMessage requestMessage = null;
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("MinimalValidResponse.xml", request => requestMessage = request);

            // Act
            Assert.Throws<FormatException>(
                () => { client.UserAgent = "User,Agent"; },
                "Failed to throw a proper exception when an invalid user agent was set."
            );
        }

        [Test]
        public void When_UserAgentNotSet_Then_ShouldSerializeDefaultToRequestHeader()
        {
            // Arrange
            HttpRequestMessage requestMessage = null;
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("MinimalValidResponse.xml", request => requestMessage = request);
            string expectedUserAgent = "Twingly Search API Client/.NET v." + typeof(TwinglySearchClient).Assembly.GetName().Version;
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();

            // Act
            client.Query(validQuery);

            // Assert
            Assert.AreEqual(expectedUserAgent, requestMessage.Headers.UserAgent.ToString());
        }

        [Test]
        public void When_ApiKeyDoesNotExist_Then_ShouldSaySo()
        {
            // Arrange
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("NonexistentApiKeyResponse.xml", request => { }, HttpStatusCode.BadRequest);
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();
            // Act & Assert

            Assert.Throws<QueryException>(
                () => { client.Query(validQuery); },
                "Failed to throw a proper exception when an API key is not recognized by the server."
            );
        }

        [Test]
        public void When_ReceivedAnUnknownErrorResult_ShouldThrow()
        {
            // Arrange
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("UndefinedErrorResponse.xml", request => { }, HttpStatusCode.InternalServerError);
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();

            // Act & Assert
            Assert.Throws<ServerException>(
                () => { client.Query(validQuery); },
                "Failed to recognize and throw the proper error when server returned an unknown error response."
            );
        }

        [Test]
        public void When_ApiKeyNotAuthorized_Then_ShouldSaySo()
        {
            // Arrange
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("UnauthorizedApiKeyResponse.xml", request => { }, HttpStatusCode.Unauthorized);
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();
            // Act & Assert
            Assert.Throws<AuthException>(
                () => { client.Query(validQuery); },
                "Failed to throw a proper exception when an API key is not authorized for this request."
            );
        }

        [Test]
        public void When_ServiceUnavailable_Then_ShouldSaySo()
        {
            // Arrange
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("ServiceUnavailableResponse.xml", request => { }, HttpStatusCode.ServiceUnavailable);
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();
            // Act & Assert
            Assert.Throws<ServerException>(
                () => { client.Query(validQuery); },
                "Failed to throw a proper exception when the service is unavailable."
            );
        }

        [Test]
        public void When_RequestTimesOut_Then_ShouldThrow()
        {
            // Arrange
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("ServiceUnavailableResponse.xml", request => { Thread.Sleep(750); });
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();

            // Act & Assert
            Assert.Throws<RequestException>(
                () => { client.Query(validQuery); },
                "Failed to throw a proper exception when the request has timed out"
            );
        }

        [Test]
        public void When_RequestFormatNotRecognized_Then_ShouldThrow()
        {
            // Arrange
            TwinglySearchClient client = SetupTwinglyClientWithResponseFile("NonXmlResponse.xml", request => { }, HttpStatusCode.InternalServerError);
            Query validQuery = QueryBuilder
                                .Create("A valid query")
                                .Build();
            // Act & Assert
            Assert.Throws<RequestException>(
                () => { client.Query(validQuery); },
                "Failed to throw a proper exception when the request has timed out"
            );
        }

        private static TwinglySearchClient SetupTwinglyClientWithResponseFile(string fileName, Action<HttpRequestMessage> delegateAction, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            string executableLocation = TestContext.CurrentContext.TestDirectory;
            string fullFilePath = Path.Combine(executableLocation, "TestData", fileName);

            string responseContents = File.ReadAllText(fullFilePath);
            var response = DelegatingHttpClientHandler.GetStreamHttpResponseMessage(responseContents, statusCode);
            var messageHandler = new DelegatingHttpClientHandler(delegateAction, response);

            return new TwinglySearchClient(new FakeValidConfiguration(), new HttpClient(messageHandler));
        }
    }

    [TestFixture]
    public class TwinglySearchClientIntegrationTests
    {
        [Test, Explicit]
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

        [Test, Explicit]
        public void When_UsingInvalidKey_ShouldThrow()
        {
            // Arrange
            var client = new TwinglySearchClient(new FakeValidConfiguration(), new HttpClient());
            var theQuery = QueryBuilder.Create("twingly page-size:100").Build();

            // Act, Assert (should throw).
            Assert.Throws<AuthException>(
                () => { client.Query(theQuery); },
                "Failed to recognize and throw an error when using a non-existing API key."
            );
        }
    }
}
