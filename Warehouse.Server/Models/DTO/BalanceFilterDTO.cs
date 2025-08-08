namespace Warehouse.Server.Models.DTO;

public class BalanceFilterDTO
{
    public List<Resource> Resources { get; set; } = new();
    public List<MeasureUnit> MeasureUnits { get; set; } = new();
}