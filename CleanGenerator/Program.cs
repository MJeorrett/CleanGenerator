using CleanGenerator;
using CleanGenerator.Templates.Controller;
using CleanGenerator.Templates.CreateCommand;
using CleanGenerator.Templates.Dto;
using CleanGenerator.Templates.Entity;
using CleanGenerator.Templates.GetByIdQuery;
using CleanGenerator.Templates.ListQuery;
using CleanGenerator.Templates.UpdateCommand;
using System.CommandLine;
using System.Reflection;

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

var rootCommand = new RootCommand("App for scaffolding clean architecture projects.");

var initCommand = new Command("init", "Initialise a clean architecture project.")
{
    outputDirectoryOption, projectNameOption, entityNameOption,
};

initCommand.SetHandler((outputDirectory, projectName, entityName) =>
{
    var args = new CommandArgs
    {
        OutputDirectory = outputDirectory,
        ProjectName = projectName,
        EntityName = entityName,
    };

    RunInit(args);
}, outputDirectoryOption, projectNameOption, entityNameOption);

rootCommand.AddCommand(initCommand);

var addEntityCommand = new Command("add", "Add entity to existing clean architecture project.")
{
    outputDirectoryOption, projectNameOption, entityNameOption,
};

addEntityCommand.SetHandler((outputDirectory, projectName, entityName) =>
{
    var args = new CommandArgs
    {
        OutputDirectory = outputDirectory,
        ProjectName = projectName,
        EntityName = entityName,
    };

    RunAddEntity(args);
}, outputDirectoryOption, projectNameOption, entityNameOption);

rootCommand.AddCommand(addEntityCommand);

rootCommand.Invoke(args);

static void RunInit(CommandArgs args)
{
    string inputDirectory = ResolveInputDirectory();

    Console.WriteLine("Loading template from " + inputDirectory);

    if (Directory.EnumerateFileSystemEntries(args.OutputDirectory).Any())
    {
        throw new InvalidOperationException("Output directory is not empty.");
    }

    CopyFilesRecursively(inputDirectory, args);

    CreateMigration(args, "InitialCreate");
}

static void RunAddEntity(CommandArgs args)
{
    var templateModel = new TemplateModel
    {
        ProjectName = args.ProjectName,
        EntityTypeName = args.EntityName,
        ApiBasePath = args.EntityName.ToLower(),
    };

    CreateCrud(templateModel, args);
}

static void CreateCrud(TemplateModel templateModel, CommandArgs args)
{
    GenerateAndWriteCreateCommandFile(templateModel, args);
    GenerateAndWriteUpdateCommandFile(templateModel, args);
    GenerateAndWriteGetByIdQueryFile(templateModel, args);
    GenerateAndWriteListQueryFile(templateModel, args);
    GenerateAndWriteDtoFile(templateModel, args);
    GenerateAndWriteEntityFile(templateModel, args);
    GenerateAndWriteControllerFile(templateModel, args);

    UpdateDbContextInterface(templateModel, args);
    UpdateDbContext(templateModel, args);
    CreateMigration(args, $"Add{ args.EntityName}Table");
}
static void CreateMigration(CommandArgs args, string migrationName)
{
    RunCmd($"dotnet ef migrations add {migrationName} --startup-project {Path.Join(args.OutputDirectory, args.ProjectName)}.WebApi --project {Path.Join(args.OutputDirectory, args.ProjectName)}.Infrastructure");
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
                .Replace("blahem", args.ProjectName.ToLower()));
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
            .Replace("blahem", args.ProjectName.ToLower());

        File.WriteAllText(
            newPath
                .Replace(sourcePath, args.OutputDirectory)
                .Replace("Blahem", args.ProjectName)
                .Replace("blahem", args.ProjectName.ToLower()),
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
        relativePath.Contains("Blahem.Infrastructure\\Persistence\\Migrations");
}

static void GenerateAndWriteCreateCommandFile(TemplateModel model, CommandArgs args)
{
    var test = new CreateCommandTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(args.OutputDirectory, $"{args.ProjectName}.Application", model.EntityTypeName, "Commands", "Create");

    Directory.CreateDirectory(commandOutputDirectory);

    File.WriteAllText(Path.Join(commandOutputDirectory, $"Create{model.EntityTypeName}Command.cs"), text);
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

static void GenerateAndWriteEntityFile(TemplateModel model, CommandArgs args)
{
    var test = new EntityTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(args.OutputDirectory, $"{args.ProjectName}.Core", "Entities");

    File.WriteAllText(Path.Join(commandOutputDirectory, $"{model.EntityTypeName}Entity.cs"), text);
}

static void GenerateAndWriteControllerFile(TemplateModel model, CommandArgs args)
{
    var test = new ControllerTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(args.OutputDirectory, $"{args.ProjectName}.WebApi", "Controllers");

    File.WriteAllText(Path.Join(commandOutputDirectory, $"{model.EntityTypeName}sController.cs"), text);
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