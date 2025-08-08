using System.ComponentModel.DataAnnotations;

namespace Warehouse.Server.Models;

public class ReceiveDocument
{
    [Key]
    public string Guid { get; set; } = null!;
    public string Number { get; set; } = null!;
    public DateTime Date { get; set; }

    public ICollection<ReceiveResource> Resources { get; set; } = new List<ReceiveResource>();
}
