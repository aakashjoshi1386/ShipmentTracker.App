namespace ShipmentTacker.App.Application.Shipments.ShipmentsCommands;
public sealed record AddShipmentCommand(
    string Origin,
    string Destination,
    int CarrierId,
    DateTime ShipmentDate,
    DateTime EstimatedDeliveryDate
    ) : IRequest<bool>;
