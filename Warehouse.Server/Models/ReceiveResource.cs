using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Warehouse.Server.Models;

public class ReceiveResource
{
    [Key]
    public string Guid { get; set; } = null!;
    public string ResourceId { get; set; } = null!;
    public Resource Resource { get; set; } = null!;
    public string MeasureUnitId { get; set; } = null!;
    public MeasureUnit MeasureUnit { get; set; } = null!;
    [JsonIgnore]
    public string DocumentId { get; set; } = null!;
    [JsonIgnore]
    public ReceiveDocument Document { get; set; } = null!;
    public double Quantity { get; set; }
}
