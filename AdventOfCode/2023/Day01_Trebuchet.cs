using System.Text.RegularExpressions;

namespace AdventOfCode._2023;

[Challenge(2023, 1, "Trebuchet?!")]
public class Day01_Trebuchet
{
    private string[] _lines = Array.Empty<string>();

    [Setup]
    public void Setup()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        _lines = File.ReadAllLines(filePath);
    }

    [Part(1)]
    public void Part01()
    {
        var sum = 0;

        foreach (var line in _lines)
        {
            var matches = Regex.Matches(line, "\\d");
            if (matches.Count > 0)
            {
                var firstMatch = matches[0];
                var lastMatch = matches[^1];

                var combined = $"{firstMatch.ValueSpan}{lastMatch.ValueSpan}";

                if (int.TryParse(combined, out int parsed))
                {
                    sum += parsed;
                }
            }
        }

        Console.WriteLine("Sum of first and last numbers: {0}", sum);
    }

    [Part(2)]
    public void Part02()
    {
        var sum = 0;

        var wordNumberDict = new Dictionary<string, string>()
        {
            ["one"] = "1",
            ["two"] = "2",
            ["three"] = "3",
            ["four"] = "4",
            ["five"] = "5",
            ["six"] = "6",
            ["seven"] = "7",
            ["eight"] = "8",
            ["nine"] = "9",
        };

        foreach (var line in _lines)
        {
            // use '(?=...)' positive look 
            var matches = Regex.Matches(line, "(?=(\\d|one|two|three|four|five|six|seven|eight|nine))");
            if (matches.Count > 0)
            {
                var firstValue = matches[0].Groups[1].Value;
                var lastValue = matches[^1].Groups[1].Value;

                if (!int.TryParse(firstValue, out int _))
                    firstValue = wordNumberDict[firstValue];

                if (!int.TryParse(lastValue, out int _))
                    lastValue = wordNumberDict[lastValue];

                var combined = $"{firstValue}{lastValue}";

                if (int.TryParse(combined, out int parsed))
                {
                    sum += parsed;
                }
            }
        }

        Console.WriteLine("Sum of first and last numbers: {0}", sum);
    }
}
    