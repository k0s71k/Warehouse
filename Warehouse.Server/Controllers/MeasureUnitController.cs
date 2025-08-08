using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Warehouse.Server.Models;
using Warehouse.Server.Services.Interfaces;

namespace Warehouse.Server.Controllers;

[Route("api/measure-unit")]
[ApiController]
public class MeasureUnitController : ControllerBase
{
    private readonly IMeasureUnitService _measureUnitService;
    private readonly ILogger<MeasureUnitController> _logger;
    public MeasureUnitController(IMeasureUnitService measureUnitService, ILogger<MeasureUnitController> logger)
    {
        _measureUnitService = measureUnitService;
        _logger = logger;
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        try
        {
            return Ok(await _measureUnitService.GetActiveAsync());
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
            return Ok(await _measureUnitService.GetArchievedAsync());
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
            await _measureUnitService.AddAsync(name);
            return Ok();
        }
        catch (Exception ex)
        {
            return LogError(ex);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] List<MeasureUnit> measureUnits)
    {
        try
        {
            await _measureUnitService.UpdateRangeAsync(measureUnits);
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
            await _measureUnitService.RemoveAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return LogError(ex);
        }
    }

    private BadRequestObjectResult LogError(Exception exception)
    {
        _logger.LogError($"MeasureUnits Controller Error: {exception.Message}");

        if (exception.InnerException is SqlException sqlException)
            if (sqlException.ErrorCode == 2627)
                return BadRequest("Нарушение уникальности данных");

        return BadRequest(exception.Message);
    }
}
