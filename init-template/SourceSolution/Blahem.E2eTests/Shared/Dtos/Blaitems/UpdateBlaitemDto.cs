namespace Blahem.E2eTests.Shared.Dtos.Blaitems;

internal record UpdateBlaitemDto
{
    public int Id { get; init; }

    public string Title { get; init; } = "";
}
