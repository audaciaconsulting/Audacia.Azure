using Audacia.Azure.BlobStorage.DeleteBlob.Commands;

namespace Audacia.Azure.BlobStorage.DeleteBlob
{
    /// <summary>
    /// Interface for removing blobs from an Azure Storage account.
    /// </summary>
    public interface IDeleteAzureBlobStorageService
    {
        /// <summary>
        /// Removes a blob with the <paramref name="command.BlobName"/> within <paramref name="command.ContainerName"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to remove a blob.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Whether the removing of the blob was successful.</returns>
        Task<bool> ExecuteAsync(DeleteAzureBlobStorageCommand command, CancellationToken cancellationToken = default);
    }
}