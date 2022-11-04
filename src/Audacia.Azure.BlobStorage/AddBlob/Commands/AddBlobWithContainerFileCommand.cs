namespace Audacia.Azure.BlobStorage.AddBlob.Commands;

/// <summary>
/// Command used for adding a container and a blob which has it's data stored on the file system.
/// </summary>
public class AddBlobWithContainerFileCommand : BaseAddBlobStorageCommand
{
    /// <summary>
    /// Gets the full file path to the location of the file which you want to upload.
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    /// Command used for adding a container and a blob which has it's data stored on the file system.
    /// </summary>
    /// <param name="containerName">Name of the container.</param>
    /// <param name="blobName">Name of the blob.</param>
    /// <param name="filePath">Location of the file on the file system.</param>
    public AddBlobWithContainerFileCommand(
        string containerName, 
        string blobName, 
        string filePath) : base(containerName, blobName, false)
    {
        FilePath = filePath;
    }
}
