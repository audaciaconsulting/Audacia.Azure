namespace Audacia.Azure.BlobStorage.UpdateBlob.Commands
{
    public class UpdateAzureBlobStorageFileCommand : BaseUpdateBlobStorageCommand
    {
        /// <summary>
        /// Gets the full file path to the location of the file which you want to upload.
        /// </summary>
        public string FilePath { get; }

        public UpdateAzureBlobStorageFileCommand(string containerName, string blobName, string filePath,
            bool doesContainerExist = true) : base(containerName, blobName, doesContainerExist)
        {
            FilePath = filePath;
        }
    }
}
