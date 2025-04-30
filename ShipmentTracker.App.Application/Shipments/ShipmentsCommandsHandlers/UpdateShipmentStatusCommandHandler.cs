namespace ShipmentTacker.App.Application.Shipments.ShipmentsCommandsHandlers;
public sealed class UpdateShipmentStatusCommandHandler(IShipmentService shipmentService) : IRequestHandler<UpdateShipmentStatusCommand, bool>
{
    private readonly IShipmentService _shipmentService = shipmentService;

    public async Task<bool> Handle(UpdateShipmentStatusCommand request, CancellationToken cancellationToken)
    {
        return await _shipmentService.UpdateShipmentStatus(request);
    }
}
