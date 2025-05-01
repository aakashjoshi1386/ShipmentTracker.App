namespace ShipmentTacker.App.Application.Shipments.ShipmentsCommands;
public sealed record UpdateShipmentStatusCommand(
    long Id,
    int StatusId) : IRequest<bool>;
