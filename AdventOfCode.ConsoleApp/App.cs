using AdventOfCode.ConsoleApp;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;

internal static class App
{
    public static string ProjectRootFolder { get; private set; } = string.Empty;
    public static IConfiguration? Configuration { get; private set; }
    public static AppOptions? Options { get; private set; }

    public static void Startup(string[] args)
    {
        Configuration = new ConfigurationBuilder()
            .AddCommandLine(args)
            .Build();

        Options = new AppOptions();
        Configuration.Bind(Options);

        SetProjectRootFolder();
    }

    private static void SetProjectRootFolder([CallerFilePath] string? callerFilePath = null)
    {
        ProjectRootFolder = Path.GetDirectoryName(callerFilePath) ?? string.Empty;
    }
}
