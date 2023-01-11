// See https://aka.ms/new-console-template for more information

using CleanGenerator;

Console.WriteLine("Hello, World!");

const string projectName = "MicahelDemo";
const string inputDirectory = "C:/git/github/mjeorrett/CleanGenerator/SourceSolution";
const string outputDirectory = "C:/git/github/mjeorrett/CleanGenerator/test-output";

if (Directory.EnumerateFileSystemEntries(outputDirectory).Any())
{
    throw new InvalidOperationException("Output directory is not empty.");
}


var templateModel = new TemplateModel
{
    ProjectName = projectName,
    EntityTypeName = "Cat",
    ApiBasePath = "cats",
};

CopyFilesRecursively(inputDirectory, outputDirectory, projectName);

CreateCrud(templateModel, outputDirectory);

static void CreateCrud(TemplateModel templateModel, string outputBasePath)
{
    GenerateAndWriteCreateCommandFile(templateModel, outputDirectory);
    GenerateAndWriteUpdateCommandFile(templateModel, outputDirectory);
    GenerateAndWriteGetByIdQueryFile(templateModel, outputDirectory);
    GenerateAndWriteListQueryFile(templateModel, outputDirectory);
    GenerateAndWriteDtoFile(templateModel, outputDirectory);
    GenerateAndWriteEntityFile(templateModel, outputDirectory);
    GenerateAndWriteControllerFile(templateModel, outputDirectory);

    UpdateDbContextInterface(templateModel, outputDirectory);
    UpdateDbContext(templateModel, outputDirectory);

    RunCmd($"dotnet ef migrations add InitialCreate --startup-project {Path.Join(outputDirectory, projectName)}.WebApi --project {Path.Join(outputDirectory, projectName)}.Infrastructure");
    RunCmd($"dotnet ef database update --startup-project {Path.Join(outputDirectory, projectName)}.WebApi --project {Path.Join(outputDirectory, projectName)}.Infrastructure");
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


static void CopyFilesRecursively(string sourcePath, string targetPath, string projectName)
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
                .Replace(sourcePath, targetPath)
                .Replace("Blahem", projectName)
                .Replace("blahem", projectName.ToLower()));
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
            .Replace("Blahem", projectName)
            .Replace("blahem", projectName.ToLower());

        File.WriteAllText(
            newPath
                .Replace(sourcePath, targetPath)
                .Replace("Blahem", projectName)
                .Replace("blahem", projectName.ToLower()),
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

static void GenerateAndWriteCreateCommandFile(TemplateModel model, string outputBasePath)
{
    var test = new CreateCommandTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(outputBasePath, $"{projectName}.Application", model.EntityTypeName, "Commands", "Create");

    Directory.CreateDirectory(commandOutputDirectory);

    File.WriteAllText(Path.Join(commandOutputDirectory, $"Create{model.EntityTypeName}Command.cs"), text);
}

static void GenerateAndWriteUpdateCommandFile(TemplateModel model, string outputBasePath)
{
    var test = new UpdateCommandTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(outputBasePath, $"{projectName}.Application", model.EntityTypeName, "Commands", "Update");

    Directory.CreateDirectory(commandOutputDirectory);

    File.WriteAllText(Path.Join(commandOutputDirectory, $"Update{model.EntityTypeName}Command.cs"), text);
}

static void GenerateAndWriteGetByIdQueryFile(TemplateModel model, string outputBasePath)
{
    var test = new GetByIdQueryTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(outputBasePath, $"{projectName}.Application", model.EntityTypeName, "Queries", "GetById");

    Directory.CreateDirectory(commandOutputDirectory);

    File.WriteAllText(Path.Join(commandOutputDirectory, $"Get{model.EntityTypeName}ByIdQuery.cs"), text);
}

static void GenerateAndWriteListQueryFile(TemplateModel model, string outputBasePath)
{
    var test = new ListQueryTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(outputBasePath, $"{projectName}.Application", model.EntityTypeName, "Queries", "List");

    Directory.CreateDirectory(commandOutputDirectory);

    File.WriteAllText(Path.Join(commandOutputDirectory, $"List{model.EntityTypeName}sQuery.cs"), text);
}

static void GenerateAndWriteDtoFile(TemplateModel model, string outputBasePath)
{
    var test = new DtoTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(outputBasePath, $"{projectName}.Application", model.EntityTypeName, "Dtos");

    Directory.CreateDirectory(commandOutputDirectory);

    File.WriteAllText(Path.Join(commandOutputDirectory, $"{model.EntityTypeName}Dto.cs"), text);
}

static void GenerateAndWriteEntityFile(TemplateModel model, string outputBasePath)
{
    var test = new EntityTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(outputBasePath, $"{projectName}.Core", "Entities");

    File.WriteAllText(Path.Join(commandOutputDirectory, $"{model.EntityTypeName}Entity.cs"), text);
}

static void GenerateAndWriteControllerFile(TemplateModel model, string outputBasePath)
{
    var test = new ControllerTemplate(model);

    var text = test.TransformText();

    var commandOutputDirectory = Path.Join(outputBasePath, $"{projectName}.WebApi", "Controllers");

    File.WriteAllText(Path.Join(commandOutputDirectory, $"{model.EntityTypeName}sController.cs"), text);
}

static void UpdateDbContextInterface(TemplateModel model, string outputBasePath)
{
    var path = Path.Join(outputBasePath, $"{model.ProjectName}.Application", "Common", "Interfaces", $"I{model.ProjectName}DbContext.cs");

    var lines = File.ReadAllLines(path).ToList();
    var lastDbSetLine = lines.Last(_ => _.Contains("DbSet<"));
    var lastDbSetLineNumber = lines.LastIndexOf(lastDbSetLine);

    lines.Insert(lastDbSetLineNumber, $"    public DbSet<{model.EntityTypeName}Entity> {model.EntityTypeName}s {{ get; }}");

    File.WriteAllLines(path, lines);
}

static void UpdateDbContext(TemplateModel model, string outputBasePath)
{
    var path = Path.Join(outputBasePath, $"{model.ProjectName}.Infrastructure", "Persistence", $"{model.ProjectName}DbContext.cs");

    var lines = File.ReadAllLines(path).ToList();
    var lastDbSetLine = lines.Last(_ => _.Contains("DbSet<"));
    var lastDbSetLineNumber = lines.LastIndexOf(lastDbSetLine);

    lines.Insert(lastDbSetLineNumber, $"    public DbSet<{model.EntityTypeName}Entity> {model.EntityTypeName}s {{ get; init; }} = null!;");

    File.WriteAllLines(path, lines);
}