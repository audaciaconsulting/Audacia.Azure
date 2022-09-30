using Audacia.Azure.BlobStorage.Common.Commands;

namespace Audacia.Azure.BlobStorage.AddBlob.Commands
{
    /// <summary>
    /// Base command for all add blob storage commands.
    /// </summary>
    public abstract class BaseAddBlobStorageCommand : BaseBlobCommand
    {
        /// <summary>
        /// Gets a value indicating whether or not the container already exist.
        /// </summary>
        public bool DoesContainerExist { get; }

        /// <summary>
        /// Base command for all add blob storage commands.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobName">Name of the blob.</param>
        /// <param name="doesContainerExist">Whether the container which the blob is been added too exist.</param>
        internal BaseAddBlobStorageCommand(
            string containerName,
            string blobName,
            bool doesContainerExist = true) : base(containerName, blobName)
        {
            DoesContainerExist = doesContainerExist;
        }
    }
}
