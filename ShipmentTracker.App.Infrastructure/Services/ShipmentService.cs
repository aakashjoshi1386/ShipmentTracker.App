namespace ShipmentTracker.App.Infrastructure.Services;
public class ShipmentService(ShipmentTrackerAppDBContext context) : IShipmentService
{
    private readonly ShipmentTrackerAppDBContext _context = context;
    public async Task<IEnumerable<ShipmentDTO>> GetShipments(GetShipmentsQuery query)
    {
        var shipmentQuery = _context.Shipments.AsQueryable();

        if (!string.IsNullOrEmpty(query.Status) &&
            Enum.TryParse<Status>(query.Status, true, out var parsedStatus))
        {
            shipmentQuery = shipmentQuery.Where(x => x.Status == parsedStatus);
        }

        if (query.CarrierId.HasValue)
        {
            shipmentQuery = shipmentQuery.Where(x => x.CarrierId == query.CarrierId.Value);
        }

        shipmentQuery = shipmentQuery.Skip((query.PageNumber - 1) * query.PageSize)
                                     .Take(query.PageSize);

        var shipments = await shipmentQuery
                                .Select(x => new ShipmentDTO
                                            (
                                                x.Id,
                                                x.Origin,
                                                x.Destination,
                                                _context.Carriers
                                                        .Where(c => c.Id == x.CarrierId)
                                                        .Select(c => c.Name)
                                                        .FirstOrDefault() ?? string.Empty,
                                                x.ShipmentDate,
                                                x.EstimatedDeliveryDate,
                                                x.Status.ToString()
                                            ))
                                .ToListAsync();

        return shipments;
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
            Status = Status.Processing,
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

        shipment.Status = update.Status switch
        {
            "Processing" => Status.Processing,
            "Shipped" => Status.Shipped,
            "InTransit" => Status.InTransit,
            "OutForDelivery" => Status.OutForDelivery,
            "Delivered" => Status.Delivered,
            "PickedUp" => Status.PickedUp,
            _ => shipment.Status
        };
        shipment.ModifiedAt = DateTime.UtcNow;

        _context.Entry(shipment).State = EntityState.Modified;
        var result = await _context.SaveChangesAsync();
        return result > 0 ? true : false;
    }
}
