using Blahem.Application.Blahems.Commands.Create;
using Blahem.Application.Blahems.Commands.Update;
using Blahem.Application.Blahems.Dtos;
using Blahem.Application.Blahems.Queries.GetById;
using Blahem.Application.Blahems.Queries.List;
using Blahem.Application.Common.AppRequests;
using Blahem.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Blahem.WebApi.Controllers;

[ApiController]
public class BlahemController : ControllerBase
{
    [HttpPost("api/blahems")]
    public async Task<ActionResult<AppResponse<int>>> CreateBlahem(
        [FromBody] CreateBlahemCommand command,
        [FromServices] CreateBlahemCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var appResponse = await handler.Handle(command, cancellationToken);

        return appResponse.ToActionResult();
    }

    [HttpGet("api/blahems")]
    public async Task<ActionResult<AppResponse<List<BlahemDto>>>> ListBlahems(
        [FromServices] ListBlahemsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var appResponse = await handler.Handle(new(), cancellationToken);

        return appResponse.ToActionResult();
    }

    [HttpGet("api/blahems/{blahemId}")]
    public async Task<ActionResult<AppResponse<BlahemDto>>> ListBlahems(
        [FromRoute] int blahemId,
        [FromServices] GetBlahemByIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var appResponse = await handler.Handle(new() { BlahemId = blahemId }, cancellationToken);

        return appResponse.ToActionResult();
    }

    [HttpPut("api/blahems/{blahemId}")]
    public async Task<ActionResult<AppResponse>> UpdateBlahem(
        [FromRoute] int blahemId,
        [FromBody] UpdateBlahemCommand command,
        [FromServices] UpdateBlahemCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var appResponse = await handler.Handle(command with { BlahemId = blahemId }, cancellationToken);

        return appResponse.ToActionResult();
    }
}