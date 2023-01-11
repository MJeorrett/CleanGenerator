using Blahem.Application.Common.AppRequests;
using Microsoft.AspNetCore.Mvc;

namespace Blahem.WebApi.Extensions;

public static class AppResponseExtensions
{
    public static ActionResult<AppResponse> ToActionResult(this AppResponse responseModel)
    {
        ActionResult<AppResponse> result = responseModel.StatusCode switch
        {
            200 => new OkObjectResult(responseModel),
            400 => new BadRequestObjectResult(responseModel),
            401 => new UnauthorizedResult(),
            403 => new ForbidResult(),
            404 => new NotFoundObjectResult(responseModel),
            _ => new ObjectResult(responseModel) { StatusCode = responseModel.StatusCode },
        };

        return result;
    }

    public static ActionResult<AppResponse<T>> ToActionResult<T>(this AppResponse<T> responseModel)
    {
        ActionResult<AppResponse<T>> result = responseModel.StatusCode switch
        {
            200 => new OkObjectResult(responseModel),
            400 => new BadRequestObjectResult(responseModel),
            401 => new UnauthorizedResult(),
            403 => new ForbidResult(),
            404 => new NotFoundObjectResult(responseModel),
            _ => new ObjectResult(responseModel) { StatusCode = responseModel.StatusCode },
        };

        return result;
    }
}
