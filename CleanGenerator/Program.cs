﻿using CleanGenerator;
using CleanGenerator.Templates.Controller;
using CleanGenerator.Templates.CreateCommand;
using CleanGenerator.Templates.DeleteCommand;
using CleanGenerator.Templates.Dto;
using CleanGenerator.Templates.E2eTests;
using CleanGenerator.Templates.EntityTypeConfiguration;
using CleanGenerator.Templates.GetByIdQuery;
using CleanGenerator.Templates.ListQuery;
using CleanGenerator.Templates.TestDtos;
using CleanGenerator.Templates.TestEndpoints;
using CleanGenerator.Templates.UpdateCommand;
using System.CommandLine;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var outputDirectoryOption = new Option<string>("--output", "Root output path of scaffolded project.")
{
    IsRequired = true,
};

var projectNameOption = new Option<string>("--name", "Name of the project e.g. \"TodoApp\".")
{
    IsRequired = true,
};

var entityNameOption = new Option<string>("--entity", "Name of the new entity e.g. \"Todo\".")
{
    IsRequired = true,
};

var createMigrationOption = new Option<bool>("--migrate", "Create migration and updated database")
{
    IsRequired = false,
};

var rootCommand = new RootCommand("App for scaffolding clean architecture projects.");

var initCommand = new Command("init", "Initialise a clean architecture project.")
{
    outputDirectoryOption, projectNameOption, entityNameOption, createMigrationOption,
};

initCommand.SetHandler((outputDirectory, projectName, entityName, createMigration) =>
{
    var args = new CommandArgs
    {
        OutputDirectory = outputDirectory,
        ProjectName = projectName,
        EntityName = entityName,
    };

    RunInit(args);

    if (createMigration)
    {
        CreateMigration(args, "InitialCreate");
    }
}, outputDirectoryOption, projectNameOption, entityNameOption, createMigrationOption);

rootCommand.AddCommand(initCommand);

var addEntityCommand = new Command("add", "Add entity to existing clean architecture project.")
{
    outputDirectoryOption, entityNameOption, createMigrationOption,
};

addEntityCommand.SetHandler((outputDirectory, entityName, createMigration) =>
{
    string projectName = GetProjectName(outputDirectory);

    string entityFilePath = FindEntityFilePath(entityName, outputDirectory, projectName);

    var entityConfig = LoadAndValidateEntityClass(entityFilePath);

    var args = new CommandArgs
    {
        OutputDirectory = outputDirectory,
        ProjectName = projectName,
        EntityName = entityConfig.EntityName,
    };

    RunAddEntity(args, entityConfig.Properties);

    if (createMigration)
    {
        CreateMigration(args, $"Add{args.EntityName}Table");
    }

}, outputDirectoryOption, entityNameOption, createMigrationOption);

rootCommand.AddCommand(addEntityCommand);

rootCommand.Invoke(args);

static void RunInit(CommandArgs args)
{
    string inputDirectory = ResolveInputDirectory();

    Console.WriteLine("Loading template from " + inputDirectory);

    if (!Directory.Exists(args.OutputDirectory))
    {
        Console.WriteLine($"Directory '{args.OutputDirectory}' does not exist. Creating now...");
        Directory.CreateDirectory(args.OutputDirectory);
    }

    if (Directory.EnumerateFileSystemEntries(args.OutputDirectory).Any())
    {
        throw new InvalidOperationException("Output directory is not empty.");
    }

    CopyFilesRecursively(inputDirectory, args);
}

static void RunAddEntity(CommandArgs args, List<EntityPropertyConfiguration> propertyConfigs)
{
    var templateModel = new TemplateModel
    {
        ProjectName = args.ProjectName,
        EntityTypeName = args.EntityName.Replace("Entity", ""),
        ApiBasePath = args.EntityName.Replace("Entity", "").ToLower() + "s",
        PropertyConfigs = propertyConfigs,
    };

    GenerateAndWriteCreateCommandFile(templateModel, args);
    GenerateAndWriteCreateCommandValidatorFile(templateModel, args);
    GenerateAndWriteDeleteCommandFile(templateModel, args);
    GenerateAndWriteUpdateCommandFile(templateModel, args);
    GenerateAndWriteGetByIdQueryFile(templateModel, args);
    GenerateAndWriteListQueryFile(templateModel, args);
    GenerateAndWriteDtoFile(templateModel, args);
    GenerateAndWriteEntityTypeConfigurationFile(templateModel, args);
    GenerateAndWriteControllerFile(templateModel, args);
    GenerateAndWriteTestDtoFile(templateModel, args);
    GenerateAndWriteTestCreateDtoFile(templateModel, args);
    GenerateAndWriteTestUpdateDtoFile(templateModel, args);
    GenerateAndWriteTestEndpointsFile(templateModel, args);
    GenerateAndWriteE2eTestsFile(templateModel, args);

    UpdateDbContextInterface(templateModel, args);
    UpdateDbContext(templateModel, args);
}

static void CreateMigration(CommandArgs args, string migrationName)
{
    RunCmd($"dotnet ef migrations add {migrationName} --startup-project {Path.Join(args.OutputDirectory, args.ProjectName)}.WebApi --project {Path.Join(args.OutputDirectory, args.ProjectName)}.Infrastructure -o Persistence/Migrations");
    RunCmd($"dotnet ef database update --startup-project {Path.Join(args.OutputDirectory, args.ProjectName)}.WebApi --project {Path.Join(args.OutputDirectory, args.ProjectName)}.Infrastructure");
}

static void RunCmd(string command)
{
    System.Diagnostics.Process process = new System.Diagnostics.Process();
    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
    startInfo.FileName = "cmd.exe";
    startInfo.Arguments = $"/C {command}";
    process.StartInfo = startInfo;
    process.Start();
    process.WaitForExit();
}


static void CopyFilesRecursively(string sourcePath, CommandArgs args)
{
    // Create directories
    foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
    {
        if (IsPathExcluded(sourcePath, dirPath))
        {
            continue;
        }

        Directory.CreateDirectory(
            dirPath
                .Replace(sourcePath, args.OutputDirectory)
                .Replace("Blahem", args.ProjectName)
                .Replace("blahem", args.ProjectName.ToLower())
                .Replace("Blaitem", args.EntityName)
                .Replace("blaitem", args.EntityName.ToLower()));
    }

    // Copy files
    foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
    {
        if (IsPathExcluded(sourcePath, newPath))
        {
            continue;
        }

        var contents = File.ReadAllText(newPath);
        var updatedContents = contents
            .Replace("Blahem", args.ProjectName)
            .Replace("blahem", args.ProjectName.ToLower())
            .Replace("Blaitem", args.EntityName)
            .Replace("blaitem", args.EntityName.ToLower());

        File.WriteAllText(
            newPath
                .Replace(sourcePath, args.OutputDirectory)
                .Replace("Blahem", args.ProjectName)
                .Replace("blahem", args.ProjectName.ToLower())
                .Replace("Blaitem", args.EntityName)
                .Replace("blaitem", args.EntityName.ToLower()),
            updatedContents);
    }
}

static bool IsPathExcluded(string inputDirectory, string path)
{
    var relativePath = path.Substring(inputDirectory.Length);

    return relativePath.Contains("\\bin\\") ||
        relativePath.Contains("\\obj\\") ||
        relativePath.Contains("\\.vs\\") ||
        relativePath.Contains("\\.git\\") ||
        relativePath.EndsWith(".vs") ||
        relativePath.EndsWith(".git") ||
        relativePath.Contains(".Infrastructure\\Persistence\\Migrations");
}

static void GenerateAndWriteCreateCommandFile(TemplateModel model, CommandArgs args)
{
    var test = new CreateCommandTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(args.OutputDirectory, $"{args.ProjectName}.Application", model.EntityTypeName, "Commands", "Create");

    Directory.CreateDirectory(commandOutputDirectory);

    File.WriteAllText(Path.Join(commandOutputDirectory, $"Create{model.EntityTypeName}Command.cs"), text);
}

static void GenerateAndWriteCreateCommandValidatorFile(TemplateModel model, CommandArgs args)
{
    var test = new CreateCommandValidatorTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(args.OutputDirectory, $"{args.ProjectName}.Application", model.EntityTypeName, "Commands", "Create");

    Directory.CreateDirectory(commandOutputDirectory);

    File.WriteAllText(Path.Join(commandOutputDirectory, $"Create{model.EntityTypeName}CommandValidator.cs"), text);
}

static void GenerateAndWriteDeleteCommandFile(TemplateModel model, CommandArgs args)
{
    var test = new DeleteCommandTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(args.OutputDirectory, $"{args.ProjectName}.Application", model.EntityTypeName, "Commands", "Delete");

    Directory.CreateDirectory(commandOutputDirectory);

    File.WriteAllText(Path.Join(commandOutputDirectory, $"Delete{model.EntityTypeName}Command.cs"), text);
}

static void GenerateAndWriteUpdateCommandFile(TemplateModel model, CommandArgs args)
{
    var test = new UpdateCommandTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(args.OutputDirectory, $"{args.ProjectName}.Application", model.EntityTypeName, "Commands", "Update");

    Directory.CreateDirectory(commandOutputDirectory);

    File.WriteAllText(Path.Join(commandOutputDirectory, $"Update{model.EntityTypeName}Command.cs"), text);
}

static void GenerateAndWriteGetByIdQueryFile(TemplateModel model, CommandArgs args)
{
    var test = new GetByIdQueryTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(args.OutputDirectory, $"{args.ProjectName}.Application", model.EntityTypeName, "Queries", "GetById");

    Directory.CreateDirectory(commandOutputDirectory);

    File.WriteAllText(Path.Join(commandOutputDirectory, $"Get{model.EntityTypeName}ByIdQuery.cs"), text);
}

static void GenerateAndWriteListQueryFile(TemplateModel model, CommandArgs args)
{
    var test = new ListQueryTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(args.OutputDirectory, $"{args.ProjectName}.Application", model.EntityTypeName, "Queries", "List");

    Directory.CreateDirectory(commandOutputDirectory);

    File.WriteAllText(Path.Join(commandOutputDirectory, $"List{model.EntityTypeName}sQuery.cs"), text);
}

static void GenerateAndWriteDtoFile(TemplateModel model, CommandArgs args)
{
    var test = new DtoTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(args.OutputDirectory, $"{args.ProjectName}.Application", model.EntityTypeName, "Dtos");

    Directory.CreateDirectory(commandOutputDirectory);

    File.WriteAllText(Path.Join(commandOutputDirectory, $"{model.EntityTypeName}Dto.cs"), text);
}

static void GenerateAndWriteControllerFile(TemplateModel model, CommandArgs args)
{
    var test = new ControllerTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(args.OutputDirectory, $"{args.ProjectName}.WebApi", "Controllers");

    File.WriteAllText(Path.Join(commandOutputDirectory, $"{model.EntityTypeName}sController.cs"), text);
}

static void GenerateAndWriteEntityTypeConfigurationFile(TemplateModel model, CommandArgs args)
{
    var test = new EntityTypeConfigurationTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(args.OutputDirectory, $"{args.ProjectName}.Infrastructure", "Persistence", "EntityConfigurations");

    File.WriteAllText(Path.Join(commandOutputDirectory, $"{model.EntityTypeName}EntityTypeConfiguration.cs"), text);
}

static void GenerateAndWriteTestDtoFile(TemplateModel model, CommandArgs args)
{
    var test = new TestDtoTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(args.OutputDirectory, $"{args.ProjectName}.E2eTests", "Shared", "Dtos", $"{model.EntityTypeName}s");

    Directory.CreateDirectory(commandOutputDirectory);

    File.WriteAllText(Path.Join(commandOutputDirectory, $"{model.EntityTypeName}Dto.cs"), text);
}

static void GenerateAndWriteTestCreateDtoFile(TemplateModel model, CommandArgs args)
{
    var test = new TestCreateDtoTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(args.OutputDirectory, $"{args.ProjectName}.E2eTests", "Shared", "Dtos", $"{model.EntityTypeName}s");

    File.WriteAllText(Path.Join(commandOutputDirectory, $"Create{model.EntityTypeName}Dto.cs"), text);
}

static void GenerateAndWriteTestUpdateDtoFile(TemplateModel model, CommandArgs args)
{
    var test = new TestUpdateDtoTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(args.OutputDirectory, $"{args.ProjectName}.E2eTests", "Shared", "Dtos", $"{model.EntityTypeName}s");

    File.WriteAllText(Path.Join(commandOutputDirectory, $"Update{model.EntityTypeName}Dto.cs"), text);
}

static void GenerateAndWriteTestEndpointsFile(TemplateModel model, CommandArgs args)
{
    var test = new TestEndpointsTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(args.OutputDirectory, $"{args.ProjectName}.E2eTests", "Shared", "Endpoints");

    File.WriteAllText(Path.Join(commandOutputDirectory, $"{model.EntityTypeName}Endpoints.cs"), text);
}

static void GenerateAndWriteE2eTestsFile(TemplateModel model, CommandArgs args)
{
    var test = new E2eTestsTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(args.OutputDirectory, $"{args.ProjectName}.E2eTests", $"{model.EntityTypeName}s");

    Directory.CreateDirectory(commandOutputDirectory);

    File.WriteAllText(Path.Join(commandOutputDirectory, $"{model.EntityTypeName}E2eTests.cs"), text);
}

static void UpdateDbContextInterface(TemplateModel model, CommandArgs args)
{
    var path = Path.Join(args.OutputDirectory, $"{model.ProjectName}.Application", "Common", "Interfaces", "IApplicationDbContext.cs");

    var lines = File.ReadAllLines(path).ToList();
    var lastDbSetLine = lines.Last(_ => _.Contains("DbSet<"));
    var lastDbSetLineNumber = lines.LastIndexOf(lastDbSetLine);

    lines.Insert(lastDbSetLineNumber, $"    public DbSet<{model.EntityTypeName}Entity> {model.EntityTypeName}s {{ get; }}");

    File.WriteAllLines(path, lines);
}

static void UpdateDbContext(TemplateModel model, CommandArgs args)
{
    var path = Path.Join(args.OutputDirectory, $"{model.ProjectName}.Infrastructure", "Persistence", "ApplicationDbContext.cs");

    var lines = File.ReadAllLines(path).ToList();
    var lastDbSetLine = lines.Last(_ => _.Contains("DbSet<"));
    var lastDbSetLineNumber = lines.LastIndexOf(lastDbSetLine);

    lines.Insert(lastDbSetLineNumber, $"    public DbSet<{model.EntityTypeName}Entity> {model.EntityTypeName}s {{ get; init; }} = null!;");

    File.WriteAllLines(path, lines);
}

static string ResolveInputDirectory()
{

    if (Environment.GetEnvironmentVariable("isDev") == "true")
    {
        return new FileInfo(Assembly.GetExecutingAssembly().Location).Directory + "/SourceSolution";
    }
    else
    {
        var contentDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory?.Parent?.Parent?.Parent;

        if (contentDirectory is null)
        {
            throw new Exception("Failed to locate content directory.");
        }

        return Path.Join(contentDirectory.FullName, "SourceSolution");
    }

}

static string GetProjectName(string outputDirectory)
{
    try
    {
        var webApiDirectory = new DirectoryInfo(outputDirectory).GetDirectories().ToList()
            .FirstOrDefault(_ => _.Name.Contains(".WebApi"));

        return webApiDirectory!.Name.Split(".").First();
    }
    catch (Exception exception)
    {
        throw new Exception("Failed to resolve project name.", exception);
    }
}

static EntityConfiguration LoadAndValidateEntityClass(string entityFilePath)
{
    var sourceCode = File.ReadAllText(entityFilePath);
    var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
    var root = syntaxTree.GetCompilationUnitRoot();

    var entityClass = root.DescendantNodes()
        .OfType<ClassDeclarationSyntax>()
        .FirstOrDefault();

    if (entityClass == null)
    {
        throw new Exception("No class found in the provided entity file.");
    }

    var entityProperties = entityClass.DescendantNodes()
        .OfType<PropertyDeclarationSyntax>();

    var entityConfig = new EntityConfiguration
    {
        EntityName = entityClass.Identifier.Text,
        Properties = entityProperties.Select(p => new EntityPropertyConfiguration
        {
            Name = p.Identifier.Text,
            Type = p.Type.ToString()
        }).ToList()
    };

    var validator = new EntityConfigurationValidator();
    var validationResult = validator.Validate(entityConfig);

    if (!validationResult.IsValid)
    {
        throw new Exception("Entity configuration is invalid:\n" + string.Join("\n", validationResult.Errors.Select(_ => "  - " + _.ToString())));
    }

    return entityConfig;
}

static string FindEntityFilePath(string entityName, string outputDirectory, string projectName)
{
    var entitiesFolderPath = Path.Combine(outputDirectory, $"{projectName}.Core", "Entities");
    var entityFileName = entityName + "Entity.cs";

    var entityFilePath = Directory
        .EnumerateFiles(entitiesFolderPath, entityFileName, SearchOption.AllDirectories)
        .FirstOrDefault();

    if (entityFilePath == null)
    {
        throw new FileNotFoundException($"Entity file '{entityFileName}' not found in '{entitiesFolderPath}' or its subdirectories.");
    }

    return entityFilePath;
}