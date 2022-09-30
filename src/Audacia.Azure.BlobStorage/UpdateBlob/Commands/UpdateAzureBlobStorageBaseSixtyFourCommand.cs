namespace Audacia.Azure.BlobStorage.UpdateBlob.Commands
{
    /// <summary>
    /// Command used for updating a blob which has it's data in base 64 format.
    /// </summary>
    public class UpdateAzureBlobStorageBaseSixtyFourCommand : BaseUpdateBlobStorageCommand
    {
        /// <summary>
        /// Gets Base 64 string containing the information of the blob you want to upload.
        /// </summary>
        public string BlobData { get; }

        /// <summary>
        /// Command used for updating a blob which has it's data in base 64 format.
        /// </summary>
        /// <param name="containerName">Name of the container where the blob is been updated.</param>
        /// <param name="blobName">Name of the blob which is been updated.</param>
        /// <param name="blobData">Data of the blob.</param>
        /// <param name="doesContainerExist">Whether the container exists.</param>
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
