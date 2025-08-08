using Warehouse.Server.Models;

namespace Warehouse.Server.Services.Interfaces;

public interface IMeasureUnitService
{
    public Task<List<MeasureUnit>> GetActiveAsync();
    public Task<List<MeasureUnit>> GetArchievedAsync();
    public Task AddAsync(string name);
    public Task RemoveAsync(string id);
    public Task UpdateRangeAsync(List<MeasureUnit> clients);
}
