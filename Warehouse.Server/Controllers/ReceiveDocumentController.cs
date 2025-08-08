using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Warehouse.Server.Models.DTO;
using Warehouse.Server.Services.Interfaces;

namespace Warehouse.Server.Controllers;

[Route("api/receive-document")]
[ApiController]
public class ReceiveDocumentController : ControllerBase
{
    private readonly IReceiveDocumentService _documentService;
    private readonly ILogger<ReceiveDocumentController> _logger;

    public ReceiveDocumentController(IReceiveDocumentService documentService, ILogger<ReceiveDocumentController> logger)
    {
        _documentService = documentService;
        _logger = logger;
    }

    [HttpPost("filter")]
    public async Task<IActionResult> GetAllDocuments([FromBody] DocumentFilterDTO filter)
    {
        try
        {
            return Ok(await _documentService.GetAllAsync(filter));
        }
        catch (Exception ex)
        {
            return LogError(ex);
        }
    }

    [HttpGet("numbers")]
    public async Task<IActionResult> GetAllNumbers()
    {
        try
        {
            return Ok(await _documentService.GetAllNumbersAsync());
        }
        catch (Exception ex)
        {
            return LogError(ex);
        }
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddDocument([FromBody] DocumentDTO document)
    {
        try
        {
            await _documentService.AddAsync(document);
            return Ok();
        }
        catch (Exception ex)
        {
            return LogError(ex);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocument(string id)
    {
        try
        {
            return Ok(await _documentService.GetAsync(id));
        }
        catch (Exception ex)
        {
            return LogError(ex);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDocument(string id)
    {
        try
        {
            await _documentService.RemoveAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return LogError(ex);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDocument([FromBody] DocumentDTO document)
    {
        try
        {
            await _documentService.UpdateAsync(document);
            return Ok();
        }
        catch (Exception ex)
        {
            return LogError(ex);
        }
    }

    private BadRequestObjectResult LogError(Exception exception)
    {
        _logger.LogError($"Receive Document Controller Error: {exception.Message}");

        if (exception.InnerException is SqlException sqlException)
            if (sqlException.ErrorCode == 2627)
                return BadRequest("Нарушение уникальности данных");

        return BadRequest(exception.Message);
    }
}
