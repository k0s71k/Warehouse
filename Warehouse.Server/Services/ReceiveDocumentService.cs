using Microsoft.EntityFrameworkCore;
using Warehouse.Server.Data;
using Warehouse.Server.Models;
using Warehouse.Server.Models.DTO;
using Warehouse.Server.Services.Interfaces;

namespace Warehouse.Server.Services;

public class ReceiveDocumentService: IReceiveDocumentService
{
    private readonly WarehouseContext _context;
    private readonly IBalanceService _balanceService;

    public ReceiveDocumentService(WarehouseContext context, IBalanceService balanceService)
    {
        _context = context;
        _balanceService = balanceService;
    }

    public async Task AddAsync(DocumentDTO document)
    {
        await _context.ReceiveDocuments.AddAsync(new ReceiveDocument
        {
            Guid = Guid.NewGuid().ToString(),
            Number = document.Number,
            Date = document.Date,
            Resources = document.Resources.Select(x => new ReceiveResource {
                Guid = Guid.NewGuid().ToString(),
                ResourceId = x.Resource.Guid,
                MeasureUnitId = x.MeasureUnit.Guid,
                Quantity = x.Quantity
            }).ToList()
        });

        await _balanceService.AddRangeAsync(document.Resources);

        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(string id)
    {
        var document = await GetAsync(id);

        await _balanceService.RemoveRangeAsync(document.Resources.Select(x => new DocumentResourceDTO
        {
            Resource = x.Resource!,
            MeasureUnit = x.MeasureUnit!,
            Quantity = x.Quantity
        }).ToList());

        _context.Remove(document);

        await _context.SaveChangesAsync();
    }

    public async Task<ReceiveDocument> GetAsync(string id)
    {
        return await _context.ReceiveDocuments
            .Include("Resources")
            .Include("Resources.Resource")
            .Include("Resources.MeasureUnit")
            .FirstOrDefaultAsync(x => x.Guid == id)
            ?? throw new Exception($"Документ с ID {id} не найден");
    }

    public async Task<List<ReceiveDocument>> GetAllAsync(DocumentFilterDTO filter)
    {
        var documentsQuery = _context.ReceiveDocuments
            .Include("Resources")
            .Include("Resources.Resource")
            .Include("Resources.MeasureUnit")
            .AsQueryable();

        if (!filter.Numbers.Any() && !filter.ResourceIds.Any() && !filter.MeasureUnitIds.Any())
            return await documentsQuery
                .Where(x => filter.DateStart <= x.Date && x.Date <= filter.DateEnd)
                .ToListAsync();

        if (filter.Numbers.Any())
            documentsQuery = documentsQuery.Where(x =>
                filter.Numbers
                    .Contains(x.Number!));

        if (filter.ResourceIds.Any())
        {
            documentsQuery = documentsQuery.Where(x =>
                x.Resources.Any(y => filter.ResourceIds.Contains(y.ResourceId)));
            await documentsQuery.ForEachAsync(x => x.Resources = x.Resources
                .Where(y => filter.ResourceIds.Contains(y.ResourceId)).ToList());
        }
                
        if (filter.MeasureUnitIds.Any())
        {
            documentsQuery = documentsQuery.Where(x =>
                x.Resources.Any(y => filter.MeasureUnitIds.Contains(y.MeasureUnitId)));
            await documentsQuery.ForEachAsync(x => x.Resources = x.Resources
                .Where(y => filter.MeasureUnitIds.Contains(y.MeasureUnitId)).ToList());
        }

        return await documentsQuery.ToListAsync();
    }

    public async Task<List<string>> GetAllNumbersAsync()
    {
        return await _context.ReceiveDocuments
            .Select(x => x.Number!)
            .ToListAsync();
    }

    public async Task UpdateAsync(DocumentDTO document)
    {
        var currentDocument = await GetAsync(document.Guid!);

        await _balanceService.RemoveRangeAsync(currentDocument.Resources.Select(x => new DocumentResourceDTO
        {
            Resource = x.Resource!,
            MeasureUnit = x.MeasureUnit!,
            Quantity = x.Quantity
        }).ToList());

        _context.RemoveRange(currentDocument.Resources);

        await _balanceService.AddRangeAsync(document.Resources);

        currentDocument.Number = document.Number;
        currentDocument.Date = document.Date;
        currentDocument.Resources = document.Resources
            .Select(x => new ReceiveResource
            {
                Guid = x.Guid ?? Guid.NewGuid().ToString(),
                DocumentId = currentDocument.Guid,
                ResourceId = x.Resource.Guid,
                MeasureUnitId = x.MeasureUnit.Guid,
                Quantity = x.Quantity
            })
            .ToList();
       
        await _context.SaveChangesAsync();
    }
}
