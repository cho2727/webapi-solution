using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Features.Server;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServerController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServerController(IMediator mediator)
    {
        this._mediator = mediator;
    }


    [AllowAnonymous]
    [HttpPost("SendAgentCommand")]
    [ProducesResponseType(typeof(AgentSendMessage.Response), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SendAgentCommand(AgentSendMessage.Command request)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var response = await _mediator.Send(request);
        return Ok(response);
    }


    [AllowAnonymous]
    [HttpGet("RecvAgentCommand/{contrlBoxName}")]
    [ProducesResponseType(typeof(AgentReceiveMessage.Response), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RecvAgentCommand(string contrlBoxName)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var request = new AgentReceiveMessage.Command() { ControlBoxName = contrlBoxName };
        var response = await _mediator.Send(request);
        return Ok(response);
    }



    [AllowAnonymous]
    [HttpPost("ComputerStateUpdate")]
    [ProducesResponseType(typeof(ComputerStateMessage.Response), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ComputerStateUpdate(ComputerStateMessage.Command request)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("ProgramStateUpdate")]
    [ProducesResponseType(typeof(ProgramStateMessage.Response), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ProgramStateUpdate(ProgramStateMessage.Command request)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
