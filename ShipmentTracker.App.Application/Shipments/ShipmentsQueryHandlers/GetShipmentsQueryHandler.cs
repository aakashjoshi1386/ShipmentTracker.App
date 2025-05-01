namespace ShipmentTacker.App.Application.Shipments.ShipmentsQueryHandlers;
public sealed class GetShipmentsQueryHandler(IShipmentService shipmentService) : IRequestHandler<GetShipmentsQuery, PaginatedResponse<ShipmentDTO>>
{
    private readonly IShipmentService _shipmentService = shipmentService;

    public async Task<PaginatedResponse<ShipmentDTO>> Handle(GetShipmentsQuery request, CancellationToken cancellationToken)
    {
        return await _shipmentService.GetShipments(request);
    }
}
