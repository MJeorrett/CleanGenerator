using Blahem.Application.Blaitems.Dtos;
using Blahem.Application.Common.AppRequests;
using Blahem.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blahem.Application.Blaitems.Queries.GetById;

public record GetBlaitemByIdQuery
{
    public int BlaitemId { get; init; }
}

public class GetBlaitemByIdQueryHandler : IRequestHandler<GetBlaitemByIdQuery, BlaitemDto>
{
    private readonly IApplicationDbContext _dbContext;

    public GetBlaitemByIdQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppResponse<BlaitemDto>> Handle(
        GetBlaitemByIdQuery query,
        CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Blaitems
            .FirstOrDefaultAsync(_ => _.Id == query.BlaitemId, cancellationToken);

        if (entity == null)
        {
            return new(404);
        }

        return new(200, BlaitemDto.MapFromEntity(entity));
    }
}
