namespace Audacia.Azure.BlobStorage.AddBlob.Commands
{
    public class AddAzureBlobStorageBase64Command : BaseAddBlobStorageCommand
    {
        /// <summary>
        /// Gets base 64 string containing the information of the blob you want to upload.
        /// </summary>
        public string BlobData { get; }

        public AddAzureBlobStorageBase64Command(
            string containerName,
            string blobName,
            string blobData,
            bool doesContainerExist = true) : base(containerName, blobName, doesContainerExist)
        {
            BlobData = blobData;
        }
    }
}
