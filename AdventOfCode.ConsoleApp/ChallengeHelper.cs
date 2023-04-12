using System.Runtime.CompilerServices;

namespace AdventOfCode.ConsoleApp;
internal class ChallengeHelper
{
    public static string GetResourceFilePath([CallerFilePath] string callerFilePath = "")
    {
        var challengeName = Path.GetFileNameWithoutExtension(callerFilePath)?.Split("_")[1];
        var filePath = Path.Combine(callerFilePath, "..", "Resources", $"{challengeName}.txt");

        if (!File.Exists(filePath))
            File.Create(filePath);

        return filePath;
    }
}
