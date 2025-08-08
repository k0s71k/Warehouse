using System.ComponentModel.DataAnnotations;

namespace Warehouse.Server.Models;

public class SendDocument
{
    [Key]
    public string Guid { get; set; } = string.Empty;
    public string? Number { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public Client? Client { get; set; }
    public DateTime Date { get; set; }
    public bool IsSigned { get; set; }

    public ICollection<SendResource> Resources { get; set; } = new List<SendResource>();
}
