using System.ComponentModel.DataAnnotations;

namespace CleanGenerator;

internal record EntityConfiguration
{
    [Required]
    public string EntityName { get; init; } = null!;

    [Required]
    public List<EntityPropertyConfiguration> Properties { get; init; } = null!;
}

internal record EntityPropertyConfiguration
{
    [Required]
    public string Name { get; init; } = null!;

    [Required]
    public string Type { get; init; } = null!;

    public string ColumnType { get; init; } = null!;
}
