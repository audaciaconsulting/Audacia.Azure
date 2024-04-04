using Audacia.Azure.BlobStorage.UpdateBlob.Commands;

namespace Audacia.Azure.BlobStorage.UpdateBlob
{
    /// <summary>
    /// Interface for updating existing blobs stored within an Azure Storage account.
    /// </summary>
    public interface IUpdateAzureBlobStorageService
    {
        /// <summary>
        /// Update an existing blob with the file data from the file stored at <paramref name="command.FilePath"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A bool depending on the success of the upload.</returns>
        Task<bool> ExecuteAsync(UpdateBlobStorageFileCommand command, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update an existing blob with the file data contained within the <paramref name="command.FileData"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A bool depending on the success of the upload.</returns>
        Task<bool> ExecuteAsync(UpdateBlobStorageBytesCommand command, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update an existing blob with the file data contained within the <paramref name="command.FileData"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A bool depending on the success of the upload.</returns>
        Task<bool> ExecuteAsync(UpdateBlobStorageBaseSixtyFourCommand command, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update an existing blob with the file data contained within the <paramref name="command.FileData"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A bool depending on the success of the upload.</returns>
        Task<bool> ExecuteAsync(UpdateBlobStorageStreamCommand command, CancellationToken cancellationToken = default);
    }
}
