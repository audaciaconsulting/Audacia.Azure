namespace Audacia.Azure.BlobStorage.AddBlob.Commands
{
    /// <summary>
    /// Command used for adding a blob which has it's data in base 64 format.
    /// </summary>
    public class AddAzureBlobStorageBaseSixtyFourCommand : BaseAddBlobStorageCommand
    {
        /// <summary>
        /// Gets base 64 string containing the information of the blob you want to upload.
        /// </summary>
        public string BlobData { get; }

        /// <summary>
        /// Command used for adding a blob which has it's data in base 64 format.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobName">Name of the blob.</param>
        /// <param name="blobData">Data of the blob.</param>
        /// <param name="doesContainerExist">Whether the container exists for the blob to be added too.</param>
        public AddAzureBlobStorageBaseSixtyFourCommand(
            string containerName,
            string blobName,
            string blobData,
            bool doesContainerExist = true) : base(containerName, blobName, doesContainerExist)
        {
            BlobData = blobData;
        }
    }
}
