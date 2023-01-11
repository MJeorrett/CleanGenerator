namespace CleanGenerator;

public record TemplateModel
{
    public required string ProjectName { get; init; }

    public required string EntityTypeName { get; init; }

    public required string EntityVariableName { get; init; }

    public required string ApiBasePath { get; init; }
}
