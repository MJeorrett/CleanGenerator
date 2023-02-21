using Blahem.E2eTests.Shared.Dtos.Blaitems;
using Blahem.E2eTests.Shared.Endpoints.Base;

namespace Blahem.E2eTests.Shared.Endpoints;

internal static class BlaitemHttpClientExtensions
{
    public static CreateBlaitemEndpoint CreateBlaitem(this HttpClient httpClient) => new(httpClient);
    public static GetBlaitemByIdEndpoint GetBlaitemById(this HttpClient httpClient) => new(httpClient);
    public static ListBlaitemsEndpoint ListBlaitems(this HttpClient httpClient) => new(httpClient);
    public static UpdateBlaitemEndpoint UpdateBlaitem(this HttpClient httpClient) => new(httpClient);
    public static DeleteBlaitemEndpoint DeleteBlaitem(this HttpClient httpClient) => new(httpClient);
}

internal class CreateBlaitemEndpoint : PostApiEndpointWithDto<CreateBlaitemDto, int>
{
    internal CreateBlaitemEndpoint(HttpClient httpClient) :
        base(httpClient)
    {
    }

    internal override string BuildPath(CreateBlaitemDto dto) => $"api/blaitems";
}

internal class GetBlaitemByIdEndpoint : GetApiEndpointWithDto<int, BlaitemDto>
{
    internal GetBlaitemByIdEndpoint(HttpClient httpClient) :
        base(httpClient)
    {
    }

    internal override string BuildPath(int blaitemId) => $"api/blaitems/{blaitemId}";
}

internal class ListBlaitemsEndpoint : GetApiEndpoint<List<BlaitemDto>>
{
    internal ListBlaitemsEndpoint(HttpClient httpClient) :
        base(httpClient, "api/blaitems")
    {
    }
}

internal class UpdateBlaitemEndpoint : PutApiEndpointWithDto<UpdateBlaitemDto, int>
{
    internal UpdateBlaitemEndpoint(HttpClient httpClient) :
        base(httpClient)
    {
    }

    internal override string BuildPath(UpdateBlaitemDto dto) => $"api/blaitems/{dto.Id}";
}

internal class DeleteBlaitemEndpoint : DeleteApiEndpoint
{
    public DeleteBlaitemEndpoint(HttpClient httpClient) : base(httpClient)
    {
    }

    public override async Task<HttpResponseMessage> Call(int blaitemId)
    {
        return await HttpClient.DeleteAsync($"api/blaitems/{blaitemId}");
    }
}
