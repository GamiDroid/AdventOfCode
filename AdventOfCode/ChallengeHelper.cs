using System.Runtime.CompilerServices;

namespace AdventOfCode;
internal class ChallengeHelper
{
    public static string GetResourceFilePath([CallerFilePath] string callerFilePath = "")
    {
        return GetChallengeResourceFilePath(callerFilePath);
    }

    public static string[] ReadAllLinesFromResourceFile([CallerFilePath] string callerFilePath = "")
    {
        try
        {
            var inputPath = GetChallengeResourceFilePath(callerFilePath);
            return File.ReadAllLines(inputPath);
        }
        catch (IOException)
        {
            throw new InvalidOperationException("Resource file is just created. Add content!!!");
        }
    }

    public static string ReadAllTextFromResourceFile([CallerFilePath] string callerFilePath = "")
    {
        try
        {
            var inputPath = GetChallengeResourceFilePath(callerFilePath);
            return File.ReadAllText(inputPath);
        }
        catch (IOException)
        {
            throw new InvalidOperationException("Resource file is just created. Add content!!!");
        }
    }

    private static string GetChallengeResourceFilePath(string challengeFilePath)
    {
        var challengeName = Path.GetFileNameWithoutExtension(challengeFilePath)?.Split("_")[1];
        var filePath = Path.Combine(challengeFilePath, "..", "Resources", $"{challengeName}.txt");

        if (!File.Exists(filePath))
            File.Create(filePath);

        return filePath;
    }
}
