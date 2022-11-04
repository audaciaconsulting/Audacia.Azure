using Audacia.Azure.BlobStorage.Common.Commands;

namespace Audacia.Azure.BlobStorage.UpdateBlob.Commands
{
    /// <summary>
    /// Base update command for Azure Blob Storage.
    /// </summary>
    public abstract class BaseUpdateBlobStorageCommand : BaseBlobCommand
    {
        /// <summary>
        /// Gets a value indicating whether gets whether or not the container already exist.
        /// </summary>
        public bool DoesContainerExist { get; }

        /// <summary>
        /// Base update command for Azure Blob Storage.
        /// </summary>
        /// <param name="containerName">Name of the container where the blob is been updated.</param>
        /// <param name="blobName">Name of the blob which is been updated.</param>
        /// <param name="doesContainerExist">Whether the container exists.</param>
#pragma warning disable AV1564
        protected BaseUpdateBlobStorageCommand(string containerName, string blobName, bool doesContainerExist) : base(
#pragma warning restore AV1564
            containerName, blobName)
        {
            DoesContainerExist = doesContainerExist;
        }
    }
}
