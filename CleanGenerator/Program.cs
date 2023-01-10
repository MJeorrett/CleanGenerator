// See https://aka.ms/new-console-template for more information

using CleanGenerator;

Console.WriteLine("Hello, World!");

GenerateTemplate();

static void GenerateTemplate()
{
    var test = new CommandHandler("blahem");
    var text = test.TransformText();
    File.WriteAllText("TestCommand.cs", text);
}