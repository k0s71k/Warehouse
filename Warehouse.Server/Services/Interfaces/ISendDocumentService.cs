using Warehouse.Server.Models;
using Warehouse.Server.Models.DTO;

namespace Warehouse.Server.Services.Interfaces;

public interface ISendDocumentService
{
    public Task AddAsync(DocumentDTO document);
    public Task RemoveAsync(string id);
    public Task<SendDocument> GetAsync(string id);
    public Task<List<SendDocument>> GetAllAsync(DocumentFilterDTO filter);
    public Task<List<string>> GetAllNumbersAsync();
    public Task UpdateAsync(DocumentDTO document);
    public Task SignAsync(string id);
}
