namespace Audacia.Azure.Demo.Models.Requests.Blob;

public class AddBlobRequest
{
    public string ContainerName { get; set; } = default!;

    public IFormFile File { get; set; } = default!;
}
