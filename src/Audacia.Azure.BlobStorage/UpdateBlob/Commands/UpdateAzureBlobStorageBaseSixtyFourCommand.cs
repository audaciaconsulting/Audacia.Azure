namespace Audacia.Azure.BlobStorage.UpdateBlob.Commands
{
    public class UpdateAzureBlobStorageBaseSixtyFourCommand : BaseUpdateBlobStorageCommand
    {
        /// <summary>
        /// Gets Base 64 string containing the information of the blob you want to upload.
        /// </summary>
        public string BlobData { get; }

        public UpdateAzureBlobStorageBaseSixtyFourCommand(
            string containerName,
            string blobName,
            string blobData,
            bool doesContainerExist = true) : base(containerName, blobName, doesContainerExist)
        {
            BlobData = blobData;
        }
    }
}
