namespace ShipmentTacker.App.Application.Shipments.ShipmentsQueries;
public sealed record GetShipmentsQuery(
int page,
int pageSize,
int? statusId,
int? carrierId) : IRequest<PaginatedResponse<ShipmentDTO>>;
