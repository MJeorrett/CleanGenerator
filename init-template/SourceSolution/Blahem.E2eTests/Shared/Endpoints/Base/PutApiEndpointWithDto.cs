using System.Net.Http.Json;

namespace Blahem.E2eTests.Shared.Endpoints.Base;

internal abstract class PutApiEndpointWithDto<TDto, TResponse> : ApiEndpointBaseWithDto<TDto, TResponse>
{
    protected PutApiEndpointWithDto(HttpClient httpClient) :
        base(httpClient)
    {
    }

    internal abstract string BuildPath(TDto dto);

    public override Task<HttpResponseMessage> Call(TDto dto)
    {
        var uri = BuildPath(dto);

        return HttpClient.PutAsJsonAsync(uri, dto);
    }
}