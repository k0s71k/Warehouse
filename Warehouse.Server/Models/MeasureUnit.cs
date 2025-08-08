using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Warehouse.Server.Models;

public class MeasureUnit
{
    [Key]
    public string Guid { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsArchieved { get; set; } = false;

    [JsonIgnore]
    public ICollection<Balance> Balances { get; set; } = new List<Balance>();
    [JsonIgnore]
    public ICollection<Resource> Resources { get; set; } = new List<Resource>();
    [JsonIgnore]
    public ICollection<ReceiveResource> ReceiveResources { get; set; } = new List<ReceiveResource>();
    [JsonIgnore]
    public ICollection<SendResource> SendResources { get; set; } = new List<SendResource>();
}
