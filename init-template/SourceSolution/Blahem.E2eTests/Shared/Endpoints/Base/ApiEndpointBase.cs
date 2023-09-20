using Blahem.E2eTests.Shared.Extensions;
using FluentAssertions;

namespace Blahem.E2eTests.Shared.Endpoints.Base;

internal abstract class ApiEndpointBase<TResponse> : ApiEndpointBaseWithoutResponse
{
    protected ApiEndpointBase(HttpClient httpClient) :
        base(httpClient)
    {
    }

    public async Task<TResponse> CallAndParseResponse()
    {
        var response = await Call();
        await response.Should().HaveSuccessStatusCode();

        var content = await response.ReadResponseContentAs<TResponse>();

        return content;
    }
}

internal abstract class ApiEndpointBase<TDto, TResponse> : ApiEndpointBaseWithoutResponse<TDto>
{
    protected ApiEndpointBase(HttpClient httpClient) :
        base(httpClient)
    {
    }

    public async Task<TResponse> CallAndParseResponse(TDto dto)
    {
        var response = await Call(dto);
        await response.Should().HaveSuccessStatusCode();

        var content = await response.ReadResponseContentAs<TResponse>();

        return content;
    }
}
