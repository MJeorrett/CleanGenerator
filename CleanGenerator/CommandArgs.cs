namespace CleanGenerator;

internal record CommandArgs
{
    public required string ProjectName { get; init; }

    public required string OutputDirectory { get; init; }

    public required string EntityName { get; init; }
}
