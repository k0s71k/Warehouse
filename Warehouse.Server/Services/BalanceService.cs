using Microsoft.EntityFrameworkCore;
using Warehouse.Server.Data;
using Warehouse.Server.Models;
using Warehouse.Server.Models.DTO;
using Warehouse.Server.Services.Interfaces;

namespace Warehouse.Server.Services;

public class BalanceService: IBalanceService
{
    private readonly WarehouseContext _context;

    public BalanceService(WarehouseContext context)
    {
        _context = context;
    }

    public async Task AddRangeAsync(List<DocumentResourceDTO> resources)
    {
        foreach (var resource in resources)
        {
            var balance = await GetBalanceAsync(
                resource.Resource.Guid,
                resource.MeasureUnit.Guid);

            balance.Quantity += resource.Quantity;
            _context.Update(balance);
        }

        await _context.SaveChangesAsync();
    }

    public async Task RemoveRangeAsync(List<DocumentResourceDTO> resources)
    {
        if (!await CanRemoveRangeAsync(resources))
            throw new Exception("Не хватает баланса ресурсов");

        foreach (var resource in resources)
        {
            var balance = await GetBalanceAsync(
                resource.Resource.Guid,
                resource.MeasureUnit.Guid);

            balance.Quantity -= resource.Quantity;
            _context.Update(balance);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<Balance>> GetAllBalancesAsync(BalanceFilterDTO filter)
    {
        var balancesQuery = _context.Balances
            .Include(x => x.Resource)
            .Include(x => x.MeasureUnit)
            .AsQueryable();

        if (filter.Resources.Count() > 0)
            balancesQuery = balancesQuery
                .Where(x => filter.Resources
                    .Contains(x.Resource!));

        if (filter.MeasureUnits.Count() > 0)
            balancesQuery = balancesQuery
                .Where(x => filter.MeasureUnits
                    .Contains(x.MeasureUnit!));

        return await balancesQuery.ToListAsync();
    }

    public async Task<List<Balance>> GetAllBalancesAsync()
    {
        return await _context.Balances
            .Include(x => x.Resource)
            .Include(x => x.MeasureUnit)
            .ToListAsync();
    }

    private async Task<bool> CanRemoveRangeAsync(List<DocumentResourceDTO> resources)
    {
        foreach (var resource in resources)
        {
            var balance = await GetBalanceAsync(
                resource.Resource.Guid,
                resource.MeasureUnit.Guid);

            if (balance.Quantity < resource.Quantity)
                return false;
        }

        return true;
    }

    private async Task<Balance> CreateBalanceAsync(string resourceId, string measureUnitId)
    {
        var balance = new Balance
        {
            ResourceId = resourceId,
            MeasureUnitId = measureUnitId,
            Quantity = 0d
        };

        await _context.Balances.AddAsync(balance);
        await _context.SaveChangesAsync();

        return balance;
    }

    private async Task<Balance> GetBalanceAsync(string resourceId, string measureUnitId)
        => await _context.Balances
            .FirstOrDefaultAsync(x => x.ResourceId == resourceId && x.MeasureUnitId == measureUnitId)
            ?? await CreateBalanceAsync(resourceId, measureUnitId);
}
