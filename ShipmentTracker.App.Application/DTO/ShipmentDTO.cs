namespace ShipmentTacker.App.Application.DTO;
public sealed record ShipmentDTO(
    long Id,
    string Origin,
    string Destination,
    string Carrier,
    DateTime ShipmentDate,
    DateTime EstimatedDeliveryDate,
    string Status
    );
