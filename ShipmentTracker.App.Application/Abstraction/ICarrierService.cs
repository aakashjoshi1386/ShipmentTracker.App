namespace ShipmentTacker.App.Application.Abstraction;
public interface ICarrierService
{
    Task<IEnumerable<CarrierDTO>> GetCarriersAsync();
}
