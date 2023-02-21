using Blahem.E2eTests.Shared.Extensions;

namespace Blahem.E2eTests.Shared.Endpoints.Base;

internal abstract class ApiEndpointBase
{
    protected readonly HttpClient HttpClient;

    protected ApiEndpointBase(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    public abstract Task<HttpResponseMessage> Call();
}

internal abstract class ApiEndpointBase<TResponse> : ApiEndpointBase
{
    protected ApiEndpointBase(HttpClient httpClient) :
        base(httpClient)
    {
    }

    public async Task<TResponse> CallAndParseResponse()
    {
        var response = await Call();
        response.EnsureSuccessStatusCode();

        var content = await response.ReadResponseContentAs<TResponse>();

        return content;
    }
}
