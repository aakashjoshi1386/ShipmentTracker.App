namespace ShipmentTracker.App.Infrastructure.Services;
public class CarrierService(ShipmentTrackerAppDBContext context) : ICarrierService
{
    private readonly ShipmentTrackerAppDBContext _context = context;

    public async Task<IEnumerable<CarrierDTO>> GetCarriersAsync()
    {
        var carriers = await _context.Carriers
                                    .Where(x => x.IsActive)
                                    .Select(x => new CarrierDTO(x.Id, x.Name))
                                    .ToListAsync();

        return carriers;
    }
}
