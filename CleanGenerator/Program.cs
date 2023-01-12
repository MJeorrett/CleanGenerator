using CleanGenerator;
using System.CommandLine;

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
rootCommand.AddOption(outputDirectoryOption);
rootCommand.AddOption(projectNameOption);
rootCommand.AddOption(entityNameOption);

rootCommand.SetHandler((outputDirectory, projectName, entityName) =>
{
    var args = new CommandArgs
    {
        OutputDirectory = outputDirectory,
        ProjectName = projectName,
        EntityName = entityName,
    };

    Run(args);
},
outputDirectoryOption, projectNameOption, entityNameOption);

rootCommand.Invoke(args);

static void Run(CommandArgs args)
{
    const string inputDirectory = "C:/git/github/mjeorrett/CleanGenerator/SourceSolution";

    if (Directory.EnumerateFileSystemEntries(args.OutputDirectory).Any())
    {
        throw new InvalidOperationException("Output directory is not empty.");
    }


    var templateModel = new TemplateModel
    {
        ProjectName = args.ProjectName,
        EntityTypeName = args.EntityName,
        ApiBasePath = args.EntityName.ToLower(),
    };

    CopyFilesRecursively(inputDirectory, args);

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

    RunCmd($"dotnet ef migrations add Add{args.EntityName}Table --startup-project {Path.Join(args.OutputDirectory, args.ProjectName)}.WebApi --project {Path.Join(args.OutputDirectory, args.ProjectName)}.Infrastructure");
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
        if (IsPathExcluded(dirPath))
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
        if (IsPathExcluded(newPath))
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

static bool IsPathExcluded(string path)
{
    return path.Contains("\\bin\\") ||
        path.Contains("\\obj\\") ||
        path.Contains("\\.vs\\") ||
        path.Contains("\\.git\\") ||
        path.Contains("\\test-output\\") ||
        path.Contains("\\CleanGenerator\\") ||
        path.EndsWith(".vs") ||
        path.EndsWith(".git") ||
        path.EndsWith("test-output") ||
        path.EndsWith("CleanGenerator") ||
        path.Contains("Blahem.Infrastructure\\Persistence\\Migrations");
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
