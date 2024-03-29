﻿using System.ComponentModel.DataAnnotations;

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

    public int? Length { get; init; }

    public bool IsNullable => Type.EndsWith("?");

    public string BuildPropertyDefault() => Type switch {
        "string" => " = \"\";",
        _ => "",
    };

    public string TestValue(int index) => Type switch
    {
        "string" or "string?" => $"\"Test {Name} {index + 1}\"",
        "int" or "int?" => Math.Pow(2, index).ToString(),
        _ => throw new NotImplementedException($"No test value set up for type {Type}."),
    };

    public string UpdatedTestValue(int index) => Type switch
    {
        "string" or "string?" => $"\"Updated {Name} {index + 1}\"",
        "int" or "int?" => (Math.Pow(2, index + 1) + 1).ToString(),
        _ => throw new NotImplementedException($"No test value set up for type {Type}."),
    };

    public string BuildValidationRules()
    {
        List<string> components = new();

        if (!IsNullable)
        {
            components.Add("            .NotNull()");

            if (Type == "string")
            {
                components.Add("            .NotEmpty()");
            }
        }

        if (Length is not null)
        {
            components.Add($"           .MaximumLength({Length})");
        }

        if (components.Count == 0) return "";

        components[components.Count - 1] = components[components.Count - 1] + ";";

        return $"RuleFor(_ => _.{Name}){Environment.NewLine}" + string.Join(Environment.NewLine, components);
    }
}
