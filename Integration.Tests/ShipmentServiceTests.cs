namespace Integration.Tests;

public class ShipmentServiceTests : IDisposable
{
    private readonly ShipmentTrackerAppDBContext _context;
    private readonly ShipmentService _service;
    public ShipmentServiceTests()
    {
        var options = new DbContextOptionsBuilder<ShipmentTrackerAppDBContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;
        _context = new ShipmentTrackerAppDBContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();

        SeedData(_context);

        _service = new ShipmentService(_context);
    }
    
    private void SeedData(ShipmentTrackerAppDBContext context)
    {
        context.Carriers.AddRange(
            new Carrier { Name = "Carrier A", IsActive = true },
            new Carrier { Name = "Carrier B", IsActive = true }
        );

        context.Shipments.AddRange(
            new Shipment
            {
                Origin = "Origin A",
                Destination = "Destination A",
                CarrierId = 1,
                ShipmentDate = DateTime.UtcNow,
                EstimatedDeliveryDate = DateTime.UtcNow.AddDays(5),
                Status = 1, // Processing
                CreatedAt = DateTime.UtcNow,
            },
            new Shipment
            {
                Origin = "Origin B",
                Destination = "Destination B",
                CarrierId = 2,
                ShipmentDate = DateTime.UtcNow,
                EstimatedDeliveryDate = DateTime.UtcNow.AddDays(3),
                Status = 2, // Shipped
                CreatedAt = DateTime.UtcNow,
            }
        );
        context.SaveChanges();
    }
    [Fact]
    public async Task GetShipments_ReturnsAll_WhenNoFilters()
    {
        var query = new GetShipmentsQuery(1,10,null,null);

        var result = await _service.GetShipments(query);

        Assert.Equal(2, result.TotalCount);
        Assert.Equal(2, result.Items.Count());
    }

    [Fact]
    public async Task AddShipment_AddsAndReturnsTrue()
    {
        var addShipmentCommand = new AddShipmentCommand
        (
            "Origin C",
            "Destination C",
            1,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(4)
        );
        var result = await _service.AddShipment(addShipmentCommand);
        Assert.Equal(3, _context.Shipments.Count());
    }
    [Fact]
    public async Task UpdateShipmentStatus_ValidId_UpdatesAndReturnsTrue()
    {
        var updateShipmentStatusCommand = new UpdateShipmentStatusCommand(1, 4);
        var result = await _service.UpdateShipmentStatus(updateShipmentStatusCommand);
        Assert.True(result);
        var shipment = await _context.Shipments.FindAsync((long)1);
        Assert.Equal(4, _context.Shipments.Find((long)1).Status);
    }

    [Fact]
    public async Task UpdateShipmentStatus_InvalidId_ReturnsFalse()
    {
        var updateShipmentStatusCommand = new UpdateShipmentStatusCommand(999, 4);
        var result = await _service.UpdateShipmentStatus(updateShipmentStatusCommand);
        Assert.False(result);
    }
    public void Dispose()
    {
        _context.Database.CloseConnection();
        _context.Dispose();
    }

}