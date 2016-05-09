using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    [TestClass]
    public class QueryBuilderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Failed to throw an exception on a null search pattern")]
        public void When_CreatingQueryWithNullSearchPattern_Then_ShouldThrow()
        {
            // Arrange
            string searchPattern = null;

            // Act & Assert
            // Should throw exception now
            QueryBuilder.Create(searchPattern);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Failed to throw an exception on an empty search pattern")]
        public void When_CreatingQueryWithEmptySearchPattern_Then_ShouldThrow()
        {
            // Arrange
            string searchPattern = String.Empty;

            // Act & Assert
            // Should throw exception now
            QueryBuilder.Create(searchPattern);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Failed to throw an exception when query start date is set to future")]
        public void When_StartDateInTheFuture_Then_ShouldThrow()
        {
            // Arrange
            string searchPattern = "Hey, i'm valid!";

            // Act & Assert
            // Should throw exception now
            QueryBuilder.Create(searchPattern)
                        .StartTime(DateTime.UtcNow.AddDays(1))
                        .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Failed to throw an exception when end date comes before the start date")]
        public void When_EndDateComesEarlierThanStartDate_Then_ShouldThrow()
        {
            // Arrange
            string searchPattern = "Hey, i'm valid!";

            // Act & Assert
            // Should throw exception now
            QueryBuilder.Create(searchPattern)
                        .StartTime(DateTime.UtcNow)
                        .EndTime(DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)))
                        .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Failed to throw an exception on an empty search pattern")]
        public void When_SettingQueryPatternToNull_Then_ShouldThrow()
        {
            // Arrange
            string searchPattern = "Hey, i'm valid at first!";
            QueryBuilder builder = QueryBuilder.Create(searchPattern)
                                                .StartTime(DateTime.UtcNow)
                                                .EndTime(DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)));

            // Act & Assert
            builder.SearchPattern(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Failed to throw an exception on an null search pattern")]
        public void When_SettingQueryPatternToEmptyString_Then_ShouldThrow()
        {
            // Arrange
            string searchPattern = "Hey, i'm valid at first!";
            QueryBuilder builder = QueryBuilder.Create(searchPattern)
                                                .StartTime(DateTime.UtcNow)
                                                .EndTime(DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)));

            // Act & Assert
            builder.SearchPattern(String.Empty);
        }

        [TestMethod]
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

        [TestMethod]
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

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void When_SettingUnknownLanguageToNullValue_Then_ShouldThrow()
        {
            // Arrange, Act, Assert
            Query testResult = QueryBuilder.Create("Some text")
                                           .Language(null)
                                           .Build();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void When_SettingUnknownLanguageToEmptyValue_Then_ShouldThrow()
        {
            // Arrange, Act, Assert
            Query testResult = QueryBuilder.Create("Some text")
                                           .Language(" ")
                                           .Build();
        }
    }
}
