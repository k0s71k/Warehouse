using Warehouse.Server.Models;

namespace Warehouse.Server.Services.Interfaces;

public interface IResourceService
{
    public Task<List<Resource>> GetActiveAsync();
    public Task<List<Resource>> GetArchievedAsync();
    public Task AddAsync(string name);
    public Task RemoveAsync(string id);
    public Task UpdateRangeAsync(List<Resource> clients);
}