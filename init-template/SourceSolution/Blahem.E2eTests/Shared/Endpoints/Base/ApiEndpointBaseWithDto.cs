using Blahem.E2eTests.Shared.Extensions;

namespace Blahem.E2eTests.Shared.Endpoints.Base;

internal abstract class ApiEndpointBaseWithDto<TDto>
{
    protected readonly HttpClient HttpClient;

    protected ApiEndpointBaseWithDto(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    public abstract Task<HttpResponseMessage> Call(TDto dto);
}

internal abstract class ApiEndpointBaseWithDto<TDto, TResponse> : ApiEndpointBaseWithDto<TDto>
{
    protected ApiEndpointBaseWithDto(HttpClient httpClient) :
        base(httpClient)
    {
    }

    public async Task<TResponse> CallAndParseResponse(TDto dto)
    {
        var response = await Call(dto);
        response.EnsureSuccessStatusCode();

        var content = await response.ReadResponseContentAs<TResponse>();

        return content;
    }
}
