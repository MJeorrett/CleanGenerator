using Blahem.Core.Entities;

namespace Blahem.Application.Blaitems.Dtos;

public record BlaitemDto
{
    public int Id { get; init; }

    public string Title { get; init; } = "";

    public static BlaitemDto MapFromEntity(BlaitemEntity entity)
    {
        return new()
        {
            Id = entity.Id,
            Title = entity.Title,
        };
    }
}
