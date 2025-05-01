namespace ShipmentTracker.App.Domain.Entities;
public sealed class Shipment
{
    public long Id { get; set; }
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public int CarrierId { get; set; }
    public DateTime ShipmentDate { get; set; }
    public DateTime EstimatedDeliveryDate { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
}
