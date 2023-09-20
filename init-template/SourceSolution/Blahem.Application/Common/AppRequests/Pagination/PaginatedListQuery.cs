namespace Blahem.Application.Common.AppRequests.Pagination;

public record PaginatedListQuery
{
    public int PageIndex { get; init; }
    public int PageSize { get; init; }

    public string ToQueryString()
    {
        return $"pageIndex={PageIndex}&pageSize={PageSize}";
    }
}
