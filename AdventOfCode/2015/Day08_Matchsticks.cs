
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Challenge(2015, 8)]
internal class Day08_Matchsticks
{
    private string? _testData;

    [Setup]
    public void Setup() => _testData = GetTestData();

    [Part(1)]
    public void ExecutePart01()
    {
        var inCode = NumberOfInCodeChars(_testData);
        var inMemory = NumberOfInMemoryChars(_testData);

        Console.WriteLine($"{inCode} (inCode) - {inMemory} (inMemory) = {inCode - inMemory}");
    }

    [Part(2)]
    public void ExecutePart02()
    {
        var inCode = NumberOfInCodeChars(_testData);
        var encoded = NumberOfEncodedChars(_testData);

        Console.WriteLine($"{encoded} (encoded) - {inCode} (inCode)  = {encoded - inCode}");
    }

    private static string GetTestData()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        return File.ReadAllText(filePath);
    }

    public static int NumberOfInCodeChars(string? input) => input?.Split("\r\n").Sum(s => s.Length) ?? 0;

    public static int NumberOfInMemoryChars(string? input)
    {
        if (string.IsNullOrEmpty(input)) return 0;

        var split = input.Split("\r\n");

        int count = 0;
        foreach (var s in split)
        {
            var trim = s[1..^1];

#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
            var replaced = Regex.Replace(trim, "\\\\\"|\\\\x[\\w\\d]{2}|\\\\\\\\", ".");
#pragma warning restore SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
            count += replaced.Length;
        }

        return count;
    }

    public static int NumberOfEncodedChars(string? input)
    {
        if (string.IsNullOrEmpty(input)) return 0;

        var split = input.Split("\r\n");

        int count = 0;
        foreach (var s in split)
        {

            var encoded = Encode(s);

            count += encoded.Length;
        }

        return count;
    }

    public static string Encode(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        var s0 = input.Replace("\\", "\\\\");
        var s1 = s0.Replace("\"", "\\\"");
        return $"\"{s1}\"";
    }
}
