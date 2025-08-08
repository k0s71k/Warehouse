using Microsoft.EntityFrameworkCore;
using Warehouse.Server.Data;
using Warehouse.Server.Models;
using Warehouse.Server.Models.DTO;
using Warehouse.Server.Services.Interfaces;

namespace Warehouse.Server.Services;

public class SendDocumentService: ISendDocumentService
{
    private readonly WarehouseContext _context;
    private readonly IBalanceService _balanceService;

    public SendDocumentService(WarehouseContext context, IBalanceService balanceService)
    {
        _context = context;
        _balanceService = balanceService;
    }

    public async Task AddAsync(DocumentDTO document)
    {
        await _context.SendDocuments.AddAsync(new SendDocument
        {
            Guid = Guid.NewGuid().ToString(),
            Number = document.Number,
            Date = document.Date,
            ClientId = document.Client!.Guid,
            IsSigned = false,
            Resources = document.Resources.Select(x => new SendResource
            {
                Guid = Guid.NewGuid().ToString(),
                ResourceId = x.Resource.Guid,
                MeasureUnitId = x.MeasureUnit.Guid,
                Quantity = x.Quantity
            }).ToList()
        });

        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(string id)
    {
        var document = await GetAsync(id);

        if (document.IsSigned)
            throw new Exception("Невозможно удалить подписанный документ");

        _context.Remove(document);
        await _context.SaveChangesAsync();
    }

    public async Task<SendDocument> GetAsync(string id)
    {
        return await _context.SendDocuments
            .Include("Resources")
            .Include("Resources.Resource")
            .Include("Resources.MeasureUnit")
            .Include("Client")
            .FirstOrDefaultAsync(x => x.Guid == id)
            ?? throw new Exception($"Документ с ID {id} не найден");
    }

    public async Task<List<SendDocument>> GetAllAsync(DocumentFilterDTO filter)
    {
        var documentsQuery = _context.SendDocuments
            .Include("Resources")
            .Include("Resources.Resource")
            .Include("Resources.MeasureUnit")
            .Include("Client")
            .AsQueryable();

        if (!filter.Numbers.Any() && !filter.ResourceIds.Any() && !filter.MeasureUnitIds.Any() && !filter.ClientIds.Any())
            return await documentsQuery
                .Where(x => filter.DateStart <= x.Date && x.Date <= filter.DateEnd)
                .ToListAsync();

        if (filter.Numbers.Any())
            documentsQuery = documentsQuery.Where(x =>
                filter.Numbers
                    .Contains(x.Number!));

        if (filter.ClientIds.Any())
            documentsQuery = documentsQuery.Where(x =>
                filter.ClientIds
                    .Contains(x.ClientId));

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
        return await _context.SendDocuments
            .Select(x => x.Number!)
            .ToListAsync();
    }

    public async Task UpdateAsync(DocumentDTO document)
    {
        var currentDocument = await GetAsync(document.Guid!);

        if (currentDocument.IsSigned)
            throw new Exception("Невозможно обновить подписанный документ");
        
        _context.RemoveRange(currentDocument.Resources);

        currentDocument.Number = document.Number;
        currentDocument.Date = document.Date;
        //currentDocument.Client = document.Client!;
        currentDocument.ClientId = document.Client!.Guid;
        currentDocument.Resources = document.Resources
            .Select(x => new SendResource
            {
                Guid = x.Guid ?? Guid.NewGuid().ToString(),
                DocumentId = currentDocument.Guid,
                ResourceId = x.Resource.Guid,
                MeasureUnitId = x.MeasureUnit.Guid,
                Quantity = x.Quantity
            })
            .ToList();

        _context.Update(currentDocument);

        await _context.SaveChangesAsync();
    }

    public async Task SignAsync(string id)
    {
        var document = await GetAsync(id);
        document.IsSigned = !document.IsSigned;

        var resources = document.Resources.Select(x => new DocumentResourceDTO
        {
            Resource = x.Resource!,
            MeasureUnit = x.MeasureUnit!,
            Quantity = x.Quantity
        }).ToList();

        if (document.IsSigned)
            await _balanceService.RemoveRangeAsync(resources);
        else
            await _balanceService.AddRangeAsync(resources);

        await _context.SaveChangesAsync();
    }
}
