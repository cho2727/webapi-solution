using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Features.Middleware;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MiddlewareController : ControllerBase
{
    private readonly IMediator _mediator;

    public MiddlewareController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("MidAlive")]
    [ProducesResponseType(typeof(MiddleAlive.Response), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> MiddleAlive()
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var request = new MiddleAlive.Command();
        var response = await _mediator.Send(request);
        return Ok(response);
    }


    [AllowAnonymous]
    [HttpGet("MeasureDatas/{stationId?}")]
    [ProducesResponseType(typeof(StationMeasureData.Response), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMeasureData(long? stationId)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var request = new StationMeasureData.Command() { StationId = stationId ?? 0 };
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("MeasureDetailDatas/{stationId?}")]
    [ProducesResponseType(typeof(StationMeasureData.Response), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMeasureDetailData(long? stationId)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var request = new StationMeasureDetailData.Command() { StationId = stationId ?? 0 };
        var response = await _mediator.Send(request);
        return Ok(response);
    }


    [AllowAnonymous]
    [HttpGet("AlarmData/{eventBoxName}")]
    [ProducesResponseType(typeof(AlarmReceiveMessage.Response), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RecvAlarmData(string eventBoxName)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var request = new AlarmReceiveMessage.Command() { EventBoxName = eventBoxName };
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}