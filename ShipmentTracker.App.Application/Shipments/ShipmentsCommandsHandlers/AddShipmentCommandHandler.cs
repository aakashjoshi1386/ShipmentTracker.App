namespace ShipmentTacker.App.Application.Shipments.ShipmentsCommandsHandlers;
public sealed class AddShipmentCommandHandler(IShipmentService shipmentService) : IRequestHandler<AddShipmentCommand, bool>
{
    private readonly IShipmentService _shipmentService = shipmentService;

    public async Task<bool> Handle(AddShipmentCommand request, CancellationToken cancellationToken)
    {
        return await _shipmentService.AddShipment(request);
    }
}