using Blahem.Application.Blahems.Dtos;
using Blahem.Application.Common.AppRequests;
using Blahem.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blahem.Application.Blahems.Queries.GetById;

public record GetBlahemByIdQuery
{
    public int BlahemId { get; init; }
}

public class GetBlahemByIdQueryHandler : IRequestHandler<GetBlahemByIdQuery, BlahemDto>
{
    private readonly IApplicationDbContext _dbContext;

    public GetBlahemByIdQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppResponse<BlahemDto>> Handle(
        GetBlahemByIdQuery query,
        CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Blahems
            .FirstOrDefaultAsync(_ => _.Id == query.BlahemId, cancellationToken);

        if (entity == null)
        {
            return new(404);
        }

        return new(200, BlahemDto.MapFromEntity(entity));
    }
}
