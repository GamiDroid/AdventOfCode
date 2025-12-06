using System.Text.RegularExpressions;
using Range = (long Start, long End);

namespace AdventOfCode._2025;

[Challenge(2025, 5, "Cafeteria")]
internal class Day05_Cafeteria
{
    private readonly Range[] _ranges;
    private readonly long[] _ingredients;

    public Day05_Cafeteria()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile().ToList();

        var emptyLine = lines.IndexOf(string.Empty);
        if (emptyLine == -1)
            throw new InvalidDataException("There is no empty line found");

        var rangeLines = lines.Take(emptyLine);
        var ingredientsLines = lines.Skip(emptyLine + 1);

        _ranges = [.. rangeLines
            .Select(x => Regex.Match(x, "(\\d+)-(\\d+)").Groups)
            .Select(g => (g[1].ToInt64(), g[2].ToInt64()))];

        _ingredients = [.. ingredientsLines.Select(long.Parse)];
    }

    [Part(1)]
    public void Part1()
    {
        var freshCount = _ingredients.Count(i => _ranges.Any(r => r.Start <= i && i <= r.End));

        Console.WriteLine($"Fresh ingredients: {freshCount}");
    }
}
