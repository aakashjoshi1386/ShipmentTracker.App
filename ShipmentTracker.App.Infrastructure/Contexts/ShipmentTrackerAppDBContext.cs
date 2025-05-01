namespace ShipmentTracker.App.Infrastructure.Contexts;
public class ShipmentTrackerAppDBContext : DbContext
{
    public ShipmentTrackerAppDBContext(DbContextOptions<ShipmentTrackerAppDBContext> options) : base(options) { }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<Carrier> Carriers { get; set; }
    public DbSet<ShipmentStatus> ShipmentStatuses { get; set; }
    public DbSet<ExceptionLog> ExceptionLogs { get; set; }
}
