using Audacia.Azure.BlobStorage.Common.Services;
using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.BlobStorage.Exceptions;
using Audacia.Azure.BlobStorage.Exceptions.BlobContainerExceptions;
using Audacia.Azure.Common.ReturnOptions;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.BlobStorage.GetBlob
{
    /// <summary>
    /// Get service for returning blob from an Azure Blob Storage account.
    /// </summary>
    public class GetAzureBlobStorageService : BaseAzureBlobStorageService, IGetAzureBlobStorageService
    {
        private string StorageAccountWithContainer => $"{StorageAccountUrl}{{0}}";

        /// <summary>
        /// Constructor option for when adding the <see cref="BlobServiceClient"/> has being added to the DI.
        /// </summary>
        /// <param name="logger">Logger to giving extra information.</param>
        /// <param name="blobServiceClient">Blob Service client used get blobs from Azure.</param>
        public GetAzureBlobStorageService(
            ILogger<GetAzureBlobStorageService> logger,
            BlobServiceClient blobServiceClient) : base(
            logger, blobServiceClient)
        {
        }

        /// <summary>
        /// Constructor option for using the Options pattern with <see cref="BlobStorageOption"/>.
        /// </summary>
        /// <param name="logger">Logger to giving extra information.</param>
        /// <param name="blobStorageConfig"><see cref="BlobStorageOption"/> allowing us to create a <see cref="BlobServiceClient"/>.</param>
        public GetAzureBlobStorageService(
            ILogger<GetAzureBlobStorageService> logger,
            IOptions<BlobStorageOption> blobStorageConfig)
            : base(logger, blobStorageConfig)
        {
        }

        /// <summary>
        /// Returns a blob with the name <paramref name="blobName"/> within the <paramref name="containerName"/>.
        /// </summary>
        /// <param name="containerName">The name of the container where the blob you want to return is stored in.</param>
        /// <param name="blobName">The name of the blob you are wanting to return.</param>
        /// <typeparam name="TResult">The type of data which you want the blob to be converted into. This must match one of the
        /// return options. Please look into the different return options to decide which is best suited for you.</typeparam>
        /// <typeparam name="TResponse">The return option which you want the blob to be returned in.</typeparam>
        /// <returns>A <see cref="IBlobReturnOption{TResult}"/> which has been configured by the generic arguments.</returns>
        /// <exception cref="BlobContainerDoesNotExistException">
        /// Exception thrown when configuration is not set to create a new container and the container specified does
        /// not exist.
        /// </exception>
        public async Task<TResult> GetAsync<TResult, TResponse>(string containerName, string blobName)
            where TResponse : IBlobReturnOption<TResult>, new()
        {
            var containers = BlobServiceClient.GetBlobContainers();
            var containerExists = containers.Any(container => container.Name == containerName);

            if (containerExists)
            {
                var containerClient = BlobServiceClient.GetBlobContainerClient(containerName);

                var blobBytes = await GetBlobBytesAsync(containerClient, blobName).ConfigureAwait(false);

                var blobClientUrlString = string.Format(FormatProvider, StorageAccountWithContainer, containerName);
                var blobClientUrl = new Uri(blobClientUrlString);
                return new TResponse().Parse(blobName, blobBytes, blobClientUrl);
            }

            throw new BlobContainerDoesNotExistException(containerName, FormatProvider);
        }

        /// <summary>
        /// Returns a collection of blob based on the collection of blob names <paramref name="blobNames"/> within the
        /// container within an Azure storage account.
        /// </summary>
        /// <param name="containerName">The name of the container where the blob you want to return is stored in.</param>
        /// <param name="blobNames">A collection of blob names you are wanting to return.</param>
        /// <typeparam name="T">The type of data which you want the blob to be converted into. This must match one of the
        /// return options. Please look into the different return options to decide which is best suited for you.</typeparam>
        /// <typeparam name="TResponse">The return option which you want the blob to be returned in.</typeparam>
        /// <returns>A <see cref="IBlobReturnOption{TResult}"/> which has been configured by the generic arguments.</returns>
        /// <exception cref="BlobContainerDoesNotExistException">
        /// Exception thrown when configuration is not set to create a new container and the container specified does
        /// not exist.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="blobNames"/> is null.</exception>
        /// <exception cref="BlobContainerNameInvalidException">
        /// Exception thrown when the container cannot be found within Azure.
        /// </exception>
        public async Task<IDictionary<string, T>> GetSomeAsync<T, TResponse>(
            string containerName,
            IEnumerable<string> blobNames)
            where TResponse : IBlobReturnOption<T>, new()
        {
            if (blobNames == null)
            {
                throw new ArgumentNullException(nameof(blobNames));
            }

            var containers = BlobServiceClient.GetBlobContainers();
            var containerExists = containers.Any(container => container.Name == containerName);

            if (containerExists)
            {
                var containerClient = BlobServiceClient.GetBlobContainerClient(containerName);

                if (containerClient is null)
                {
                    throw BlobContainerNameInvalidException.UnableToFindWithContainerName(
                        FormatProvider,
                        containerName);
                }

                return await GetBlobsAsync<T, TResponse>(containerName, blobNames, containerClient)
                    .ConfigureAwait(false);
            }

            throw new BlobContainerDoesNotExistException(containerName, FormatProvider);
        }

        /// <summary>
        /// Gets all the blob bytes from the collection of blob names.
        /// </summary>
        /// <param name="containerName">The name of the container where the blob you want to return is stored in.</param>
        /// <param name="blobNames">A collection of blob names you are wanting to return.</param>
        /// <param name="containerClient">Client of the container which blobs are located.</param>
        /// <typeparam name="T">The type of data which you want the blob to be converted into. This must match one of the
        /// return options. Please look into the different return options to decide which is best suited for you.</typeparam>
        /// <typeparam name="TResponse">The return option which you want the blob to be returned in.</typeparam>
        /// <returns>Dictionary of Blobs, where the key is the name of the blob and the value is the blob itself.</returns>
        private async Task<IDictionary<string, T>> GetBlobsAsync<T, TResponse>(
            string containerName,
            IEnumerable<string> blobNames,
            BlobContainerClient containerClient) where TResponse : IBlobReturnOption<T>, new()
        {
            var blobBytesDictionary = new Dictionary<string, T>();
            foreach (var blobName in blobNames)
            {
                var blobBytes = await GetBlobBytesAsync(containerClient, blobName).ConfigureAwait(false);

                var blobClientUrlString = string.Format(FormatProvider, StorageAccountWithContainer, containerName);
                var blobClientUrl = new Uri(blobClientUrlString);
                var parsedResult = new TResponse().Parse(blobName, blobBytes, blobClientUrl);

                blobBytesDictionary.Add(blobName, parsedResult);
            }

            return blobBytesDictionary;
        }

        /// <summary>
        /// Returns all the blob within a container within the storage account.
        /// </summary>
        /// <param name="containerName">The name of the container where the blobs you want to return are stored in.</param>
        /// <typeparam name="T">The type of data which you want the blob to be converted into. This must match one of the
        /// return options. Please look into the different return options to decide which is best suited for you.</typeparam>
        /// <typeparam name="TResponse">The return option which you want the blob to be returned in.</typeparam>
        /// <returns>A collection of <see cref="IBlobReturnOption{TResult}"/> which has been configured by the generic arguments.</returns>
        /// <exception cref="BlobContainerDoesNotExistException">
        /// Exception thrown when configuration is not set to create a new container and the container specified does
        /// not exist.
        /// </exception>
        public async Task<IDictionary<string, T>> GetAllAsync<T, TResponse>(string containerName)
            where TResponse : IBlobReturnOption<T>, new()
        {
            var containers = BlobServiceClient.GetBlobContainers();
            var containerExists = containers.Any(container => container.Name == containerName);

            if (containerExists)
            {
                var containerClient = BlobServiceClient.GetBlobContainerClient(containerName);

                return await GetAllBlobsAsync<T, TResponse>(containerClient, containerName).ConfigureAwait(false);
            }

            throw new BlobContainerDoesNotExistException(containerName, FormatProvider);
        }

        private async Task<Dictionary<string, T>> GetAllBlobsAsync<T, TResponse>(
            BlobContainerClient containerClient, string containerName)
            where TResponse : IBlobReturnOption<T>, new()
        {
            var pagedBlobs = containerClient.GetBlobs();

            var blobs = pagedBlobs.ToList();

            var blobBytesDictionary = new Dictionary<string, T>();

            foreach (var blob in blobs)
            {
                var blobBytes = await GetBlobBytesAsync(containerClient, blob.Name).ConfigureAwait(false);

                var blobClientUrlString = string.Format(FormatProvider, StorageAccountWithContainer, containerName);
                var blobClientUrl = new Uri(blobClientUrlString);
                var parsedResult = new TResponse().Parse(blob.Name, blobBytes, blobClientUrl);

                blobBytesDictionary.Add(blob.Name, parsedResult);
            }

            return blobBytesDictionary;
        }

        /// <summary>
        /// Downloads the blob from the storage account and returns a convert byte array of the blobs data.
        /// </summary>
        /// <param name="containerClient">Name of the container where the blob is stored within.</param>
        /// <param name="blobName">Name of the blob which is going to be downloaded from the storage account.</param>
        /// <returns>Byte array of the data of the blob.</returns>
        private static async Task<byte[]> GetBlobBytesAsync(BlobContainerClient containerClient, string blobName)
        {
            var blobClient = containerClient.GetBlobClient(blobName);
            var blobDownloadInfo = await blobClient.DownloadAsync().ConfigureAwait(false);

            using var memoryStream = new MemoryStream();
            await blobDownloadInfo.Value.Content.CopyToAsync(memoryStream).ConfigureAwait(false);

            var blobBytes = memoryStream.ToArray();

            return blobBytes;
        }
    }
}
