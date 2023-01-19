using Blahem.Application.Common.AppRequests;
using Blahem.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blahem.Application.Blaitem.Commands.Delete;

public record DeleteBlaitemCommand
{
    public required int BlaitemId { get; init; }
}

public class DeleteBlaitemCommandHandler : IRequestHandler<DeleteBlaitemCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteBlaitemCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppResponse> Handle(
        DeleteBlaitemCommand command,
        CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Blaitems
            .FirstOrDefaultAsync(_ => _.Id == command.BlaitemId, cancellationToken);

        if (entity is null)
        {
            return new(404);
        }

        _dbContext.Blaitems.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new(200);
    }
}
