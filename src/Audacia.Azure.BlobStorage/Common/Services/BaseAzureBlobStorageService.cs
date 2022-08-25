using System;
using System.Globalization;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.BlobStorage.Exceptions;
using Audacia.Azure.BlobStorage.Extensions;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.BlobStorage.Common.Services
{
    public abstract class BaseAzureBlobStorageService
    {
        private readonly string _storageAccountConnectionString =
            "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}";

        private readonly string _storageAccountUrl = "https://{0}.blob.core.windows.net";

        private readonly string _accountName;

        public Uri StorageAccountUrl => new(string.Format(FormatProvider, _storageAccountUrl, _accountName));

        protected ILogger Logger { get; }

        protected IFormatProvider FormatProvider { get; }

        protected BlobServiceClient BlobServiceClient { get; }

        protected BaseAzureBlobStorageService(ILogger logger, BlobServiceClient blobServiceClient)
        {
            BlobServiceClient = blobServiceClient ?? throw BlobStorageConfigurationException.BlobClientNotConfigured();

            Logger = logger;
            _accountName = blobServiceClient.AccountName;
            FormatProvider = CultureInfo.InvariantCulture;
        }

        protected BaseAzureBlobStorageService(ILogger logger, IOptions<BlobStorageOption> blobStorageConfig)
        {
            FormatProvider = CultureInfo.InvariantCulture;

            OptionsConfigCheck(blobStorageConfig);

            var storageAccountConnectionString = string.Format(FormatProvider, _storageAccountConnectionString,
                blobStorageConfig.Value.AccountName, blobStorageConfig.Value.AccountKey);

            BlobServiceClient = new BlobServiceClient(storageAccountConnectionString);
            Logger = logger;
            _accountName = blobStorageConfig.Value.AccountName;
        }

        private void OptionsConfigCheck(IOptions<BlobStorageOption> blobStorageConfig)
        {
            if (blobStorageConfig?.Value == null)
            {
                throw BlobStorageConfigurationException.OptionsNotConfigured();
            }

            if (string.IsNullOrEmpty(blobStorageConfig.Value.AccountName))
            {
                throw BlobStorageConfigurationException.AccountNameNotConfigured(FormatProvider);
            }

            if (string.IsNullOrEmpty(blobStorageConfig.Value.AccountKey))
            {
                throw BlobStorageConfigurationException.AccountKeyNotConfigured(FormatProvider);
            }
        }

        protected BlobContainerClient GetContainer(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw ContainerNameInvalidException.UnableToFindWithContainerName(FormatProvider);
            }

            return BlobServiceClient.GetBlobContainerClient(containerName);
        }

        protected async Task<BlobContainerClient> CreateContainerAsync(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw ContainerNameInvalidException.UnableToCreateWithContainerName(containerName, FormatProvider);
            }

            return await BlobServiceClient.CreateBlobContainerAsync(containerName);
        }

        /// <summary>
        /// Checks if the container is pre existing.
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="doesContainerExist"></param>
        /// <exception cref="ContainerDoesNotExistException"></exception>
        /// <exception cref="ContainerAlreadyExistsException"></exception>
        protected void ContainerChecks(string containerName, bool doesContainerExist)
        {
            var storageAccountContainers = BlobServiceClient.GetBlobContainers();

            var checkContainerExists = storageAccountContainers.AlreadyExists(containerName);
            if (doesContainerExist && !checkContainerExists)
            {
                throw new ContainerDoesNotExistException(containerName, FormatProvider);
            }

            // We should check that there is no containers already existing with the name passed in.
            if (!doesContainerExist && checkContainerExists)
            {
                throw new ContainerAlreadyExistsException(containerName, FormatProvider);
            }
        }
    }
}
