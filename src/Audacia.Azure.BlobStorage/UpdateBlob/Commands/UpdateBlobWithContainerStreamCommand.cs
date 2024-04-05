using System.IO;

namespace Audacia.Azure.BlobStorage.UpdateBlob.Commands
{
    /// <summary>
    /// Command used for creating a new container and updating a blob which has it's data in an stream.
    /// </summary>
    public class UpdateBlobWithContainerStreamCommand : BaseUpdateBlobStorageCommand
    {
        /// <summary>
        /// Gets stream of the blob which you want to upload to storage.
        /// </summary>
        public Stream BlobData { get; }

        /// <summary>
        /// Command used for creating a new container and updating a blob which has it's data in an stream.
        /// </summary>
        /// <param name="containerName">Name of the container where the blob is been updated.</param>
        /// <param name="blobName">Name of the blob which is been updated.</param>
        /// <param name="blobData">Data of the blob.</param>
        public UpdateBlobWithContainerStreamCommand(
            string containerName,
            string blobName,
            Stream blobData) : base(containerName, blobName, true)
        {
            BlobData = blobData;
        }
    }
}
