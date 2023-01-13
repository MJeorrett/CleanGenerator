using Blahem.Application.Common.AppRequests;
using Blahem.Application.Common.Interfaces;
using Blahem.Core.Entities;

namespace Blahem.Application.Blaitems.Commands.Create;

public class CreateBlaitemCommand
{
    public string Title { get; init; } = null!;
}

public class CreateBlaitemCommandHandler : IRequestHandler<CreateBlaitemCommand, int>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateBlaitemCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppResponse<int>> Handle(
        CreateBlaitemCommand command,
        CancellationToken cancellationToken)
    {
        var entity = new BlaitemEntity
        {
            Title = command.Title,
        };

        _dbContext.Blaitems.Add(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new(201, entity.Id);
    }
}
