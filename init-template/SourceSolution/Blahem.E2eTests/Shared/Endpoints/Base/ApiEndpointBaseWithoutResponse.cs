using Blahem.E2eTests.Shared.Extensions;
using FluentAssertions;

namespace Blahem.E2eTests.Shared.Endpoints.Base;

internal abstract class ApiEndpointBaseWithoutResponse
{
    protected readonly HttpClient HttpClient;

    protected ApiEndpointBaseWithoutResponse(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    public abstract Task<HttpResponseMessage> Call();

    public async Task CallAndEnsureSuccess()
    {
        var response = await Call();
        await response.Should().HaveSuccessStatusCode();
    }
}

internal abstract class ApiEndpointBaseWithoutResponse<TDto>
{
    protected readonly HttpClient HttpClient;

    protected ApiEndpointBaseWithoutResponse(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    public abstract Task<HttpResponseMessage> Call(TDto dto);

    public async Task CallAndEnsureSuccess(TDto dto)
    {
        var response = await Call(dto);
        await response.Should().HaveSuccessStatusCode();
    }
}
