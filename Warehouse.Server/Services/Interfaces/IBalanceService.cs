using Warehouse.Server.Models;
using Warehouse.Server.Models.DTO;

namespace Warehouse.Server.Services.Interfaces;

public interface IBalanceService
{
    public Task AddRangeAsync(List<DocumentResourceDTO> resources);
    public Task RemoveRangeAsync(List<DocumentResourceDTO> resources);
    public Task<List<Balance>> GetAllBalancesAsync(BalanceFilterDTO filter);
    public Task<List<Balance>> GetAllBalancesAsync();
}
