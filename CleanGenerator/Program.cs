// See https://aka.ms/new-console-template for more information

using CleanGenerator;
using System.Diagnostics;

Console.WriteLine("Hello, World!");

const string projectName = "Todo";
const string inputDirectory = "C:/git/github/mjeorrett/CleanGenerator/SourceSolution";
const string outputDirectory = "C:/git/github/mjeorrett/CleanGenerator/test-output";

if (Directory.EnumerateFileSystemEntries(outputDirectory).Any())
{
    throw new InvalidOperationException("Output directory is not empty.");
}

CopyFilesRecursively(inputDirectory, outputDirectory, projectName);

static void CopyFilesRecursively(string sourcePath, string targetPath, string projectName)
{
    //Now Create all of the directories
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

    //Copy all the files & Replaces any files with the same name
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