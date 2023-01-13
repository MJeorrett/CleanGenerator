using Blahem.Application.Common.AppRequests;
using Blahem.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Blahem.Application.Blaitems.Commands.Update;

public record UpdateBlaitemCommand
{
    [JsonIgnore]
    public int BlaitemId { get; init; }

    public string Title { get; init; } = "";
}

public class UpdateBlaitemCommandHandler : IRequestHandler<UpdateBlaitemCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateBlaitemCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppResponse> Handle(
        UpdateBlaitemCommand command,
        CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Blaitems
            .FirstOrDefaultAsync(_ => _.Id == command.BlaitemId, cancellationToken);

        if (entity == null)
        {
            return new(404);
        }

        entity.Title = command.Title;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new(200);
    }
}
