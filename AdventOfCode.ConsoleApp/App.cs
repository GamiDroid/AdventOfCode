using System.Runtime.CompilerServices;

public static class App
{
    public static string ProjectRootFolder { get; private set; } = string.Empty;

    public static void SetProjectRootFolder([CallerFilePath] string? callerFilePath = null)
    {
        ProjectRootFolder = Path.GetDirectoryName(callerFilePath) ?? string.Empty;
    }
}
