using Blahem.Application.Common.AppRequests.Pagination;
using Blahem.E2eTests.Shared.Dtos.Blaitems;
using Blahem.E2eTests.Shared.Endpoints.Base;
using System.Net.Http.Json;

namespace Blahem.E2eTests.Shared.Endpoints;

internal static class BlaitemHttpClientExtensions
{
    public static CreateBlaitemEndpoint CreateBlaitem(this HttpClient httpClient) => new(httpClient);
    public static GetBlaitemByIdEndpoint GetBlaitemById(this HttpClient httpClient) => new(httpClient);
    public static ListBlaitemsEndpoint ListBlaitems(this HttpClient httpClient) => new(httpClient);
    public static UpdateBlaitemEndpoint UpdateBlaitem(this HttpClient httpClient) => new(httpClient);
    public static DeleteBlaitemEndpoint DeleteBlaitem(this HttpClient httpClient) => new(httpClient);
}

internal class CreateBlaitemEndpoint : ApiEndpointBase<CreateBlaitemDto, int>
{
    internal CreateBlaitemEndpoint(HttpClient httpClient) :
        base(httpClient)
    {
    }

    public override async Task<HttpResponseMessage> Call(CreateBlaitemDto dto)
    {
        var path = "api/blaitems";

        return await HttpClient.PostAsJsonAsync(path, dto);
    }
}

internal class GetBlaitemByIdEndpoint : ApiEndpointBase<int, BlaitemDto>
{
    internal GetBlaitemByIdEndpoint(HttpClient httpClient) :
        base(httpClient)
    {
    }

    public override async Task<HttpResponseMessage> Call(int blaitemId)
    {
        var path = $"api/blaitems/{blaitemId}";

        return await HttpClient.GetAsync(path);
    }
}

internal class ListBlaitemsEndpoint : ApiEndpointBase<PaginatedListQuery, PaginatedListResponse<BlaitemDto>>
{
    public ListBlaitemsEndpoint(HttpClient httpClient) :
        base(httpClient)
    {
    }

    public override async Task<HttpResponseMessage> Call(PaginatedListQuery dto)
    {
        var path = $"api/blaitems?{dto.ToQueryString()}";

        return await HttpClient.GetAsync(path);
    }
}

internal class UpdateBlaitemEndpoint : ApiEndpointBaseWithoutResponse<UpdateBlaitemDto>
{
    internal UpdateBlaitemEndpoint(HttpClient httpClient) :
        base(httpClient)
    {
    }

    public override async Task<HttpResponseMessage> Call(UpdateBlaitemDto dto)
    {
        var path = $"api/blaitems/{dto.Id}";

        return await HttpClient.PutAsJsonAsync(path, dto);
    }
}

internal class DeleteBlaitemEndpoint : ApiEndpointBaseWithoutResponse<int>
{
    public DeleteBlaitemEndpoint(HttpClient httpClient) : base(httpClient)
    {
    }

    public override async Task<HttpResponseMessage> Call(int blaitemId)
    {
        return await HttpClient.DeleteAsync($"api/blaitems/{blaitemId}");
    }
}