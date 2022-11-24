using Business.API.Commands;
using Business.Engine.Models;
using Contracts.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Business.API;


[Route("api/v1/business/")]
public class BusinessController : Controller
{
    private readonly IMediator _mediator;

    public BusinessController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateBusinessProcess(CancellationToken cancellationToken)
    {
        var command = new CreateBusinessProcessCommand();
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("error/{id}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeStatusToError([FromRoute]Guid id, CancellationToken cancellationToken)
    {
        var command = new CreateCommand
        {
            Id = id,
            Type = StateWorkType.Error
        };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("end/{id}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeStatusToEnd([FromRoute]Guid id, CancellationToken cancellationToken)
    {
        var command = new CreateCommand
        {
            Id = id,
            Type = StateWorkType.End
        };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

}