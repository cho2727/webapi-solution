using ApiServer.Features.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Features.Database;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DatabaseController : ControllerBase
{
    private readonly IMediator _mediator;

    public DatabaseController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("Stations")]
    [ProducesResponseType(typeof(StationInfo.Response), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> StationInfo()
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var request = new StationInfo.Command();
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("Equipments/{stationId?}")]
    [ProducesResponseType(typeof(EquipmentInfo.Response), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> EquipmentInfo(long? stationId)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var request = new EquipmentInfo.Command() { StationId = stationId ?? 0 };
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("StationTypes")]
    [ProducesResponseType(typeof(StationTypeInfo.Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStationTypes()
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var request = new StationTypeInfo.Command();
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("ObjectTypes")]
    [ProducesResponseType(typeof(ObjectTypeInfo.Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetObjectTypes()
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var request = new ObjectTypeInfo.Command();
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("ModelInfos")]
    [ProducesResponseType(typeof(ModelBaseInfo.Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetModelInfos()
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var request = new ModelBaseInfo.Command();
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [AllowAnonymous]
    //[HttpGet("ModelDataIndexes")]
    [HttpGet("ModelIndexes")]
    [ProducesResponseType(typeof(ModelDataInfo.Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetModelDataIndexes()
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var request = new ModelDataInfo.Command();
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [AllowAnonymous]
    //[HttpGet("ModelDataIndexes")]
    [HttpGet("ComputerInfo/{computerId?}")]
    [ProducesResponseType(typeof(ComputerInfoModel.Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetComputerInfo(int? computerId)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var request = new ComputerInfoModel.Command() { ComputerId = computerId ?? 0 };
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [AllowAnonymous]
    //[HttpGet("ModelDataIndexes")]
    [HttpGet("ProgramInfo/{computerId?}")]
    [ProducesResponseType(typeof(ProgramInfoModel.Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProgramInfo(int? computerId)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var request = new ProgramInfoModel.Command { ComputerId = computerId ?? 0 };
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("StationDetailInfos")]
    [ProducesResponseType(typeof(StationDetailInfo.Response), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> StationDetailInfos()
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var request = new StationDetailInfo.Command();
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("EquipmentDetailInfos/{stationId?}")]
    [ProducesResponseType(typeof(EquipmentDetailInfo.Response), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> StationDetailInfos(long? stationId)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var request = new EquipmentDetailInfo.Command() { StationId = stationId ?? 0 };
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}

