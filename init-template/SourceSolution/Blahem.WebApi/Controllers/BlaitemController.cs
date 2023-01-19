using Blahem.Application.Blaitem.Commands.Delete;
using Blahem.Application.Blaitems.Commands.Create;
using Blahem.Application.Blaitems.Commands.Update;
using Blahem.Application.Blaitems.Dtos;
using Blahem.Application.Blaitems.Queries.GetById;
using Blahem.Application.Blaitems.Queries.List;
using Blahem.Application.Common.AppRequests;
using Blahem.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Blahem.WebApi.Controllers;

[ApiController]
public class BlaitemController : ControllerBase
{
    [HttpPost("api/blaitems")]
    public async Task<ActionResult<AppResponse<int>>> CreateBlaitem(
        [FromBody] CreateBlaitemCommand command,
        [FromServices] CreateBlaitemCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var appResponse = await handler.Handle(command, cancellationToken);

        return appResponse.ToActionResult();
    }

    [HttpGet("api/blaitems")]
    public async Task<ActionResult<AppResponse<List<BlaitemDto>>>> ListBlaitems(
        [FromServices] ListBlaitemsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var appResponse = await handler.Handle(new(), cancellationToken);

        return appResponse.ToActionResult();
    }

    [HttpGet("api/blaitems/{blaitemId}")]
    public async Task<ActionResult<AppResponse<BlaitemDto>>> GetBlaitemById(
        [FromRoute] int blaitemId,
        [FromServices] GetBlaitemByIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var appResponse = await handler.Handle(new() { BlaitemId = blaitemId }, cancellationToken);

        return appResponse.ToActionResult();
    }

    [HttpPut("api/blaitems/{blaitemId}")]
    public async Task<ActionResult<AppResponse>> UpdateBlaitem(
        [FromRoute] int blaitemId,
        [FromBody] UpdateBlaitemCommand command,
        [FromServices] UpdateBlaitemCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var appResponse = await handler.Handle(command with { BlaitemId = blaitemId }, cancellationToken);

        return appResponse.ToActionResult();
    }

    [HttpDelete("api/blaitems/{blaitemId}")]
    public async Task<ActionResult<AppResponse>> DeleteBlaitem(
        [FromRoute] int blaitemId,
        [FromServices] DeleteBlaitemCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var appResponse = await handler.Handle(new() { BlaitemId = blaitemId }, cancellationToken);

        return appResponse.ToActionResult();
    }
}