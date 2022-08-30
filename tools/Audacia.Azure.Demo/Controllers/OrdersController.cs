using Audacia.Azure.Demo.Models.Requests.Queue;
using Audacia.Azure.StorageQueue.AddMessageToQueue;
using Audacia.Azure.StorageQueue.DeleteMessageFromQueue;
using Audacia.Azure.StorageQueue.GetMessages;
using Audacia.Azure.StorageQueue.GetMessages.Commands;
using Audacia.Azure.StorageQueue.Models;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Mvc;

namespace Audacia.Azure.Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IGetAzureQueueStorageService _getAzureQueueStorageService;

        private readonly IAddAzureQueueStorageService _addAzureQueueStorageService;

        private readonly IDeleteAzureQueueStorageService _deleteAzureQueueStorageService;

        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IGetAzureQueueStorageService getAzureQueueStorageService,
            IAddAzureQueueStorageService addAzureQueueStorageService,
            IDeleteAzureQueueStorageService deleteAzureQueueStorageService,
            ILogger<OrdersController> logger)
        {
            _getAzureQueueStorageService = getAzureQueueStorageService;
            _addAzureQueueStorageService = addAzureQueueStorageService;
            _deleteAzureQueueStorageService = deleteAzureQueueStorageService;
            _logger = logger;
        }

        [HttpGet, Route("Queue/{queueName}")]
        [ProducesResponseType(typeof(AzureQueueStorageMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string queueName)
        {
            var command = new GetMessageStorageQueueCommand(queueName);
            var messages = await _getAzureQueueStorageService.GetAsync(command);

            return Ok(messages);
        }

        [HttpGet, Route("Queue/GetAll/{queueName}")]
        [ProducesResponseType(typeof(IEnumerable<AzureQueueStorageMessage>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(string queueName)
        {
            var command = new GetMessagesStorageQueueCommand(queueName, 10);
            var messages = await _getAzureQueueStorageService.GetSomeAsync(command);

            return Ok(messages);
        }

        [HttpPost, Route("Queue/Add")]
        [ProducesResponseType(typeof(SendReceipt), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromForm] AddQueueRequest request)
        {
            var messages = await _addAzureQueueStorageService.ExecuteAsync(request.QueueName, request.Message);

            return Ok(messages);
        }

        [HttpDelete, Route("Queue/{queueName}/{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string queueName, string id)
        {
            var result = await _deleteAzureQueueStorageService.ExecuteAsync(queueName, id);

            return Ok(result);
        }
    }
}
