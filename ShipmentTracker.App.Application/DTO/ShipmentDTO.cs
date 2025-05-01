namespace ShipmentTacker.App.Application.DTO;
public sealed record ShipmentDTO(
    long Id,
    string Origin,
    string Destination,
    string Carrier,
    string ShipmentDate,
    string EstimatedDeliveryDate,
    int StatusId
    );
