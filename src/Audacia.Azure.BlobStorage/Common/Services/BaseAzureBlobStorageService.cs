using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.BlobStorage.Exceptions;
using Audacia.Azure.BlobStorage.Exceptions.BlobContainerExceptions;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.BlobStorage.Common.Services
{
    /// <summary>
    /// Base Service class for all the Azure Blob services, containing shared methods and properties to use.
    /// </summary>
    public abstract class BaseAzureBlobStorageService
    {
        private readonly string _storageAccountConnectionString =
            "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1};BlobEndpoint={2}";

        private readonly string _storageAccountUrl = "https://{0}.blob.core.windows.net";

        private readonly string _accountName;

        private string StorageAccountString => string.Format(FormatProvider, _storageAccountUrl, _accountName);

        /// <summary>
        /// Gets the URL of where the Storage account is hosted.
        /// </summary>
        public Uri StorageAccountUrl => new Uri(StorageAccountString);

        /// <summary>
        /// Gets the logger to allow for providing extra information throughout the flow of the services.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Gets the format provider for formatting exception messages.
        /// </summary>
        protected static IFormatProvider FormatProvider => CultureInfo.InvariantCulture;

        /// <summary>
        /// Gets an instance of the service client for a blob to provide an object to carry out the required operations.
        /// </summary>
        protected BlobServiceClient BlobServiceClient { get; }

        /// <summary>
        /// Base constructor checking <see cref="BlobServiceClient"/> is valid and creating an instance if so. 
        /// </summary>
        /// <param name="logger">Logger for providing extra information when services are called.</param>
        /// <param name="blobServiceClient">An instance of the blob service client to call operations on.</param>
        /// <exception cref="BlobStorageConfigurationException">
        /// Exception thrown if the <see cref="BlobServiceClient"/> passed in is null.
        /// </exception>
        protected BaseAzureBlobStorageService(ILogger logger, BlobServiceClient blobServiceClient)
        {
            BlobServiceClient = blobServiceClient ?? throw BlobStorageConfigurationException.BlobClientNotConfigured();

            Logger = logger;
            _accountName = blobServiceClient.AccountName;
        }

        /// <summary>
        /// Base constructor checking <see cref="BlobServiceClient"/> is valid and creating an instance if so. 
        /// </summary>
        /// <param name="logger">Logger for providing extra information when services are called.</param>
        /// <param name="blobStorageConfig">Config options for creating a new <see cref="BlobServiceClient"/>.</param>
        protected BaseAzureBlobStorageService(ILogger logger, IOptions<BlobStorageOption> blobStorageConfig)
        {
            OptionsConfigCheck(blobStorageConfig);

            _accountName = blobStorageConfig.Value.AccountName;

            var blobEndpoint = blobStorageConfig?.Value?.BlobEndpoint?.ToString();
            if (!string.IsNullOrEmpty(blobEndpoint))
            {
                _storageAccountUrl = blobEndpoint;
            }

            var storageAccountConnectionString = string.Format(
                FormatProvider,
                _storageAccountConnectionString,
                blobStorageConfig!.Value.AccountName,
                blobStorageConfig!.Value.AccountKey,
                StorageAccountString);

            BlobServiceClient = new BlobServiceClient(storageAccountConnectionString);
            Logger = logger;
        }

        /// <summary>
        /// Checks if the <paramref name="blobStorageConfig"/> are valid to create a <see cref="BlobServiceClient"/>.
        /// </summary>
        /// <param name="blobStorageConfig">An instance of the <see cref="BlobStorageOption"/>.</param>
        /// <exception cref="BlobStorageConfigurationException">
        /// Exception thrown when a property from the <see cref="BlobStorageOption"/> is either null or empty.
        /// </exception>
        private static void OptionsConfigCheck(IOptions<BlobStorageOption> blobStorageConfig)
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

        /// <summary>
        /// Gets a <see cref="BlobContainerClient"/> from an Azure blob storage account.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <returns>
        /// The instance of the <see cref="BlobContainerClient"/> with the name <paramref name="containerName"/>.
        /// </returns>
        /// <exception cref="BlobContainerNameInvalidException">
        /// Exception thrown when the name of the container is invalid.
        /// </exception>
        protected BlobContainerClient GetContainer(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw BlobContainerNameInvalidException.UnableToFindWithEmptyContainerName(FormatProvider);
            }

            return BlobServiceClient.GetBlobContainerClient(containerName);
        }

        /// <summary>
        /// Creates a new container with an Azure blob storage account.
        /// </summary>
        /// <param name="containerName">Name of the container which is getting created.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An instance of the newly created <see cref="BlobContainerClient"/>.</returns>
        /// <exception cref="BlobContainerNameInvalidException">
        /// Exception thrown when the name of the container is invalid.
        /// </exception>
        protected async Task<BlobContainerClient> CreateContainerAsync(string containerName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw BlobContainerNameInvalidException.UnableToCreateWithContainerName(containerName, FormatProvider);
            }

            return await BlobServiceClient.CreateBlobContainerAsync(containerName, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Checks if the container is pre existing.
        /// </summary>
        /// <param name="containerName">Name of the container where the blob lives.</param>
        /// <param name="doesContainerExist">
        /// Whether the container exists and whether that matches what has been passed apart of the command.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="BlobContainerDoesNotExistException">
        /// Exception thrown when configuration is not set to create a new container and the container specified does
        /// not exist.
        /// </exception>
        /// <exception cref="BlobContainerAlreadyExistsException">
        /// Exception thrown when configuration wants to create a new container however it already exists.
        /// </exception>
        /// <returns>Returns a boolean representing if the container exists within the storage account.</returns>
#pragma warning disable AV1564
        protected async Task<bool> ContainerChecksAsync(string containerName, bool doesContainerExist, CancellationToken cancellationToken)
#pragma warning restore AV1564
        {
            var containerExists = await CheckContainerExistsAsync(containerName, cancellationToken).ConfigureAwait(false);

            if (doesContainerExist && !containerExists)
            {
                throw new BlobContainerDoesNotExistException(containerName, FormatProvider);
            }

            // We should check that there is no containers already existing with the name passed in.
            if (!doesContainerExist && containerExists)
            {
                throw new BlobContainerAlreadyExistsException(containerName, FormatProvider);
            }

            return containerExists;
        }

        private async Task<bool> CheckContainerExistsAsync(string containerName, CancellationToken cancellationToken)
        {
            var containerExists = false;
            var containers = BlobServiceClient.GetBlobContainersAsync(cancellationToken: cancellationToken).AsPages();

            // Use the async method instead of all containers.
            await foreach (var pagedContainers in containers)
            {
                containerExists = pagedContainers.Values.Any(containerItem => containerItem.Name == containerName);

                if (containerExists)
                {
                    break;
                }
            }

            return containerExists;
        }
    }
}