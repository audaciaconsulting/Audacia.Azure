namespace Audacia.Azure.BlobStorage.AddBlob.Commands
{
    public class AddAzureBlobStorageFileCommand : BaseAddBlobStorageCommand
    {
        /// <summary>
        /// Gets the full file path to the location of the file which you want to upload.
        /// </summary>
        public string FilePath { get; }

        public AddAzureBlobStorageFileCommand(string containerName, string blobName, string filePath,
            bool doesContainerExist = true) : base(containerName, blobName, doesContainerExist)
        {
            FilePath = filePath;
        }
    }
}
