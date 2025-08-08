using System.Text.Json.Serialization;
using Warehouse.Server.Data;

namespace Warehouse.Server.Models.DTO;

public class DocumentDTO
{
    public string? Guid { get; set; }
    public Client? Client { get; set; }
    public string Number { get; set; } = null!;
    public DateTime Date { get; set; }
    public List<DocumentResourceDTO> Resources { get; set; } = new();
}
