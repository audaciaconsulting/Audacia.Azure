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
        /// Constructor for creating a command used for add a blob which has it's data in base 64 format.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobName"></param>
        /// <param name="blobData"></param>
        /// <param name="doesContainerExist"></param>
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
