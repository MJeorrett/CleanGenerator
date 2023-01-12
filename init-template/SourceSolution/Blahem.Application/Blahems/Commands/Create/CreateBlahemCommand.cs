using Blahem.Application.Common.AppRequests;
using Blahem.Application.Common.Interfaces;
using Blahem.Core.Entities;

namespace Blahem.Application.Blahems.Commands.Create;

public class CreateBlahemCommand
{
    public string Title { get; init; } = null!;
}

public class CreateBlahemCommandHandler : IRequestHandler<CreateBlahemCommand, int>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateBlahemCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppResponse<int>> Handle(
        CreateBlahemCommand command,
        CancellationToken cancellationToken)
    {
        var entity = new BlahemEntity
        {
            Title = command.Title,
        };

        _dbContext.Blahems.Add(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new(201, entity.Id);
    }
}
