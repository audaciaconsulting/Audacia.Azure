namespace Audacia.Azure.BlobStorage.AddBlob.Commands;

/// <summary>
/// Command used for adding a container and a blob which has it's data in an enumerable of bytes.
/// </summary>
public class AddBlobWithContainerBytesCommand : BaseAddBlobStorageCommand
{
    /// <summary>
    /// Gets array fo bytes which contains the data of the information which you want to upload as a blob.
    /// </summary>
    public IEnumerable<byte> BlobData { get; }

    /// <summary>
    /// Command used for adding a container and a blob which has it's data in an enumerable of bytes.
    /// </summary>
    /// <param name="containerName">Name of the container.</param>
    /// <param name="blobName">Name of the blob.</param>
    /// <param name="blobData">Data of the blob.</param>
    public AddBlobWithContainerBytesCommand(
        string containerName, 
        string blobName, 
        byte[] blobData) : base(containerName, blobName, false)
    {
        BlobData = blobData;
    }
}
