namespace Audacia.Azure.BlobStorage.UpdateBlob.Commands
{
    /// <summary>
    /// Command used for adding a blob which has it's data in an enumerable of bytes.
    /// </summary>
    public class UpdateAzureBlobStorageBytesCommand : BaseUpdateBlobStorageCommand
    {
        /// <summary>
        /// Gets Array fo bytes which contains the data of the information which you want to upload as a blob.
        /// </summary>
        public IEnumerable<byte> BlobData { get; }

        /// <summary>
        /// Command used for adding a blob which has it's data in an enumerable of bytes.
        /// </summary>
        /// <param name="containerName">Name of the container where the blob is been updated.</param>
        /// <param name="blobName">Name of the blob which is been updated.</param>
        /// <param name="blobData">Data of the blob.</param>
        /// <param name="doesContainerExist">Whether the container exists.</param>
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
