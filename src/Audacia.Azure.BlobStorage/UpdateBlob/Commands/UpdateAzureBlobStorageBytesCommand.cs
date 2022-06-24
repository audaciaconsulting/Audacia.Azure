using System.Collections;

namespace Audacia.Azure.BlobStorage.UpdateBlob.Commands
{
    public class UpdateAzureBlobStorageBytesCommand : BaseUpdateBlobStorageCommand
    {
        /// <summary>
        /// Gets Array fo bytes which contains the data of the information which you want to upload as a blob.
        /// </summary>
#pragma warning disable CA1819
        public byte[] BlobData { get; }
#pragma warning restore CA1819

        public UpdateAzureBlobStorageBytesCommand(
            string containerName,
            string blobName,
            byte[] blobData,
            bool doesContainerExist = true) : base(containerName, blobName, doesContainerExist)
        {
            BlobData = blobData;
        }
    }
}
