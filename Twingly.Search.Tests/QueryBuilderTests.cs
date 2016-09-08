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
        public void When_CreatingQueryWithNullSearchPattern_Then_ShouldThrow()
        {
            // Arrange
            string searchPattern = null;

            // Act & Assert
            // Should throw exception now
            Assert.Throws<ArgumentOutOfRangeException>(
                () => { QueryBuilder.Create(searchPattern); },
                "Failed to throw an exception on a null search pattern"
            );
        }

        [Test]
        public void When_CreatingQueryWithEmptySearchPattern_Then_ShouldThrow()
        {
            // Arrange
            string searchPattern = string.Empty;

            // Act & Assert
            // Should throw exception now
            Assert.Throws<ArgumentOutOfRangeException>(
               () => { QueryBuilder.Create(searchPattern); },
               "Failed to throw an exception on an empty search pattern"
            );
        }

        [Test]
        public void When_StartDateInTheFuture_Then_ShouldThrow()
        {
            // Arrange
            string searchPattern = "Hey, i'm valid!";

            // Act & Assert
            // Should throw exception now
            Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                {
                    QueryBuilder.Create(searchPattern)
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
            string searchPattern = "Hey, i'm valid!";

            // Act & Assert
            // Should throw exception now
            Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                {
                    QueryBuilder.Create(searchPattern)
                                .StartTime(DateTime.UtcNow)
                                .EndTime(DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)))
                                .Build();
                },
                "Failed to throw an exception when end date comes before the start date"
            );
        }

        [Test]
        public void When_SettingQueryPatternToNull_Then_ShouldThrow()
        {
            // Arrange
            string searchPattern = "Hey, i'm valid at first!";
            QueryBuilder builder = QueryBuilder.Create(searchPattern)
                                                .StartTime(DateTime.UtcNow)
                                                .EndTime(DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)));

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => { builder.SearchPattern(null); },
                "Failed to throw an exception on an empty search pattern"
            );
        }

        [Test]
        public void When_SettingQueryPatternToEmptyString_Then_ShouldThrow()
        {
            // Arrange
            string searchPattern = "Hey, i'm valid at first!";
            QueryBuilder builder = QueryBuilder.Create(searchPattern)
                                                .StartTime(DateTime.UtcNow)
                                                .EndTime(DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)));

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => { builder.SearchPattern(string.Empty); },
                "Failed to throw an exception on an null search pattern"
            );
        }

        [Test]
        public void When_CreatingQueryWithValidPattern_Then_ShouldSucceed()
        {
            // Arrange
            string searchPattern = "Hey, i'm totally valid!";
            string errorMessage = "Failed to set the search pattern to the given value";

            // Act
            Query testResult = QueryBuilder.Create(searchPattern)
                                                .Build();

            // Assert
            Assert.AreEqual(searchPattern, testResult.SearchPattern, errorMessage);
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
