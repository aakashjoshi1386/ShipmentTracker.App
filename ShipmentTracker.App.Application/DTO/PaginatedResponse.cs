namespace ShipmentTracker.App.Application.DTO;

public sealed record PaginatedResponse<T>(
IEnumerable<T> Items,
int TotalCount
);
