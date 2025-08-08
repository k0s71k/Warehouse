namespace Warehouse.Server.Models;

public class Balance
{
    public string ResourceId { get; set; } = null!;
    public Resource Resource { get; set; } = null!;
    public string MeasureUnitId { get; set; } = null!;
    public MeasureUnit MeasureUnit { get; set; } = null!;
    public double Quantity { get; set; }
}