using System.Globalization;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.AddBlob;
using Audacia.Azure.BlobStorage.AddBlob.Commands;
using Audacia.Azure.BlobStorage.Exceptions;
using Moq;
using Xunit;

namespace Audacia.Azure.BlobStorage.Tests
{
    public class ConfigAzureBlobStorageTests
    {
        private IAddAzureBlobStorageService _addAzureBlobStorageService;

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Should_throw_exception_when_finding_container_with_name_that_is_null_or_empty(
            string containerName)
        {
            // Arrange
            var mockService = new Mock<IAddAzureBlobStorageService>();

            _addAzureBlobStorageService = mockService.Object;

            var cmd = new AddAzureBlobStorageFileCommand(containerName, "selfie.png", string.Empty);

            var expectedException =
                ContainerNameInvalidException.UnableToFindWithContainerName(CultureInfo.InvariantCulture);
            mockService.Setup(x =>
                    x.ExecuteAsync(It.IsAny<AddAzureBlobStorageFileCommand>()))
                .ThrowsAsync(expectedException);

            // Act
            Task Result() => _addAzureBlobStorageService.ExecuteAsync(cmd);

            // Assert
            await Assert.ThrowsAsync<ContainerNameInvalidException>(Result);
        }

        [Theory]
        [InlineData("Pictures", true)]
        [InlineData("Icons", false)]
        public async Task Should_throw_exception_when_container_already_exists(
            string containerName,
            bool shouldThrowException)
        {
            // Arrange
            var mockService = new Mock<IAddAzureBlobStorageService>();

            _addAzureBlobStorageService = mockService.Object;

            var cmd = new AddAzureBlobStorageFileCommand(containerName, "selfie.png", string.Empty);

            var expectedException = new ContainerDoesNotExistException(containerName, CultureInfo.InvariantCulture);
            mockService.Setup(x =>
                    x.ExecuteAsync(It.Is<AddAzureBlobStorageFileCommand>(y => y.ContainerName == "Pictures")))
                .ThrowsAsync(expectedException);

            // Act
            Task Result() => _addAzureBlobStorageService.ExecuteAsync(cmd);

            // Assert
            if (shouldThrowException)
            {
                await Assert.ThrowsAsync<ContainerDoesNotExistException>(Result);
            }
        }
    }
}
