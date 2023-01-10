using Blahem.Application.Blahems.Commands.Create;
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
}