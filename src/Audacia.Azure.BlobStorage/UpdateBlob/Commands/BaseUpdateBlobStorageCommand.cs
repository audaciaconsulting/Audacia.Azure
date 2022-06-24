using Audacia.Azure.BlobStorage.Common.Commands;

namespace Audacia.Azure.BlobStorage.UpdateBlob.Commands
{
    public abstract class BaseUpdateBlobStorageCommand : BaseBlobCommand
    {
        /// <summary>
        /// Gets a value indicating whether gets whether or not the container already exist.
        /// </summary>
        public bool DoesContainerExist { get; }

        protected BaseUpdateBlobStorageCommand(string containerName, string blobName, bool doesContainerExist = true) : base(
            containerName, blobName)
        {
            DoesContainerExist = doesContainerExist;
        }
    }
}
