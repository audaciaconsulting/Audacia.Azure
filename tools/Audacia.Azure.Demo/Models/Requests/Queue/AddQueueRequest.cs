namespace Audacia.Azure.Demo.Models.Requests.Queue
{
    public class AddQueueRequest
    {
        public string QueueName { get; set; } = default!;

        public string Message { get; set; } = default!;
    }
}
