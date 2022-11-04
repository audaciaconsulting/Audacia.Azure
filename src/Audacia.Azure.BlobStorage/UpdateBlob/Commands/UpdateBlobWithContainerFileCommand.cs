namespace Audacia.Azure.BlobStorage.UpdateBlob.Commands;

/// <summary>
/// Command used for creating a new container and updating a blob which has it's data stored on the file system.
/// </summary>
public class UpdateBlobWithContainerFileCommand : BaseUpdateBlobStorageCommand
{
    /// <summary>
    /// Gets the full file path to the location of the file which you want to upload.
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    /// Command used for creating a new container and updating a blob which has it's data stored on the file system.
    /// </summary>
    /// <param name="containerName">Name of the container where the blob is been updated.</param>
    /// <param name="blobName">Name of the blob which is been updated.</param>
    /// <param name="filePath">Location of the file on the file system.</param>
    public UpdateBlobWithContainerFileCommand(
        string containerName,
        string blobName, 
        string filePath) : base(containerName, blobName, true)
    {
        FilePath = filePath;
    }
}
