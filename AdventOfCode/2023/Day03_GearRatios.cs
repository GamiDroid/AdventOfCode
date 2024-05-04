using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2023;

[Challenge(2023, 3, "Gear Ratios")]
internal class Day03_GearRatios
{
    private readonly string[] _lines;

    public Day03_GearRatios()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        _lines = File.ReadAllLines(filePath);
    }

    [Part(1)]
    public void Part01()
    {
        var allNumberMatches = new List<ValueMatch>();
        var allSymbolMatches = new List<ValueMatch>();

        for (int i = 0; i < _lines.Length; i++)
        {
            var line = _lines[i];
            var numberMatches = Regex.Matches(line, "\\d+");
            foreach (var match in numberMatches.Cast<Match>())
            {
                allNumberMatches.Add(new ValueMatch(i, match.Index, match.Length, match.ToInt32()));
            }

            var symbolMatches = Regex.Matches(line, "[^\\d.\\s]");
            foreach (var match in symbolMatches.Cast<Match>())
            {
                allSymbolMatches.Add(new ValueMatch(i, match.Index, 1, 0));
            }
        }

        var sum = 0;
        foreach (var numberMatch in allNumberMatches)
        {
            foreach (var (line, index) in GetSurroundingPositions(numberMatch))
            {
                Console.WriteLine($"try {numberMatch.Value}, {numberMatch.Index} {numberMatch.Length}: ({line}, {index})");
                if (allSymbolMatches.Any(m => m.Line == line && m.Index == index))
                {
                    Console.WriteLine(numberMatch.Value);
                    sum += numberMatch.Value;
                    break;
                }
            }
        }

        Console.WriteLine("Sum of numbers with symbol: {0}", sum);
    }

    private static IEnumerable<(int line, int index)> GetSurroundingPositions(ValueMatch valueMatch)
    {
        yield return (valueMatch.Line, valueMatch.Index - 1);
        yield return (valueMatch.Line, valueMatch.Index + valueMatch.Length);

        for (int i = -1; i < valueMatch.Length + 1; i++)
        {
            yield return (valueMatch.Line + 1, valueMatch.Index + i);
            yield return (valueMatch.Line - 1, valueMatch.Index + i);
        }
    }

    [Part(2)]
    public void Part02()
    {
        var allNumberMatches = new List<ValueMatch>();
        var allStarMatches = new List<ValueMatch>();

        for (int i = 0; i < _lines.Length; i++)
        {
            var line = _lines[i];
            var starMatches = Regex.Matches(line, "\\*");
            foreach (var match in starMatches.Cast<Match>())
            {
                allStarMatches.Add(new ValueMatch(i, match.Index, 1, 0));
            }

            var numberMatches = Regex.Matches(line, "\\d+");
            foreach (var match in numberMatches.Cast<Match>())
            {
                allNumberMatches.Add(new ValueMatch(i, match.Index, match.Length, match.ToInt32()));
            }
        }

        ICollection<int[]> numbersAroundStar = new List<int[]>();
        foreach (var starMatch in allStarMatches)
        {
            var coll = new HashSet<ValueMatch>();
            foreach (var (line, index) in GetSurroundingPositions(starMatch))
            {
                var match = allNumberMatches
                    .Where(x => x.Line == line && 
                        x.Index <= index && 
                        (x.Index + x.Length-1) >= index)
                    .FirstOrDefault();

                if (match.Length == 0)
                    continue;

                coll.Add(match);
            }

            numbersAroundStar.Add(coll.Select(x => x.Value).ToArray());
        }

        var sum = 0;
        foreach (var sums in numbersAroundStar.Where(x => x.Length == 2))
        {
            sum += sums[0] * sums[1];
        }

        Console.WriteLine("Sum of numbers around stars: {0}", sum);
    }

    record struct ValueMatch(int Line, int Index, int Length, int Value);
}
