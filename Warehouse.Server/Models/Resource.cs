using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Warehouse.Server.Models;

public class Resource
{
    [Key]
    public string Guid { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsArchieved { get; set; } = false;

    [JsonIgnore]
    public ICollection<Balance> Balances { get; set; } = new List<Balance>();
    [JsonIgnore]
    public ICollection<MeasureUnit> MeasureUnits { get; set; } = new List<MeasureUnit>();
    [JsonIgnore]
    public ICollection<ReceiveResource> ReceiveResources { get; set; } = new List<ReceiveResource>();
    [JsonIgnore]
    public ICollection<SendResource> SendResources { get; set; } = new List<SendResource>();
}
