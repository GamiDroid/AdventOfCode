using Microsoft.VisualBasic;
using System.Text.RegularExpressions;

namespace AdventOfCode._2023;

[Challenge(2023, 12, "Hot Springs")]
internal class Day12_HotSprings
{
    private readonly SpringRecord[] _springRecords;

    public Day12_HotSprings()
    {
        var text = ChallengeHelper.ReadAllTextFromResourceFile();
        var matches = Regex.Matches(text, "([.#?]+) ([\\d,]+)");

        _springRecords = matches
            .Select(m => new SpringRecord(
                Springs: m.Groups[1].Value,
                Groups: m.Groups[2].Value.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c.ToString())).ToArray()))
            .ToArray();
    }

    [Part(1)]
    public void Part01()
    {
        var sum = _springRecords.Sum(r => FindAmountOfDifferentArrangements(r.Springs, r.Groups));
        Console.WriteLine($"Sum of diffent arrangements is {sum}");
    }

    internal static int FindAmountOfDifferentArrangements(string springs, int[] groups)
    {
        int springsCount = springs.Length;
        int minimumSpringsNeeded = groups.Sum() + groups.Length - 1;

        if (springsCount == minimumSpringsNeeded)
            return 1;

        var damagedSpringsCount = springs.Count(x => x == '#');
        var damagedSpringsCountNeeded = groups.Sum();

        if (damagedSpringsCount == damagedSpringsCountNeeded)
            return 1;

        var damagedSpringsMissingCount = damagedSpringsCountNeeded - damagedSpringsCount;
        var unknownSpringLocations = Regex.Matches(springs, "\\?").Select(m => m.Index).ToArray();

        List<List<int>> combinations = [];
        FindCombinations(unknownSpringLocations, damagedSpringsMissingCount, ref combinations);

        int matches = 0;
        foreach (var combination in combinations)
        {
            var springsAsArr = springs.ToCharArray();
            foreach (var i in combination)
            {
                springsAsArr[i] = '#';
            }

            var ranges = FindDamagedSpringGroups(new string(springsAsArr)).Select(x => x.Length).ToArray();
            if (groups.SequenceEqual(ranges))
            {
                matches++;
            }
        }

        return matches;
    }

    private static List<GroupRange> FindDamagedSpringGroups(string springs)
    {
        var damagedSpringGroups = Regex.Matches(springs, "#+");
        var damagedGroups = damagedSpringGroups
            .Select(m => new GroupRange(m.Index, m.Length))
            .ToList();
        return damagedGroups;
    }

    static void FindCombinations(int[] arr, int x, ref List<List<int>> result, int start = 0, List<int>? path = null)
    {
        path ??= [];

        if (path.Count == x)
        {
            result.Add(new List<int>(path));
            return;
        }

        for (int i = start; i < arr.Length; i++)
        {
            path.Add(arr[i]);
            FindCombinations(arr, x, ref result, i + 1, path);
            path.RemoveAt(path.Count - 1);
        }
    }

    internal record SpringRecord(string Springs, int[] Groups);
    internal readonly record struct GroupRange
    {
        public GroupRange(int start, int length)
        {
            Start = start;
            Length = length;
            End = start + length;
        }

        public int Start { get; }
        public int End { get; }
        public int Length { get; }
    }
}