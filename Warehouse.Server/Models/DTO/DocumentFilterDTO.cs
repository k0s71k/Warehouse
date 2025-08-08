namespace Warehouse.Server.Models.DTO;

public class DocumentFilterDTO
{
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public List<string> Numbers { get; set; } = new();
    public List<string> ResourceIds { get; set; } = new();
    public List<string> MeasureUnitIds { get; set; } = new();
    public List<string> ClientIds { get; set; } = new();
}