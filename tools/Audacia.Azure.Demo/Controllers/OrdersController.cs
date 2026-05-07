using Audacia.Azure.Demo.Models.Requests.Queue;
using Audacia.Azure.StorageQueue.AddMessageToQueue;
using Audacia.Azure.StorageQueue.DeleteMessageFromQueue;
using Audacia.Azure.StorageQueue.GetMessages;
using Audacia.Azure.StorageQueue.GetMessages.Commands;
using Audacia.Azure.StorageQueue.Models;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Mvc;

namespace Audacia.Azure.Demo.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(IGetAzureQueueStorageService getAzureQueueStorageService,
    IAddAzureQueueStorageService addAzureQueueStorageService,
    IDeleteAzureQueueStorageService deleteAzureQueueStorageService) : ControllerBase
{
    private readonly IGetAzureQueueStorageService _getAzureQueueStorageService = getAzureQueueStorageService;

    private readonly IAddAzureQueueStorageService _addAzureQueueStorageService = addAzureQueueStorageService;

    private readonly IDeleteAzureQueueStorageService _deleteAzureQueueStorageService = deleteAzureQueueStorageService;

    [HttpGet, Route("Queue/{queueName}")]
    [ProducesResponseType(typeof(AzureQueueStorageMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(string queueName, CancellationToken cancellationToken = default)
    {
        var command = new GetMessageStorageQueueCommand(queueName);
        var messages = await _getAzureQueueStorageService.GetAsync(command, cancellationToken);

        return Ok(messages);
    }

    [HttpGet, Route("Queue/GetAll/{queueName}")]
    [ProducesResponseType(typeof(IEnumerable<AzureQueueStorageMessage>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(string queueName, CancellationToken cancellationToken = default)
    {
        var command = new GetMessagesStorageQueueCommand(queueName, 10);
        var messages = await _getAzureQueueStorageService.GetSomeAsync(command, cancellationToken);

        return Ok(messages);
    }

    [HttpPost, Route("Queue/Add")]
    [ProducesResponseType(typeof(SendReceipt), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post([FromForm] AddQueueRequest request, CancellationToken cancellationToken = default)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var messages = await _addAzureQueueStorageService.ExecuteAsync(request.QueueName, request.Message, cancellationToken);

        return Ok(messages);
    }

    [HttpDelete, Route("Queue/{queueName}/{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(string queueName, string id, CancellationToken cancellationToken = default)
    {
        var result = await _deleteAzureQueueStorageService.ExecuteAsync(queueName, id, cancellationToken);

        return Ok(result);
    }
}
