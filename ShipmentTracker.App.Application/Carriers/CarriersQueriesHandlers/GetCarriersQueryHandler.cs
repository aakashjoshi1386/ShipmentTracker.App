namespace ShipmentTacker.App.Application.Carriers.CarriersQueryHandlers;
public sealed class GetCarriersQueryHandler(ICarrierService carrierService) : IRequestHandler<GetCarriersQuery, IEnumerable<CarrierDTO>>
{
    private readonly ICarrierService _carrierService = carrierService;

    public async Task<IEnumerable<CarrierDTO>> Handle(GetCarriersQuery request, CancellationToken cancellationToken)
    {
        return await _carrierService.GetCarriersAsync();
    }
}