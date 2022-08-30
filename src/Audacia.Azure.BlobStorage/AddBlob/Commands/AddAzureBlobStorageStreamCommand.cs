namespace Audacia.Azure.BlobStorage.AddBlob.Commands
{
    public class AddAzureBlobStorageStreamCommand : BaseAddBlobStorageCommand
    {
        public string ContentType { get; }

        /// <summary>
        /// Gets stream of the blob which you want to upload to storage.
        /// </summary>
        public Stream BlobData { get; }

        public AddAzureBlobStorageStreamCommand(string containerName, string blobName, Stream blobData,
            bool doesContainerExist = true) : base(containerName, blobName, doesContainerExist)
        {
            BlobData = blobData;
        }
    }
}
