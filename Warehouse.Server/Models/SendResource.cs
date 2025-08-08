using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Warehouse.Server.Models;

public class SendResource
{
    [Key]
    public string Guid { get; set; } = string.Empty;
    public string ResourceId { get; set; } = string.Empty;
    public Resource? Resource { get; set; }
    public string MeasureUnitId { get; set; } = string.Empty;
    public MeasureUnit? MeasureUnit { get; set; }
    [JsonIgnore]
    public string DocumentId { get; set; } = string.Empty;
    [JsonIgnore]
    public SendDocument? Document { get; set; }
    public double Quantity { get; set; }
}
