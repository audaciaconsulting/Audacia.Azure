using Audacia.Azure.BlobStorage.Common.Commands;

namespace Audacia.Azure.BlobStorage.DeleteBlob.Commands
{
    /// <summary>
    /// Command for removing a blob from Azure Blob Storage.
    /// </summary>
    public class DeleteAzureBlobStorageCommand : BaseBlobCommand
    {
        /// <summary>
        /// Command for removing a blob from Azure Blob Storage.
        /// </summary>
        /// <param name="containerName">Name of the container which the blob resides in.</param>
        /// <param name="blobName">Name of the blob to be removed.</param>
        public DeleteAzureBlobStorageCommand(string containerName, string blobName) : base(containerName, blobName)
        {
        }
    }
}
