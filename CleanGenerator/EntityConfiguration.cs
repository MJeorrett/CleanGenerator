using System.ComponentModel.DataAnnotations;

namespace CleanGenerator;

public record EntityConfiguration
{
    [Required]
    public string EntityName { get; init; } = null!;

    [Required]
    public List<EntityPropertyConfiguration> Properties { get; init; } = null!;
}

public record EntityPropertyConfiguration
{
    [Required]
    public string Name { get; init; } = null!;

    [Required]
    public string Type { get; init; } = null!;

    public string DefaultValue => Type == "string" ? "\"\"" : "0";

    public string ColumnType { get; init; } = null!;
}
