namespace Audacia.Azure.BlobStorage.AddBlob.Commands
{
    /// <summary>
    /// Command used for adding a blob which has it's data in an stream.
    /// </summary>
    public class AddAzureBlobStorageStreamCommand : BaseAddBlobStorageCommand
    {
        /// <summary>
        /// Gets stream of the blob which you want to upload to storage.
        /// </summary>
        public Stream BlobData { get; }

        /// <summary>
        /// Command used for adding a blob which has it's data in an stream.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobName">Name of the blob.</param>
        /// <param name="blobData">Data of the blob.</param>
        /// <param name="doesContainerExist">Whether the container exists for the blob to be added too.</param>
        public AddAzureBlobStorageStreamCommand(string containerName, string blobName, Stream blobData,
            bool doesContainerExist = true) : base(containerName, blobName, doesContainerExist)
        {
            BlobData = blobData;
        }
    }
}
