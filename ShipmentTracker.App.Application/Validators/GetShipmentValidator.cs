namespace ShipmentTacker.App.Application.Validators;
public sealed class GetShipmentValidator : AbstractValidator<GetShipmentsQuery>
{
    public GetShipmentValidator()
    {
        RuleFor(x => x.PageNumber)
             .GreaterThan(0)
             .WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.");

        RuleFor(x => x.Status)
            .Must(status => string.IsNullOrWhiteSpace(status) || Enum.GetNames(typeof(Status)).Any(e => e.Equals(status, StringComparison.OrdinalIgnoreCase)))
            .WithMessage("Invalid shipment status. Valid values are: Processing, Shipped, InTransit, OutForDelivery, Delivered, PickedUp.");

        RuleFor(x => x.CarrierId)
            .Must(id => !id.HasValue || id.Value > 0)
            .WithMessage("CarrierId must be greater than 0.");
    }
}

public sealed class AddShipmentValidator : AbstractValidator<AddShipmentCommand>
{
    public AddShipmentValidator()
    {
        RuleFor(x => x.Origin)
             .NotEmpty()
             .WithMessage("Origin is required.");

        RuleFor(x => x.Destination)
            .NotEmpty()
            .WithMessage("Destination is required.");

        RuleFor(x => x.CarrierId)
            .Must(id => id > 0)
            .WithMessage("CarrierId must be greater than 0.");

        RuleFor(x => x.ShipmentDate)
            .Must(date => date > DateTime.MinValue)
            .WithMessage("Shipment date is required.");

        RuleFor(x => x.EstimatedDeliveryDate)
            .GreaterThan(x => x.ShipmentDate)
            .WithMessage("Estimated delivery date must be greater than Shipment date.");
    }
}

public sealed class UpdateShipmentValidator : AbstractValidator<UpdateShipmentStatusCommand>
{
    public UpdateShipmentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Shipment is required.")
            .Must(x => x > 0)
            .WithMessage("ShipmentId must be greater than 0.");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required.")
            .Must(status => string.IsNullOrWhiteSpace(status) || Enum.GetNames(typeof(Status)).Any(e => e.Equals(status, StringComparison.OrdinalIgnoreCase)))
            .WithMessage("Invalid shipment status. Valid values are: Processing, Shipped, InTransit, OutForDelivery, Delivered, PickedUp.");
    }
}