namespace ShipmentTacker.App.Application.Carriers.CarriersQueries;
public sealed record GetCarriersQuery() : IRequest<IEnumerable<CarrierDTO>>;
