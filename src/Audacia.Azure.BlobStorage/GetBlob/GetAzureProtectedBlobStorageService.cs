using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Common.Services;
using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.BlobStorage.Exceptions.BlobContainerExceptions;
using Audacia.Azure.Common.ReturnOptions;
using Audacia.Azure.Common.ReturnOptions.ImageOption;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.BlobStorage.GetBlob
{
    /// <summary>
    /// Get service for returning a protected blob from an Azure Blob Storage account.
    /// </summary>
    public class GetAzureProtectedBlobStorageService : BaseAzureBlobStorageService, IGetAzureProtectedBlobStorageService
    {
        /// <summary>
        /// Constructor option for when adding the <see cref="BlobServiceClient"/> has being added to the DI.
        /// </summary>
        /// <param name="logger">Logger to giving extra information.</param>
        /// <param name="blobServiceClient">Blob Service client used get blobs from Azure.</param>
        public GetAzureProtectedBlobStorageService(
            ILogger<GetAzureProtectedBlobStorageService> logger,
            BlobServiceClient blobServiceClient) : base(
            logger, blobServiceClient)
        {
        }

        /// <summary>
        /// Constructor option for using the Options pattern with <see cref="BlobStorageOption"/>.
        /// </summary>
        /// <param name="logger">Logger to giving extra information.</param>
        /// <param name="blobStorageConfig"><see cref="BlobStorageOption"/> allowing us to create a <see cref="BlobServiceClient"/>.</param>
        public GetAzureProtectedBlobStorageService(
            ILogger<GetAzureProtectedBlobStorageService> logger,
            IOptions<BlobStorageOption> blobStorageConfig)
            : base(logger, blobStorageConfig)
        {
        }

        /// <summary>
        /// Returns a blob with the name <paramref name="blobName"/> within the <paramref name="containerName"/> that is
        /// protected with a SAS token.
        /// </summary>
        /// <param name="containerName">The name of the container where the blobs you want to return are stored in.</param>
        /// <param name="blobName">The name of the blob to get within the container.</param>
        /// <param name="policyName">The name of the security policy for the container.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TResponse">The return option which you want the blob to be returned in.</typeparam>
        /// <returns>Returns a blob within a container within the storage account.</returns>
        /// <exception cref="BlobContainerDoesNotExistException">
        /// Exception thrown when configuration is not set to create a new container and the container specified does
        /// not exist.
        /// </exception>
        public async Task<string> GetAsync<TResponse>(
            string containerName,
            string blobName,
            string? policyName,
            CancellationToken cancellationToken = default)
            where TResponse : ReturnProtectedUrlOption, new()
        {
            var containerExists = await ContainerChecksAsync(containerName, true, cancellationToken).ConfigureAwait(false);

            if (containerExists)
            {
                var containerClient = BlobServiceClient.GetBlobContainerClient(containerName);
                var result = ProcessProtectedBlob<TResponse>(containerClient, blobName, policyName);

                return result;
            }

            throw new BlobContainerDoesNotExistException(containerName, FormatProvider);
        }

        /// <summary>
        /// Returns a collection of blob based on the collection of blob names <paramref name="blobNames"/> within the
        /// container within an Azure storage account that are protected with a SAS token.
        /// </summary>
        /// <param name="containerName">The name of the container where the blobs you want to return are stored in.</param>
        /// <param name="blobNames">The names of the blob's to get within the container.</param>
        /// <param name="policyName">The name of the security policy for the container.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
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
        public async Task<IDictionary<string, string>> GetSomeAsync<TResponse>(
            string containerName,
            IEnumerable<string> blobNames,
            string? policyName,
            CancellationToken cancellationToken = default) where TResponse : ReturnProtectedUrlOption, new()
        {
            if (blobNames == null)
            {
                throw new ArgumentNullException(nameof(blobNames));
            }

            var containerExists = await ContainerChecksAsync(containerName, true, cancellationToken).ConfigureAwait(false);

            if (containerExists)
            {
                var containerClient = BlobServiceClient.GetBlobContainerClient(containerName);

                if (containerClient is null)
                {
                    throw BlobContainerNameInvalidException.UnableToFindWithContainerName(
                        FormatProvider,
                        containerName);
                }

                return GetSomeBlobs<TResponse>(containerClient, blobNames, policyName);
            }

            throw new BlobContainerDoesNotExistException(containerName, FormatProvider);
        }

        /// <summary>
        /// Gets all the blob bytes from the collection of blob names.
        /// </summary>
        /// <param name="containerClient">Client of the container which blobs are located.</param>
        /// <param name="blobNames">A collection of blob names you are wanting to return.</param>
        /// <param name="policyName">The name of the security policy that the container ir protected with.</param>
        /// <typeparam name="TResponse">The return option which you want the blob to be returned in.</typeparam>
        /// <returns>Dictionary of Blobs, where the key is the name of the blob and the value is the blob itself.</returns>
        private static IDictionary<string, string> GetSomeBlobs<TResponse>(
            BlobContainerClient containerClient,
            IEnumerable<string> blobNames,
            string? policyName) where TResponse : ReturnProtectedUrlOption, new()
        {
            var blobBytesDictionary = new Dictionary<string, string>();

            foreach (var blobName in blobNames)
            {
                var result = ProcessProtectedBlob<TResponse>(containerClient, blobName, policyName);

                blobBytesDictionary.Add(blobName, result);
            }

            return blobBytesDictionary;
        }

        /// <summary>
        /// Returns all the blob within a container within the storage account that are protected with a SAS token.
        /// </summary>
        /// <param name="containerName">The name of the container where the blobs you want to return are stored in.</param>
        /// <param name="policyName">The name of the security policy for the container.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TResponse">The return option which you want the blob to be returned in.</typeparam>
        /// <returns>A collection of <see cref="IBlobReturnOption{T}"/> which has been configured by the generic arguments.</returns>
        /// <exception cref="BlobContainerDoesNotExistException">
        /// Exception thrown when configuration is not set to create a new container and the container specified does
        /// not exist.
        /// </exception>
        public async Task<IDictionary<string, string>> GetAllAsync<TResponse>(
            string containerName,
            string? policyName,
            CancellationToken cancellationToken = default)
            where TResponse : ReturnProtectedUrlOption, new()
        {
            var containers = BlobServiceClient.GetBlobContainers();
            var containerExists = containers.Any(container => container.Name == containerName);

            if (containerExists)
            {
                var containerClient = BlobServiceClient.GetBlobContainerClient(containerName);

                return await GetAllBlobsAsync<TResponse>(containerClient, policyName, cancellationToken).ConfigureAwait(false);
            }

            throw new BlobContainerDoesNotExistException(containerName, FormatProvider);
        }

        private static async Task<Dictionary<string, string>> GetAllBlobsAsync<TResponse>(
            BlobContainerClient containerClient,
            string? policyName,
            CancellationToken cancellationToken)
            where TResponse : ReturnProtectedUrlOption, new()
        {
            var blobBytesDictionary = new Dictionary<string, string>();
            var blobs = new List<BlobItem>();

            await foreach (var blob in containerClient.GetBlobsAsync(cancellationToken: cancellationToken).ConfigureAwait(false))
            {
                blobs.Add(blob);
            }

            foreach (var blob in blobs)
            {
                var result = ProcessProtectedBlob<TResponse>(containerClient, blob.Name, policyName);

                blobBytesDictionary.Add(blob.Name, result);
            }

            return blobBytesDictionary;
        }

        private static string ProcessProtectedBlob<TResponse>(
            BlobContainerClient containerClient,
            string blobName,
            string? policyName) where TResponse : ReturnProtectedUrlOption, new()
        {
            var blobClient = containerClient.GetBlobClient(blobName);
            var blobSasTokenUri = GetServiceSasUriForBlob(blobClient, policyName);

            _ = blobSasTokenUri ??
                throw BlobClientUnauthorisedException.UnableToGenerateSasToken(containerClient.Name, FormatProvider);

            var blobBytes = Array.Empty<byte>();

            return new TResponse().Parse(blobName, blobBytes, blobSasTokenUri);
        }

        private static Uri? GetServiceSasUriForBlob(BlobClient blobClient, string? storedPolicyName)
        {
            // Check whether this BlobClient object has been authorized with Shared Key.
            if (blobClient.CanGenerateSasUri)
            {
                // Create a SAS token that's valid for one hour.
                var sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                    BlobName = blobClient.Name,
                    Resource = "b"
                };

                if (storedPolicyName == null)
                {
                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(1);
                    sasBuilder.SetPermissions(BlobSasPermissions.Read |
                                              BlobSasPermissions.Write);
                }
                else
                {
                    sasBuilder.Identifier = storedPolicyName;
                }

                return blobClient.GenerateSasUri(sasBuilder);
            }

            return null;
        }
    }
}