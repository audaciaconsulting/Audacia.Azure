namespace Audacia.Azure.Demo.Models.Requests;

public class UpdateBlobRequest
{
    public string ContainerName { get; set; } = default!;
    
    public string BlobName { get; set; } = default!;
    
    public IFormFile File { get; set; } = default!;
}
