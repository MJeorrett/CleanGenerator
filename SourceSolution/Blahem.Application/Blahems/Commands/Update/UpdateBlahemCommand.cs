using Blahem.Application.Common.AppRequests;
using Blahem.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Blahem.Application.Blahems.Commands.Update;

public record UpdateBlahemCommand
{
    [JsonIgnore]
    public int BlahemId { get; init; }

    public string Title { get; init; } = "";
}

public class UpdateBlahemCommandHandler : IRequestHandler<UpdateBlahemCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateBlahemCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppResponse> Handle(
        UpdateBlahemCommand command,
        CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Blahems
            .FirstOrDefaultAsync(_ => _.Id == command.BlahemId, cancellationToken);

        if (entity == null)
        {
            return new(404);
        }

        entity.Title = command.Title;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new(200);
    }
}
