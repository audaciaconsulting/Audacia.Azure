namespace Audacia.Azure.BlobStorage.AddBlob.Commands
{
    public class AddAzureBlobStorageBytesCommand : BaseAddBlobStorageCommand
    {
        /// <summary>
        /// Gets array fo bytes which contains the data of the information which you want to upload as a blob.
        /// </summary>
#pragma warning disable CA1819
        public byte[] BlobData { get; }
#pragma warning restore CA1819

        public AddAzureBlobStorageBytesCommand(string containerName, string blobName, byte[] blobData,
            bool doesContainerExist = true) : base(containerName, blobName, doesContainerExist)
        {
            BlobData = blobData;
        }
    }
}
