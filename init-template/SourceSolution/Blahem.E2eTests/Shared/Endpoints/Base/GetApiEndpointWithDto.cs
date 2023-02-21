namespace Blahem.E2eTests.Shared.Endpoints.Base;

internal abstract class GetApiEndpointWithDto<TDto, TResponse> : ApiEndpointBaseWithDto<TDto, TResponse>
{
    protected GetApiEndpointWithDto(HttpClient httpClient) :
        base(httpClient)
    {
    }

    internal abstract string BuildPath(TDto dto);

    public override Task<HttpResponseMessage> Call(TDto dto)
    {
        var uri = BuildPath(dto);

        return HttpClient.GetAsync(uri);
    }
}