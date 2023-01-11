using Blahem.Application.Blahems.Dtos;
using Blahem.Application.Common.AppRequests;
using Blahem.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blahem.Application.Blahems.Queries.List;

public record ListBlahemsQuery
{

}

public class ListBlahemsQueryHandler : IRequestHandler<ListBlahemsQuery, List<BlahemDto>>
{
    private readonly IBlahemDbContext _dbContext;

    public ListBlahemsQueryHandler(IBlahemDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppResponse<List<BlahemDto>>> Handle(
        ListBlahemsQuery query,
        CancellationToken cancellationToken)
    {
        var dtos = await _dbContext.Blahems
            .Select(_ => BlahemDto.MapFromEntity(_))
            .ToListAsync(cancellationToken);

        return new(200, dtos);
    }
}
