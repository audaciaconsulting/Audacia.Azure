using Audacia.Azure.BlobStorage.AddBlob;
using Audacia.Azure.BlobStorage.AddBlob.Commands;
using Audacia.Azure.BlobStorage.DeleteBlob;
using Audacia.Azure.BlobStorage.DeleteBlob.Commands;
using Audacia.Azure.BlobStorage.GetBlob;
using Audacia.Azure.BlobStorage.UpdateBlob;
using Audacia.Azure.BlobStorage.UpdateBlob.Commands;
using Audacia.Azure.Common.ReturnOptions.ImageOption;
using Audacia.Azure.Demo.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Audacia.Azure.Demo.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IGetAzureBlobStorageService _getAzureBlobStorageService;

    private readonly IAddAzureBlobStorageService _addAzureBlobStorageService;

    private readonly IUpdateAzureBlobStorageService _updateAzureBlobStorageService;

    private readonly IDeleteAzureBlobStorageService _deleteAzureBlobStorageService;

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(IGetAzureBlobStorageService getAzureBlobStorageService,
        IAddAzureBlobStorageService addAzureBlobStorageService,
        IUpdateAzureBlobStorageService updateAzureBlobStorageService,
        IDeleteAzureBlobStorageService deleteAzureBlobStorageService,
        ILogger<WeatherForecastController> logger)
    {
        _getAzureBlobStorageService = getAzureBlobStorageService;
        _addAzureBlobStorageService = addAzureBlobStorageService;
        _updateAzureBlobStorageService = updateAzureBlobStorageService;
        _deleteAzureBlobStorageService = deleteAzureBlobStorageService;

        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IDictionary<string, string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(string containerName)
    {
        try
        {
            var blobs = await _getAzureBlobStorageService.GetAllAsync<string, ReturnUrlOption>(containerName);

            return Ok(blobs);
        }
#pragma warning disable CA1031
        catch (Exception e)
#pragma warning restore CA1031
        {
            _logger.LogError(e.Message, e);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromForm] AddBlobRequest addBlobRequest)
    {
        _ = addBlobRequest ?? throw new ArgumentNullException(nameof(addBlobRequest));
        
        var fileExtension = addBlobRequest.File.FileName.Split('.');
        var uniqueBlobName = $"{Guid.NewGuid().ToString()}.{fileExtension[^1]}";

        await using var fileStream = addBlobRequest.File.OpenReadStream();

        var command = new AddBlobStreamCommand(addBlobRequest.ContainerName, uniqueBlobName, fileStream);

        var addBlobResult = await _addAzureBlobStorageService.ExecuteAsync(command);

        if (addBlobResult)
        {
            return Ok(uniqueBlobName);
        }

        return BadRequest("Failed to add blob");
    }

    [HttpPut]
    [Route("/bytes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put([FromForm] UpdateBlobRequest updateBlobRequest)
    {
        _ = updateBlobRequest ?? throw new ArgumentNullException(nameof(updateBlobRequest));
        
        byte[] fileBytes;
        await using (var ms = new MemoryStream())
        {
            await updateBlobRequest.File.CopyToAsync(ms);

            fileBytes = ms.ToArray();
        }

        var command = new UpdateBlobStorageBytesCommand(updateBlobRequest.ContainerName,
            updateBlobRequest.BlobName,
            fileBytes);

        var updateBlobResult = await _updateAzureBlobStorageService.ExecuteAsync(command);

        if (updateBlobResult)
        {
            return Ok();
        }

        return BadRequest("Failed to update blob");
    }

    [HttpPut]
    [Route("/base64")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutWithBase64([FromForm] UpdateBlobRequest updateBlobRequest)
    {
        _ = updateBlobRequest ?? throw new ArgumentNullException(nameof(updateBlobRequest));

        byte[] fileBytes;
        await using (var ms = new MemoryStream())
        {
            await updateBlobRequest.File.CopyToAsync(ms);

            fileBytes = ms.ToArray();
        }
        String fileBase64 = Convert.ToBase64String(fileBytes);

        var command = new UpdateBlobStorageBaseSixtyFourCommand(updateBlobRequest.ContainerName,
            updateBlobRequest.BlobName,
            fileBase64);

        var updateBlobResult = await _updateAzureBlobStorageService.ExecuteAsync(command);

        if (updateBlobResult)
        {
            return Ok();
        }

        return BadRequest("Failed to update blob");
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromForm] DeleteBlobRequest deleteBlobRequest)
    {
        _ = deleteBlobRequest ?? throw new ArgumentNullException(nameof(deleteBlobRequest));
        var command = new DeleteAzureBlobStorageCommand(deleteBlobRequest.ContainerName, deleteBlobRequest.BlobName);

        var deleteBlobResult = await _deleteAzureBlobStorageService.ExecuteAsync(command);

        if (deleteBlobResult)
        {
            return Ok();
        }

        return BadRequest("Failed to delete blob");
    }
}
