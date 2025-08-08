using Warehouse.Server.Models;
using Warehouse.Server.Models.DTO;

namespace Warehouse.Server.Services.Interfaces;

public interface IReceiveDocumentService
{
    public Task AddAsync(DocumentDTO document);
    public Task RemoveAsync(string id);
    public Task<ReceiveDocument> GetAsync(string id);
    public Task<List<ReceiveDocument>> GetAllAsync(DocumentFilterDTO filter);
    public Task<List<string>> GetAllNumbersAsync();
    public Task UpdateAsync(DocumentDTO document);
}
