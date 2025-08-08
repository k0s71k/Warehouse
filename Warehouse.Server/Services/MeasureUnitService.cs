using Microsoft.EntityFrameworkCore;
using Warehouse.Server.Data;
using Warehouse.Server.Models;
using Warehouse.Server.Services.Interfaces;

namespace Warehouse.Server.Services;

public class MeasureUnitService: IMeasureUnitService
{
    private readonly WarehouseContext _context;
    public MeasureUnitService(WarehouseContext context)
    {
        _context = context;
    }

    public async Task<List<MeasureUnit>> GetActiveAsync()
    {
        return await _context.MeasureUnits
            .Where(x => !x.IsArchieved)
            .ToListAsync();
    }

    public async Task<List<MeasureUnit>> GetArchievedAsync()
    {
        return await _context.MeasureUnits
            .Where(x => x.IsArchieved)
            .ToListAsync();
    }

    public async Task AddAsync(string name)
    {
        await _context.MeasureUnits.AddAsync(new MeasureUnit
        {
            Guid = Guid.NewGuid().ToString(),
            Name = name,
            IsArchieved = false,
        });

        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(string id)
    {
        var measureUnit = await GetMeasureUnitAsync(id)
            ?? throw new Exception($"Не удалось найти ед. измерения с ID {id}");

        if (measureUnit.Balances.Count() != 0)
            throw new Exception("Нельзя удалить используемую ед. измерения");

        _context.MeasureUnits.Remove(measureUnit);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(List<MeasureUnit> measureUnits)
    {
        _context.MeasureUnits.UpdateRange(measureUnits);
        await _context.SaveChangesAsync();
    }

    private async Task<MeasureUnit> GetMeasureUnitAsync(string id)
    {
        return await _context.MeasureUnits
            .Include(x => x.Balances)
            .FirstOrDefaultAsync(x => x.Guid == id)
            ?? throw new Exception($"Единица измерения с ID {id} не найдена");
    }
}
