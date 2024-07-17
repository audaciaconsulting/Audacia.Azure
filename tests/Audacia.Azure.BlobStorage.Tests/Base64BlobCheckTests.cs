using System.Globalization;
using Audacia.Azure.BlobStorage.Exceptions.BlobDataExceptions;
using Audacia.Azure.BlobStorage.Extensions;
using Audacia.Azure.BlobStorage.Models;

namespace Audacia.Azure.BlobStorage.Tests
{
    public class Base64BlobCheckTests
    {
        [Fact]
        public void Checking_valid_base_64()
        {
            // Arrange
            var blobName = "Hello";
            var validImageInBaseSixtyFour =
                "iVBORw0KGgoAAAANSUhEUgAAAQAAAAEACAIAAADTED8xAAADMElEQVR4nOzVwQnAIBQFQYXff81RUkQCOyDj1YOPnbXWPmeTRef+/3O/OyBjzh3CD95BfqICMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMO0TAAD//2Anhf4QtqobAAAAAElFTkSuQmCC";

            // Act
            var bytes = validImageInBaseSixtyFour.BaseSixtyFourBlobChecks(blobName, CultureInfo.InvariantCulture);

            // Assert
            Assert.NotEmpty(bytes);
        }

        [Fact]
        public void Checking_invalid_base_64()
        {
            // Arrange
            var blobName = "Hello";
            var invalidImageInBaseSixtyFour = "invalid-base-sixty-four";

            // Acts
            var exception = Assert.Throws<BlobDataCannotBeInvalidBaseSixtyFourException>(
                () => invalidImageInBaseSixtyFour.BaseSixtyFourBlobChecks(blobName, CultureInfo.InvariantCulture));

            // Assert
            Assert.Equal(
                $"Cannot add Blob: {blobName} because the data value is invalid Base 64: {invalidImageInBaseSixtyFour}",
                exception.Message);
        }


        [Fact]
        public void Checking_empty_base_64()
        {
            // Arrange
            var blobName = "Hello";
            var invalidImageInBaseSixtyFour = "";

            // Acts
            var exception = Assert.Throws<BlobDataCannotBeEmptyException>(
                () => invalidImageInBaseSixtyFour.BaseSixtyFourBlobChecks(blobName, CultureInfo.InvariantCulture));

            // Assert
            Assert.Equal(
                $"Cannot add Blob: {blobName} which is of type {BlobDataType.BaseSixtyFour} be empty",
                exception.Message);
        }
    }
}