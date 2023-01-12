using Blahem.Core.Entities;

namespace Blahem.Application.Blahems.Dtos;

public record BlahemDto
{
    public int Id { get; init; }

    public string Title { get; init; } = "";

    public static BlahemDto MapFromEntity(BlahemEntity entity)
    {
        return new()
        {
            Id = entity.Id,
            Title = entity.Title,
        };
    }
}
