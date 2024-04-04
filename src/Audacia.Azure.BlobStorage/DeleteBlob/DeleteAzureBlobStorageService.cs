using Audacia.Azure.BlobStorage.Common.Services;
using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.BlobStorage.DeleteBlob.Commands;
using Audacia.Azure.BlobStorage.Exceptions;
using Azure;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.BlobStorage.DeleteBlob
{
    /// <summary>
    /// Delete service for removing blobs from an Azure Blob Storage account.
    /// </summary>
    public class DeleteAzureBlobStorageService : BaseAzureBlobStorageService, IDeleteAzureBlobStorageService
    {
        /// <summary>
        /// Constructor option for when adding the <see cref="BlobServiceClient"/> has being added to the DI.
        /// </summary>
        /// <param name="logger">Logger to giving extra information.</param>
        /// <param name="blobServiceClient">Client for accessing blob's.</param>
        public DeleteAzureBlobStorageService(
            ILogger<DeleteAzureBlobStorageService> logger,
            BlobServiceClient blobServiceClient) : base(logger, blobServiceClient)
        {
        }

        /// <summary>
        /// Constructor option for using the Options pattern with <see cref="BlobStorageOption"/>.
        /// </summary>
        /// <param name="logger">Logger to giving extra information.</param>
        /// <param name="blobStorageConfig">Blob storage options containing information of where to get blobs from.</param>
        public DeleteAzureBlobStorageService(
            ILogger<DeleteAzureBlobStorageService> logger,
            IOptions<BlobStorageOption> blobStorageConfig) : base(
            logger, blobStorageConfig)
        {
        }

        /// <summary>
        /// Removes a blob with the <paramref name="command.BlobName"/> within <paramref name="command.ContainerName"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to remove a blob.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Whether the removing of the blob was successful.</returns>
        /// <exception cref="BlobDoesNotExistException">
        /// Exception thrown when the blob trying to be deleted is not within the specified container.
        /// </exception>
        /// <exception cref="ArgumentNullException">Command for service is null therefore cannot continue.</exception>
        public async Task<bool> ExecuteAsync(DeleteAzureBlobStorageCommand command, CancellationToken cancellationToken)
        {
            if (command != null)
            {
                var blobClient = GetBlobClient(command.ContainerName, command.BlobName);

                var blobExists = await blobClient.ExistsAsync(cancellationToken).ConfigureAwait(false);

                if (blobExists.Value)
                {
                    return await DeleteBlobAsync(blobClient, command, cancellationToken).ConfigureAwait(false);
                }

                throw new BlobDoesNotExistException(command.BlobName, command.ContainerName, FormatProvider);
            }

            throw new ArgumentNullException(nameof(command));
        }

        /// <summary>
        /// Returns a blob client based off an container and blob name.
        /// </summary>
        /// <param name="containerName">Name of the container where the Blob is.</param>
        /// <param name="blobName">Name of the blob within an Azure blob container.</param>
        /// <returns>Returns the blob client based off the container and blob name.</returns>
        private BlobClient GetBlobClient(string containerName, string blobName)
        {
            var containerClient = BlobServiceClient.GetBlobContainerClient(containerName);

            return containerClient.GetBlobClient(blobName);
        }

        private async Task<bool> DeleteBlobAsync(BlobClient blobClient, DeleteAzureBlobStorageCommand command, CancellationToken cancellationToken)
        {
            try
            {
                using (await blobClient.DeleteAsync(cancellationToken: cancellationToken).ConfigureAwait(false))
                {
                    Logger.LogInformation(
                        $"Deleted blob: {command.BlobName} from Container: {command.ContainerName}");
                    return true;
                }
            }
            catch (RequestFailedException requestFailedException)
            {
                Logger.LogError(requestFailedException, requestFailedException.Message);
                return false;
            }
        }
    }
}
