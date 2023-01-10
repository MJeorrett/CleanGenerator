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

CopyFilesRecursively(inputDirectory, outputDirectory, projectName);

static void CopyFilesRecursively(string sourcePath, string targetPath, string projectName)
{
    //Now Create all of the directories
    foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
    {
        if (dirPath.Contains("\\bin\\") ||
            dirPath.Contains("\\obj\\") ||
            dirPath.Contains("\\.vs\\") ||
            dirPath.Contains("\\.git\\") ||
            dirPath.Contains("\\test-output\\") ||
            dirPath.EndsWith(".vs") ||
            dirPath.EndsWith(".git") ||
            dirPath.EndsWith("test-output") ||
            dirPath.Contains("Blahem.Infrastructure\\Persistence\\Migrations"))
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
        if (newPath.Contains("\\bin\\") ||
            newPath.Contains("\\obj\\") ||
            newPath.Contains("\\.vs\\") ||
            newPath.Contains("\\.git\\") ||
            newPath.Contains("\\test-output\\") ||
            newPath.EndsWith(".vs") ||
            newPath.EndsWith(".git") ||
            newPath.EndsWith("test-output") ||
            newPath.Contains("Blahem.Infrastructure\\Persistence\\Migrations"))
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

static void GenerateTemplate()
{
    var test = new CommandHandler("blahem");
    var text = test.TransformText();
    File.WriteAllText("TestCommand.cs", text);
}