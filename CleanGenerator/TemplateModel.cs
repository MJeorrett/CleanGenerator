namespace CleanGenerator;

public record TemplateModel
{
    public required string ProjectName { get; init; }

    public required string EntityTypeName { get; init; }

    public required List<EntityPropertyConfiguration> PropertyConfigs { get; init; }

    public string EntityVariableName => FirstCharToLowerCase(EntityTypeName) ?? "";

    public required string ApiBasePath { get; init; }

    public TemplateModel CloneWithoutIdPropertyConfig()
    {
        return this with
        {
            PropertyConfigs = PropertyConfigs.Where(_ => _.Name != "Id").ToList()
        };
    }

    public static string? FirstCharToLowerCase(string? str)
    {
        if (!string.IsNullOrEmpty(str) && char.IsUpper(str[0]))
            return str.Length == 1 ? char.ToLower(str[0]).ToString() : char.ToLower(str[0]) + str[1..];

        return str;
    }
}
