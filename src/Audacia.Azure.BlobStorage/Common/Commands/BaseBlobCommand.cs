namespace Audacia.Azure.BlobStorage.Common.Commands
{
    /// <summary>
    /// Base command for all Azure blob services.
    /// </summary>
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

        /// <summary>
        /// Base command for all Azure blob services.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobName">Name of the blob.</param>
        internal BaseBlobCommand(string containerName, string blobName)
        {
            ContainerName = containerName;
            BlobName = blobName;
        }
    }
}
