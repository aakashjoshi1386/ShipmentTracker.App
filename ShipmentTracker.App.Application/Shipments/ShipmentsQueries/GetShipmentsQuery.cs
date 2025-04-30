namespace ShipmentTacker.App.Application.Shipments.ShipmentsQueries;
public sealed record GetShipmentsQuery(
int PageNumber,
int PageSize,
string? Status,
int? CarrierId) : IRequest<IEnumerable<ShipmentDTO>>;
