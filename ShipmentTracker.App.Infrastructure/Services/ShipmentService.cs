namespace ShipmentTracker.App.Infrastructure.Services;
public class ShipmentService(ShipmentTrackerAppDBContext context) : IShipmentService
{
    private readonly ShipmentTrackerAppDBContext _context = context;
    public async Task<PaginatedResponse<ShipmentDTO>> GetShipments(GetShipmentsQuery query)
    {
        var shipmentQuery = _context.Shipments.AsQueryable();

        if (query.statusId.HasValue)
        {
            shipmentQuery = shipmentQuery.Where(x => x.Status == query.statusId.Value);
        }

        if (query.carrierId.HasValue)
        {
            shipmentQuery = shipmentQuery.Where(x => x.CarrierId == query.carrierId.Value);
        }

        var totalCount = await shipmentQuery.CountAsync();

        var shipments = await shipmentQuery
            .Skip((query.page - 1) * query.pageSize)
            .Take(query.pageSize)
            .Select(x => new ShipmentDTO
            (
                x.Id,
                x.Origin,
                x.Destination,
                _context.Carriers
                        .Where(c => c.Id == x.CarrierId)
                        .Select(c => c.Name)
                        .FirstOrDefault() ?? string.Empty,
                x.ShipmentDate.ToString("MM-dd-yyyy",CultureInfo.InvariantCulture),
                x.EstimatedDeliveryDate.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture),
                x.Status
            ))
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        return new PaginatedResponse<ShipmentDTO>(shipments,totalCount);
    }
    public async Task<bool> AddShipment(AddShipmentCommand add)
    {
        Shipment shipment = new Shipment()
        {
            Origin = add.Origin,
            Destination = add.Destination,
            CarrierId = add.CarrierId,
            ShipmentDate = add.ShipmentDate,
            EstimatedDeliveryDate = add.EstimatedDeliveryDate,
            Status = 1, // default status: Processing
            CreatedAt = DateTime.UtcNow,
        };
        await _context.Shipments.AddAsync(shipment);
        await _context.SaveChangesAsync();

        return shipment.Id > 0 ? true : false;
    }
    public async Task<bool> UpdateShipmentStatus(UpdateShipmentStatusCommand update)
    {
        Shipment shipment = await _context.Shipments.FindAsync(update.Id);
        if (shipment is null)
            return false;

        shipment.Status = update.StatusId;
        shipment.ModifiedAt = DateTime.UtcNow;

        _context.Entry(shipment).State = EntityState.Modified;
        var result = await _context.SaveChangesAsync();
        return result > 0 ? true : false;
    }
}
