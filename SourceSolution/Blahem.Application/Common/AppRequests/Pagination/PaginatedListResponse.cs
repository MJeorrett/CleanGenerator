using Microsoft.EntityFrameworkCore;

namespace Blahem.Application.Common.AppRequests.Pagination;

public record PaginatedListResponse<T>(
    List<T> Items,
    int TotalCount,
    int TotalPages,
    int PageIndex,
    int PageSize)
{
    public bool HasPreviousPage => PageIndex > 0;

    public bool HasNextPage => PageIndex < (TotalPages - 1);

    public static async Task<PaginatedListResponse<T>> Create<Tin>(IQueryable<Tin> source, PaginatedListQuery query, Func<Tin, T> mapper, CancellationToken cancellationToken)
    {
        var pageIndex = query.PageIndex;
        var pageSize = query.PageSize;

        if (pageIndex < 0) throw new ArgumentOutOfRangeException(nameof(pageIndex), "Must be greater than or equal to 0.");
        if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "Must be greater than 0.");

        var totalCount = await source.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var flooredPageNumber = Math.Min(pageIndex, totalPages == 0 ? 0 : totalPages - 1);

        if (totalCount == 0) return new PaginatedListResponse<T>(new List<T>(), totalCount, totalPages, pageIndex, pageSize);

        var inputItems = await source
            .Skip((flooredPageNumber) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var outputItems = inputItems
            .Select(mapper)
            .ToList();

        return new PaginatedListResponse<T>(outputItems, totalCount, totalPages, flooredPageNumber, pageSize);
    }
}
