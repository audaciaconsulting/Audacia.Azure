using Audacia.Azure.BlobStorage.Common.Commands;

namespace Audacia.Azure.BlobStorage.AddBlob.Commands
{
    public abstract class BaseAddBlobStorageCommand : BaseBlobCommand
    {
        /// <summary>
        /// Gets a value indicating whether or not the container already exist.
        /// </summary>
        public bool DoesContainerExist { get; }

        internal BaseAddBlobStorageCommand(string containerName, string blobName, bool doesContainerExist = true) : base(
            containerName, blobName)
        {
            DoesContainerExist = doesContainerExist;
        }
    }
}
