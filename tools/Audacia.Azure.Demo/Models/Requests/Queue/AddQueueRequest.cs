using Microsoft.AspNetCore.Http;

namespace Audacia.Azure.Demo.Models.Requests.Queue
{
    public class AddQueueRequest
    {
        public string QueueName { get; set; }

        public string Message { get; set; }
    }
}
