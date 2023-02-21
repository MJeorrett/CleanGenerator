namespace Blahem.E2eTests.Shared.Endpoints.Base;

internal abstract class DeleteApiEndpoint : ApiEndpointBaseWithDto<int>
{
    protected DeleteApiEndpoint(HttpClient httpClient) :
        base(httpClient)
    {
    }
}
