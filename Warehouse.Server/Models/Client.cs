using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Warehouse.Server.Models;

public class Client
{
    [Key]
    public string Guid { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public bool IsArchieved { get; set; } = false;

    [JsonIgnore]
    public ICollection<SendDocument> SendDocuments { get; set; } = new List<SendDocument>();
}
