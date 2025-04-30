namespace ShipmentTracker.App.Domain.Entities;

public class Carrier
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
