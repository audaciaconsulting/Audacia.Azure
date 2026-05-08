namespace Audacia.Azure.Demo.Models.Requests.Blob;

public class DeleteBlobRequest
{
    public string ContainerName { get; set; } = default!;

    public string BlobName { get; set; } = default!;
}
