using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2024;

[Challenge(2024, 1, "Historian Hysteria")]
internal class Day01_HistorianHysteria
{
    private readonly int[] _numbers1;
    private readonly int[] _numbers2;

    public Day01_HistorianHysteria()
    {
        var input = ChallengeHelper.ReadAllTextFromResourceFile();

        var matches = Regex.Matches(input, "(?<n1>\\d+)\\s+(?<n2>\\d+)");
        _numbers1 = matches.Select(x => x.Groups["n1"].ToInt32()).ToArray();
        _numbers2 = matches.Select(x => x.Groups["n2"].ToInt32()).ToArray();
    }

    [Part(1)]
    public void Part01()
    {
        var sum = _numbers1
            .Order()
            .Zip(_numbers2.Order())
            .Select(x => Math.Abs(x.First - x.Second))
            .Sum();

        Console.WriteLine($"Part 1: {sum}");
    }

    [Part(2)]
    public void Part02()
    {
        var sum = _numbers1
            .Select(x => x * _numbers2.Count(x1 => x1 == x))
            .Sum();

        Console.WriteLine($"Part 2: {sum}");
    }
}
