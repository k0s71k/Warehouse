namespace Warehouse.Server.Models.DTO
{
    public class DocumentResourceDTO
    {
        public string? Guid { get; set; }
        public Resource Resource { get; set; } = null!;
        public MeasureUnit MeasureUnit { get; set; } = null!;
        public double Quantity { get; set; }
    }
}
