using NUnit.Framework;
using System;
using Twingly.Search.Client;
using Twingly.Search.Client.Domain;

namespace Twingly.Search.Tests
{
    /// <summary>
    /// Verifies basic correctness of the <see cref="QueryBuilder"/>.
    /// More advanced scenarios involving <see cref="QueryBuilder"/>
    /// are covered in <see cref="TwinglySearchClientTests"/> to check
    /// components in interaction.
    /// </summary>
    [TestFixture]
    public class QueryBuilderTests
    {
        [Test]
        public void When_CreatingQueryWithNullSearchQuery_Then_ShouldThrow()
        {
            // Arrange
            string searchQuery = null;

            // Act & Assert
            // Should throw exception now
            Assert.Throws<ArgumentOutOfRangeException>(
                () => { QueryBuilder.Create(searchQuery); },
                "Failed to throw an exception on a null search query"
            );
        }

        [Test]
        public void When_CreatingQueryWithEmptySearchQuery_Then_ShouldThrow()
        {
            // Arrange
            string searchQuery = string.Empty;

            // Act & Assert
            // Should throw exception now
            Assert.Throws<ArgumentOutOfRangeException>(
               () => { QueryBuilder.Create(searchQuery); },
               "Failed to throw an exception on an empty search query"
            );
        }

        [Test]
        public void When_StartDateInTheFuture_Then_ShouldThrow()
        {
            // Arrange
            string searchQuery = "Hey, i'm valid!";

            // Act & Assert
            // Should throw exception now
            Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                {
                    QueryBuilder.Create(searchQuery)
                                .StartTime(DateTime.UtcNow.AddDays(1))
                                .Build();
                },
                "Failed to throw an exception when query start date is set to future"
            );
        }

        [Test]
        public void When_EndDateComesEarlierThanStartDate_Then_ShouldThrow()
        {
            // Arrange
            string searchQuery = "Hey, i'm valid!";

            // Act & Assert
            // Should throw exception now
            Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                {
                    QueryBuilder.Create(searchQuery)
                                .StartTime(DateTime.UtcNow)
                                .EndTime(DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)))
                                .Build();
                },
                "Failed to throw an exception when end date comes before the start date"
            );
        }

        [Test]
        public void When_SettingQueryToNull_Then_ShouldThrow()
        {
            // Arrange
            string searchQuery = "Hey, i'm valid at first!";
            QueryBuilder builder = QueryBuilder.Create(searchQuery)
                                                .StartTime(DateTime.UtcNow)
                                                .EndTime(DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)));

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => { builder.SearchQuery(null); },
                "Failed to throw an exception on an empty search query"
            );
        }

        [Test]
        public void When_SettingQueryToEmptyString_Then_ShouldThrow()
        {
            // Arrange
            string searchQuery = "Hey, i'm valid at first!";
            QueryBuilder builder = QueryBuilder.Create(searchQuery)
                                                .StartTime(DateTime.UtcNow)
                                                .EndTime(DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)));

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => { builder.SearchQuery(string.Empty); },
                "Failed to throw an exception on an null search query"
            );
        }

        [Test]
        public void When_CreatingQueryWithValidQuery_Then_ShouldSucceed()
        {
            // Arrange
            string searchQuery = "Hey, i'm totally valid!";
            string errorMessage = "Failed to set the search query to the given value";

            // Act
            Query testResult = QueryBuilder.Create(searchQuery)
                                                .Build();

            // Assert
            Assert.AreEqual(searchQuery, testResult.SearchQuery, errorMessage);
        }

        [Test]
        public void When_SettingUnknownLanguageToNonEmptyValue_Then_ShouldSucceed()
        {
            // Arrange
            string language = "WW";
            string errorMessage = "Failed to set the language to the given value";

            // Act
            Query testResult = QueryBuilder.Create("Some text")
                                           .Language(language)
                                           .Build();

            // Assert
            Assert.AreEqual(language, testResult.Language, errorMessage);
        }

        [Test]
        public void When_SettingUnknownLanguageToNullValue_Then_ShouldThrow()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() =>
            {
                QueryBuilder.Create("Some text")
                            .Language(null)
                            .Build();
            });
        }

        [Test]
        public void When_SettingUnknownLanguageToEmptyValue_Then_ShouldThrow()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() =>
            {
                QueryBuilder.Create("Some text")
                             .Language(" ")
                             .Build();
            });
        }
    }
}
