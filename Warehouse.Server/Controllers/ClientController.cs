using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Warehouse.Server.Models;
using Warehouse.Server.Models.DTO;
using Warehouse.Server.Services.Interfaces;

namespace Warehouse.Server.Controllers;

[Route("api/client")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly ILogger<ClientController> _logger;

    public ClientController(IClientService clientService, ILogger<ClientController> logger)
    {
        _clientService = clientService;
        _logger = logger;
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        try
        {
            return Ok(
                await _clientService.GetActiveAsync());
        }
        catch (Exception ex)
        {
            return LogError(ex);
        }
    }

    [HttpGet("archieved")]
    public async Task<IActionResult> GetArchieved()
    {
        try
        {
            return Ok(
                await _clientService.GetArchievedAsync());
        }
        catch (Exception ex)
        {
            return LogError(ex);
        }
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] ClientAddDTO client)
    {
        try
        {
            await _clientService.AddAsync(client.Name, client.Address);
            return Ok();
        }
        catch (Exception ex)
        {
            return LogError(ex);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] List<Client> clients)
    {
        try
        {
            await _clientService.UpdateRangeAsync(clients);
            return Ok();
        }
        catch (Exception ex)
        {
            return LogError(ex);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _clientService.RemoveAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return LogError(ex);
        }
    }

    private BadRequestObjectResult LogError(Exception exception)
    {
        _logger.LogError($"Client Controller Error: {exception.Message}");

        if (exception.InnerException is SqlException sqlException)
            if (sqlException.ErrorCode == 2627)
                return BadRequest("Нарушение уникальности данных");

        return BadRequest(exception.Message);
    }
}
