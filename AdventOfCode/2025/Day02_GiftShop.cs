using System;

namespace AdventOfCode._2025;

[Challenge(2025, 2, "Gift Shop")]
internal class Day02_GiftShop
{
    private readonly (long Start, long End)[] _ranges;

    public Day02_GiftShop()
    {
        var input = ChallengeHelper.ReadAllTextFromResourceFile();

        _ranges = [.. input
            .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => x.Split("-", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Select(x => (long.Parse(x[0]), long.Parse(x[1])))];
    }

    [Part(1)]
    public void Part1()
    {
        long sum = 0;

        foreach (var (start, end) in _ranges)
        {
            for (var num = start; num <= end; num++)
            {
                var numAsSpan = num.ToString().AsSpan();

                // number has not even length
                if (numAsSpan.Length % 2 == 1)
                    continue;

                var firstHalf = numAsSpan[..(numAsSpan.Length / 2)];
                var secondHalf = numAsSpan[(numAsSpan.Length / 2)..];

                if (firstHalf.Equals(secondHalf, StringComparison.Ordinal))
                {
                    sum += num;
                }
            }
        }

        Console.WriteLine($"Sum of invalid IDs is {sum}");
    }

    [Part(2)]
    public void Part2()
    {
        long sum = 0;

        foreach (var (start, end) in _ranges)
        {
            for (var num = start; num <= end; num++)
            {
                var numAsSpan = num.ToString().AsSpan();

                var firstHalf = numAsSpan[..(numAsSpan.Length / 2)];
                var secondHalf = numAsSpan[(numAsSpan.Length / 2)..];

                if (firstHalf.Equals(secondHalf, StringComparison.Ordinal))
                {
                    sum += num;
                }
            }
        }

        Console.WriteLine($"Sum of invalid IDs is {sum}");
    }
}
