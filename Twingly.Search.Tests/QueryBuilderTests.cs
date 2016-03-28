using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Twingly.Search.Client;

namespace Twingly.Search.Tests
{
    [TestClass]
    public class BasicInputValidationTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Failed to throw an exception on a null search pattern")]
        public void When_SearchPatternIsNull_Then_ShouldThrow()
        {
            // Arrange
            string searchPattern = null;

            // Act & Assert
            // Should throw exception now
            QueryBuilder.Create(searchPattern);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Failed to throw an exception on an empty search pattern")]
        public void When_SearchPatternIsEmpty_Then_ShouldThrow()
        {
            // Arrange
            string searchPattern = String.Empty;

            // Act & Assert
            // Should throw exception now
            QueryBuilder.Create(searchPattern);
        }

    }
}
