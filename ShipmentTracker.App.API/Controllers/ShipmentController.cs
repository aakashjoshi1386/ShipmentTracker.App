namespace ShipmentTracker.App.API.Controllers;

[Route("api/shipments")]
[ApiController]
public class ShipmentController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetShipments([FromQuery] GetShipmentsQuery query)
    {
        var shipments = await mediator.Send(query);
        return shipments.Items.Any() ? Ok(shipments) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> AddShipment([FromBody] AddShipmentCommand add)
    {
        var result = await mediator.Send(add);
        return result ? Created(string.Empty, result) : BadRequest();
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateShipmentStatus(long id, [FromBody] UpdateShipmentStatusCommand update)
    {
        var result = await mediator.Send(update with { Id = id });
        return result ? Ok(result) : BadRequest();
    }
}
