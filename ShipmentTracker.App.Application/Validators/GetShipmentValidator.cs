namespace ShipmentTacker.App.Application.Validators;
public sealed class GetShipmentValidator : AbstractValidator<GetShipmentsQuery>
{
    public GetShipmentValidator()
    {
        RuleFor(x => x.page)
             .GreaterThan(0)
             .WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.pageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.");

        RuleFor(x => x.statusId)
            .Must(id => !id.HasValue || id.Value > 0)
            .WithMessage("Status must be greater than 0.");

        RuleFor(x => x.carrierId)
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
            .GreaterThan(DateTime.MinValue)
            .WithMessage("Shipment date is required.");

        RuleFor(x => x.EstimatedDeliveryDate)
            .GreaterThan(DateTime.MinValue)
            .WithMessage("Estimated delivery date is required.")
            .GreaterThanOrEqualTo(x => x.ShipmentDate)
            .WithMessage("Estimated delivery date must be on or after the shipment date.");

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

        RuleFor(x => x.StatusId)
            .NotEmpty()
            .WithMessage("Status is required.")
            .Must(x =>x > 0)
            .WithMessage("Status must be greater than 0.");
    }
}