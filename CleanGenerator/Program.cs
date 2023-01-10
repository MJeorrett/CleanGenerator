// See https://aka.ms/new-console-template for more information

using CleanGenerator;
using System.Diagnostics;

Console.WriteLine("Hello, World!");

const string projectName = "Todo";
const string inputDirectory = "C:/git/github/mjeorrett/CleanGenerator";
const string outputDirectory = "C:/git/github/mjeorrett/CleanGenerator/test-output";

if (Directory.EnumerateFileSystemEntries(outputDirectory).Any())
{
    throw new InvalidOperationException("Output directory is not empty.");
}

var applicationDirectory = Path.Join(outputDirectory, $"{projectName}.Application");
var coreDirectory = Path.Join(outputDirectory, $"{projectName}.Core");
var infrastructureDirectory = Path.Join(outputDirectory, $"{projectName}.Infrastructure");
var webApiDirectory = Path.Join(outputDirectory, $"{projectName}.WebApi");

Directory.CreateDirectory(applicationDirectory);
Directory.CreateDirectory(coreDirectory);
Directory.CreateDirectory(infrastructureDirectory);
Directory.CreateDirectory(webApiDirectory);

CopyFilesRecursively(Path.Join(inputDirectory, "Blahem.Application"), applicationDirectory, projectName);
CopyFilesRecursively(Path.Join(inputDirectory, "Blahem.Core"), coreDirectory, projectName);
CopyFilesRecursively(Path.Join(inputDirectory, "Blahem.Infrastructure"), infrastructureDirectory, projectName);
CopyFilesRecursively(Path.Join(inputDirectory, "Blahem.WebApi"), webApiDirectory, projectName);

static void CopyFilesRecursively(string sourcePath, string targetPath, string projectName)
{
    //Now Create all of the directories
    foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
    {
        if (dirPath.Contains("\\bin\\") ||
            dirPath.Contains("\\obj\\") ||
            dirPath.Contains("Blahem.Infrastructure\\Persistence\\Migrations"))
        {
            continue;
        }

        Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath).Replace("Blahem", projectName));
    }

    //Copy all the files & Replaces any files with the same name
    foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
    {
        if (newPath.Contains("\\bin\\") ||
            newPath.Contains("\\obj\\") ||
            newPath.Contains("Blahem.Infrastructure\\Persistence\\Migrations"))
        {
            continue;
        }

        var contents = File.ReadAllText(newPath);
        var updatedContents = contents.Replace("Blahem", projectName);
        File.WriteAllText(newPath.Replace(sourcePath, targetPath).Replace("Blahem", projectName), updatedContents);
    }
}

static void GenerateTemplate()
{
    var test = new CommandHandler("blahem");
    var text = test.TransformText();
    File.WriteAllText("TestCommand.cs", text);
}