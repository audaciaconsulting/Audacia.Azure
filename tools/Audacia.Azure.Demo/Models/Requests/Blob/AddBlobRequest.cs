namespace Audacia.Azure.Demo.Models.Requests
{
    public class AddBlobRequest
    {
        public string ContainerName { get; set; } = default!;
        
        public IFormFile File { get; set; } = default!;
    }
}
