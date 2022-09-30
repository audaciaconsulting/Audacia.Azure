namespace Audacia.Azure.BlobStorage.AddBlob.Commands
{
    /// <summary>
    /// Command used for adding a blob which has it's data in an enumerable of bytes.
    /// </summary>
    public class AddAzureBlobStorageBytesCommand : BaseAddBlobStorageCommand
    {
        /// <summary>
        /// Gets array fo bytes which contains the data of the information which you want to upload as a blob.
        /// </summary>
        public IEnumerable<byte> BlobData { get; }

        /// <summary>
        /// Command used for adding a blob which has it's data in an enumerable of bytes.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobName">Name of the blob.</param>
        /// <param name="blobData">Data of the blob.</param>
        /// <param name="doesContainerExist">Whether the container exists for the blob to be added too.</param>
        public AddAzureBlobStorageBytesCommand(string containerName, string blobName, byte[] blobData,
            bool doesContainerExist = true) : base(containerName, blobName, doesContainerExist)
        {
            BlobData = blobData;
        }
    }
}
