using System.ComponentModel.DataAnnotations;

namespace Warehouse.Server.Models;

public class SendDocument
{
    [Key]
    public string Guid { get; set; } = null!;
    public string Number { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public Client Client { get; set; } = null!;
    public DateTime Date { get; set; }
    public bool IsSigned { get; set; }

    public ICollection<SendResource> Resources { get; set; } = new List<SendResource>();
}
