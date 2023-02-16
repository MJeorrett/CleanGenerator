using Blahem.Application.Blaitems.Dtos;
using Blahem.Application.Common.AppRequests;
using Blahem.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blahem.Application.Blaitems.Queries.List;

public record ListBlaitemsQuery
{

}

public class ListBlaitemsQueryHandler : IRequestHandler<ListBlaitemsQuery, List<BlaitemDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public ListBlaitemsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppResponse<List<BlaitemDto>>> Handle(
        ListBlaitemsQuery query,
        CancellationToken cancellationToken)
    {
        var dtos = await _dbContext.Blaitems
            .Select(_ => BlaitemDto.MapFromEntity(_))
            .ToListAsync(cancellationToken);

        return new(200, dtos);
    }
}
