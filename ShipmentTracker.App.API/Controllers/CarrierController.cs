namespace ShipmentTracker.App.API.Controllers;

[Route("api/carriers")]
[ApiController]
public class CarrierController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetCarriers()
    {
        var carriers = await _mediator.Send(new GetCarriersQuery());
        return carriers.Any() ? Ok(carriers) : NotFound(carriers);
    }
}
