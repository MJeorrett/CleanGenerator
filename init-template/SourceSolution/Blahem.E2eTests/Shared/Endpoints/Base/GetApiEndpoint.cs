namespace Blahem.E2eTests.Shared.Endpoints.Base;

internal class GetApiEndpoint<TResponse> : ApiEndpointBase<TResponse>
{
    private readonly string _path;

    protected GetApiEndpoint(HttpClient httpClient, string path) :
        base(httpClient)
    {
        _path = path;
    }

    public override Task<HttpResponseMessage> Call()
    {
        return HttpClient.GetAsync(_path);
    }
}