namespace ShipmentTacker.App.Application.Shipments.ShipmentsCommands;
public sealed record UpdateShipmentStatusCommand(
    long Id,
    string Status) : IRequest<bool>;
