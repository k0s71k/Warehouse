using Microsoft.EntityFrameworkCore;
using Warehouse.Server.Data;
using Warehouse.Server.Models;
using Warehouse.Server.Services.Interfaces;

namespace Warehouse.Server.Services;

public class ClientService: IClientService
{
    private readonly WarehouseContext _context;

    public ClientService(WarehouseContext context)
    {
        _context = context;
    }

    public async Task<List<Client>> GetActiveAsync()
    {
        return await _context.Clients
            .Where(x => !x.IsArchieved)
            .ToListAsync();
    }

    public async Task<List<Client>> GetArchievedAsync()
    {
        return await _context.Clients
            .Where(x => x.IsArchieved)
            .ToListAsync();
    }

    public async Task AddAsync(string name, string address)
    {
        await _context.Clients.AddAsync(new Client
        {
            Guid = Guid.NewGuid().ToString(),
            Name = name,
            Address = address,
            IsArchieved = false,
        });
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(string id)
    {
        var client = await GetClientAsync(id);

        if (client.SendDocuments.Count() != 0)
            throw new Exception("Нельзя удалить клиента у которого есть отправленные заказы");

        _context.Remove(client);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(List<Client> clients)
    {
        _context.UpdateRange(clients);
        await _context.SaveChangesAsync();
    }

    private async Task<Client> GetClientAsync(string id)
    {
        return await _context.Clients
            .Include(x => x.SendDocuments)
            .FirstOrDefaultAsync(x => x.Guid == id)
            ?? throw new Exception($"Не удалось найти клиента с ID {id}");
    }
}
