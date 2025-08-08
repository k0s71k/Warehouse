using Microsoft.EntityFrameworkCore;
using System.Resources;
using Warehouse.Server.Data;
using Warehouse.Server.Models;
using Warehouse.Server.Services.Interfaces;

namespace Warehouse.Server.Services;

public class ResourceService: IResourceService
{
    private readonly WarehouseContext _context;
    public ResourceService(WarehouseContext context)
    {
        _context = context;
    }

    public async Task<List<Resource>> GetActiveAsync()
    {
        return await _context.Resources
            .Where(x => !x.IsArchieved)
            .ToListAsync();
    }

    public async Task<List<Resource>> GetArchievedAsync()
    {
        return await _context.Resources
            .Where(x => x.IsArchieved)
            .ToListAsync();
    }

    public async Task AddAsync(string name)
    {
        await _context.Resources.AddAsync(new Resource
        {
            Guid = Guid.NewGuid().ToString(),
            Name = name,
            IsArchieved = false,
        });

        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(string id)
    {
        var resource = await GetResourceAsync(id)
            ?? throw new Exception($"Не удалось найти ресурс с ID {id}");

        if (resource.Balances.Count() != 0)
            throw new Exception("Нельзя удалить используемый ресурс");

        _context.Resources.Remove(resource);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(List<Resource> resources)
    {
        _context.Resources.UpdateRange(resources);
        await _context.SaveChangesAsync();
    }

    private async Task<Resource> GetResourceAsync(string id)
    {
        return await _context.Resources
            .Include(x => x.Balances)
            .FirstOrDefaultAsync(x => x.Guid == id)
            ?? throw new Exception($"Ресурс с ID {id} не найден");
    }
}
