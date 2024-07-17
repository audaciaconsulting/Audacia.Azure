using System.Globalization;
using Audacia.Azure.StorageQueue.AddMessageToQueue;
using Audacia.Azure.StorageQueue.Config;
using Audacia.Azure.StorageQueue.Exceptions;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.StorageQueue.Tests
{
    public class SetupAzureQueueStorageTests
    {
        [Fact]
        public async Task Should_throw_exception_if_option_value_is_null()
        {
            // Arrange
            var queueStorageOption = Options.Create<QueueStorageOption>(null);

            // Act
            var expectedException = StorageQueueConfigurationException.OptionsNotConfigured();
            Exception thrownException = null;
            try
            {
                var addAzureQueueStorageService =
                    new AddAzureQueueStorageService(queueStorageOption);
            }
            catch (StorageQueueConfigurationException exception)
            {
                thrownException = exception;
            }

            // Assert
            Assert.NotNull(thrownException);
            Assert.Equal(expectedException.GetType(), thrownException.GetType());
            Assert.Equal(expectedException.Message, thrownException.Message);
        }

        [Fact]
        public async Task Should_throw_exception_if_option_value_is_empty()
        {
            // Arrange
            var queueStorageOption = new QueueStorageOption();

            var queueStorageOptions = Options.Create(queueStorageOption);

            // Act
            var expectedException =
                StorageQueueConfigurationException.AccountNameNotConfigured(CultureInfo.InvariantCulture);
            Exception thrownException = null;
            try
            {
                var addAzureQueueStorageService =
                    new AddAzureQueueStorageService(queueStorageOptions);
            }
            catch (StorageQueueConfigurationException exception)
            {
                thrownException = exception;
            }

            // Assert
            Assert.NotNull(thrownException);
            Assert.Equal(expectedException.GetType(), thrownException.GetType());
            Assert.Equal(expectedException.Message, thrownException.Message);
        }

        [Theory]
        [InlineData("Photos-dev", null)]
        [InlineData("Files", "")]
        public async Task Should_throw_exception_if_account_key_option_value_is_null_or_empty(
            string accountName,
            string accountKey)
        {
            // Arrange
            var queueStorageOption = new QueueStorageOption
            {
                AccountName = accountName,
                AccountKey = accountKey
            };

            var queueStorageOptions = Options.Create(queueStorageOption);

            // Act
            var expectedException =
                StorageQueueConfigurationException.AccountKeyNotConfigured(CultureInfo.InvariantCulture);
            Exception thrownException = null;
            try
            {
                var addAzureQueueStorageService =
                    new AddAzureQueueStorageService(queueStorageOptions);
            }
            catch (StorageQueueConfigurationException exception)
            {
                thrownException = exception;
            }

            // Assert
            Assert.NotNull(thrownException);
            Assert.Equal(expectedException.GetType(), thrownException.GetType());
            Assert.Equal(expectedException.Message, thrownException.Message);
        }

        [Theory]
        [InlineData(null, "123685924")]
        [InlineData("", "968574125")]
        public async Task Should_throw_exception_if_account_name_option_value_is_null_or_empty(
            string accountName,
            string accountKey)
        {
            // Arrange
            var queueStorageOption = new QueueStorageOption
            {
                AccountName = accountName,
                AccountKey = accountKey
            };

            var queueStorageOptions = Options.Create(queueStorageOption);

            // Act
            var expectedException =
                StorageQueueConfigurationException.AccountNameNotConfigured(CultureInfo.InvariantCulture);
            Exception thrownException = null;
            try
            {
                var addAzureQueueStorageService =
                    new AddAzureQueueStorageService(queueStorageOptions);
            }
            catch (StorageQueueConfigurationException exception)
            {
                thrownException = exception;
            }

            // Assert
            Assert.NotNull(thrownException);
            Assert.Equal(expectedException.GetType(), thrownException.GetType());
            Assert.Equal(expectedException.Message, thrownException.Message);
        }
    }
}