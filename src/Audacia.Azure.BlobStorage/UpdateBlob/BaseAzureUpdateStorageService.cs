using Audacia.Azure.BlobStorage.Common.Services;
using Audacia.Azure.BlobStorage.Config;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.BlobStorage.UpdateBlob
{
    /// <summary>
    /// Base class for adding and updating blobs.
    /// </summary>
    public abstract class BaseAzureUpdateStorageService : BaseAzureBlobStorageService
    {
        /// <summary>
        /// Constructor for the base updating blob services.
        /// </summary>
        /// <param name="logger">Logger to giving extra information.</param>
        /// <param name="blobServiceClient">An instance of the blob service client to call operations on.</param>
        protected BaseAzureUpdateStorageService(ILogger logger, BlobServiceClient blobServiceClient)
            : base(logger, blobServiceClient)
        {
        }

        /// <summary>
        /// Constructor for the base updating blob services.
        /// </summary>
        /// <param name="logger">Logger to giving extra information.</param>
        /// <param name="blobStorageConfig">Config options for creating a new <see cref="BlobServiceClient"/>.</param>
        protected BaseAzureUpdateStorageService(ILogger logger, IOptions<BlobStorageOption> blobStorageConfig) : base(
            logger, blobStorageConfig)
        {
        }

        /// <summary>
        /// Gets the byte array containing the data from a file stored on the local file storage.
        /// </summary>
        /// <param name="filePath">Full file path for where a file is located.</param>
        /// <returns>Byte array containing the data from the file stored at <paramref name="filePath"/>.</returns>
        /// <exception cref="FileNotFoundException">Thrown when unable to find the file located at <paramref name="filePath"/>.</exception>
        protected static byte[] GetFileData(string filePath)
        {
            var fileExist = File.Exists(filePath);

            if (fileExist)
            {
                var bytes = File.ReadAllBytes(filePath);

                return bytes;
            }

            throw new FileNotFoundException($"File does not exist at: {filePath}");
        }

        /// <summary>
        /// Gets or creates a <see cref="BlobContainerClient"/>.
        /// </summary>
        /// <param name="containerName">The name of the container.</param>
        /// <param name="doesContainerExist">Whether or not you expect the container to exist.</param>
        /// <returns>Returns the existing or newly created <see cref="BlobContainerClient"/>.</returns>
        protected async Task<BlobContainerClient> GetOrCreateContainerAsync(
            string containerName,
            bool doesContainerExist)
        {
            BlobContainerClient container;
            if (!doesContainerExist)
            {
                container = await CreateContainerAsync(containerName).ConfigureAwait(false);
            }
            else
            {
                container = GetContainer(containerName);
            }

            return container;
        }
    }
}
