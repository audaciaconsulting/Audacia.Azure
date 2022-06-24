namespace Audacia.Azure.BlobStorage.Common.Commands
{
    public abstract class BaseBlobCommand
    {
        /// <summary>
        /// Gets the name of the container where you want the blob to be added.
        /// </summary>
        public string ContainerName { get; }

        /// <summary>
        /// Gets the name of the blob which is going to be added to the storage account.
        /// </summary>
        public string BlobName { get; }

        internal BaseBlobCommand(string containerName, string blobName)
        {
            ContainerName = containerName;
            BlobName = blobName;
        }
    }
}
