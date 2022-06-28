using System.Globalization;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Exceptions;
using Audacia.Azure.StorageQueue.AddMessageToQueue;
using Audacia.Azure.StorageQueue.Exceptions;
using Moq;

namespace Audacia.Azure.QueueStorage.Tests
{
    public class ConfigAzureQueueStorageTests
    {
        private IAddAzureQueueStorageService _addAzureQueueStorageService;

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Should_throw_exception_when_finding_queue_with_name_that_is_null_or_empty(
            string queueName)
        {
            // Arrange
            var mockService = new Mock<IAddAzureQueueStorageService>();
            const string queueMessage = "Hello World!";

            _addAzureQueueStorageService = mockService.Object;

            var expectedException = new QueueDoesNotExistException(queueName, CultureInfo.InvariantCulture);
            mockService.Setup(x =>
                    x.ExecuteAsync(queueName, queueMessage))
                .ThrowsAsync(expectedException);

            // Act
            Task Result() => _addAzureQueueStorageService.ExecuteAsync(queueName, queueMessage);

            // Assert
            await Assert.ThrowsAsync<QueueDoesNotExistException>(Result);
        }

        [Theory]
        [InlineData("Pictures", true)]
        [InlineData("Icons", false)]
        public async Task Should_throw_exception_when_queue_already_exists(
            string queueName,
            bool shouldThrowException)
        {
            // Arrange
            var mockService = new Mock<IAddAzureQueueStorageService>();
            const string queueMessage = "Hello World!";

            _addAzureQueueStorageService = mockService.Object;

            var expectedException = new QueueDoesNotExistException(queueName, CultureInfo.InvariantCulture);
            mockService.Setup(x =>
                    x.ExecuteAsync(queueName, queueMessage))
                .ThrowsAsync(expectedException);

            // Act
            Task Result() => _addAzureQueueStorageService.ExecuteAsync(queueName, queueMessage);

            // Assert
            if (shouldThrowException)
            {
                await Assert.ThrowsAsync<QueueDoesNotExistException>(Result);
            }
        }
    }
}
