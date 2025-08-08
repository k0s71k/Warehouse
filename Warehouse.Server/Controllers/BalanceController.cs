using Microsoft.AspNetCore.Mvc;
using Warehouse.Server.Models.DTO;
using Warehouse.Server.Services.Interfaces;

namespace Warehouse.Server.Controllers;

[Route("api/balance")]
[ApiController]
public class BalanceController : ControllerBase
{
    private readonly IBalanceService _balanceService;
    private readonly ILogger<BalanceController> _logger;
    public BalanceController(IBalanceService balanceService, ILogger<BalanceController> logger)
    {
        _balanceService = balanceService;
        _logger = logger;
    }

    [HttpPost("filter")]
    public async Task<IActionResult> GetFilteredBalances([FromBody] BalanceFilterDTO filter)
    {
        try
        {
            return Ok(await _balanceService.GetAllBalancesAsync(filter));
        }
        catch (Exception ex)
        {
            return LogError(ex);
        }
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllBalances()
    {
        try
        {
            return Ok(await _balanceService.GetAllBalancesAsync());
        }
        catch (Exception ex)
        {
            return LogError(ex);
        }
    }

    private IActionResult LogError(Exception ex)
    {
        _logger.LogCritical($"Balance controller error: {ex.Message}");
        return BadRequest(ex.Message);
    }
}


