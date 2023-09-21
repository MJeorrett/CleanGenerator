using Blahem.Application.Blaitems.Dtos;
using Blahem.Application.Common.AppRequests;
using Blahem.Application.Common.AppRequests.Pagination;
using Blahem.Application.Common.Interfaces;
using Blahem.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blahem.Application.Blaitems.Queries.List;

public record ListBlaitemsQuery : PaginatedListQuery
{

}

public class ListBlaitemsQueryHandler : IRequestHandler<ListBlaitemsQuery, PaginatedListResponse<BlaitemDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public ListBlaitemsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppResponse<PaginatedListResponse<BlaitemDto>>> Handle(
        ListBlaitemsQuery query,
        CancellationToken cancellationToken)
    {
        var blaitemQueryable = BuildQueryable(query);

        var result = await PaginatedListResponse<BlaitemDto>.Create(
            blaitemQueryable,
            query,
            entity => BlaitemDto.MapFromEntity(entity),
            cancellationToken);

        return new(200, result);
    }

    private IQueryable<BlaitemEntity> BuildQueryable(ListBlaitemsQuery query)
    {
        var queryable = _dbContext.Blaitems;

        return queryable;
    }
}
