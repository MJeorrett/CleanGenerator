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

CopyFilesRecursively(inputDirectory, outputDirectory, projectName);

RunCmd($"dotnet ef migrations add InitialCreate --startup-project {Path.Join(outputDirectory, projectName)}.WebApi --project {Path.Join(outputDirectory, projectName)}.Infrastructure");
RunCmd($"dotnet ef database update --startup-project {Path.Join(outputDirectory, projectName)}.WebApi --project {Path.Join(outputDirectory, projectName)}.Infrastructure");

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

static void GenerateTemplate()
{
    var test = new CommandHandler("blahem");
    var text = test.TransformText();
    File.WriteAllText("TestCommand.cs", text);
}