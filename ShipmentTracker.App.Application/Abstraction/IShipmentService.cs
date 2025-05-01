namespace ShipmentTacker.App.Application.Abstraction;
public interface IShipmentService
{
    Task<PaginatedResponse<ShipmentDTO>> GetShipments(GetShipmentsQuery shipments);
    Task<bool> AddShipment(AddShipmentCommand add);
    Task<bool> UpdateShipmentStatus(UpdateShipmentStatusCommand update);
}
