using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Warehouse.Server.Models;
using Warehouse.Server.Services;
using Warehouse.Server.Services.Interfaces;

namespace Warehouse.Server.Controllers;

[Route("api/resource")]
[ApiController]
public class ResourceController : ControllerBase
{
    private readonly IResourceService _resourceService;
    private readonly ILogger<ResourceController> _logger;
    public ResourceController(IResourceService resourceService, ILogger<ResourceController> logger)
    {
        _resourceService = resourceService;
        _logger = logger;
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        try
        {
            return Ok(await _resourceService.GetActiveAsync());
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
            return Ok(await _resourceService.GetArchievedAsync());
        }
        catch (Exception ex)
        {
            return LogError(ex);
        }
    }

    [HttpPost("add/{name}")]
    public async Task<IActionResult> Add(string name)
    {
        try
        {
            await _resourceService.AddAsync(name);
            return Ok();
        }
        catch (Exception ex)
        {
            return LogError(ex);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] List<Resource> resources)
    {
        try
        {
            await _resourceService.UpdateRangeAsync(resources);
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
            await _resourceService.RemoveAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return LogError(ex);
        }
    }

    private IActionResult LogError(Exception exception)
    {
        _logger.LogError($"Resource Controller Error: {exception.Message}");

        if (exception.InnerException is SqlException sqlException)
            if (sqlException.ErrorCode == 2627)
                return BadRequest("Нарушение уникальности данных");

        return BadRequest(exception.Message);
    }
}
