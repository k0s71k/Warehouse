using Warehouse.Server.Models;

namespace Warehouse.Server.Services.Interfaces;

public interface IClientService
{
    public Task<List<Client>> GetActiveAsync();
    public Task<List<Client>> GetArchievedAsync();
    public Task AddAsync(string name, string address);
    public Task RemoveAsync(string id);
    public Task UpdateRangeAsync(List<Client> clients);
}
