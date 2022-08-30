namespace Audacia.Azure.BlobStorage.UpdateBlob.Commands
{
    public class UpdateAzureBlobStorageStreamCommand : BaseUpdateBlobStorageCommand
    {
        /// <summary>
        /// Gets stream of the blob which you want to upload to storage.
        /// </summary>
        public Stream BlobData { get; }

        public UpdateAzureBlobStorageStreamCommand(
            string containerName,
            string blobName,
            Stream blobData,
            bool doesContainerExist = true) : base(containerName, blobName, doesContainerExist)
        {
            BlobData = blobData;
        }
    }
}
