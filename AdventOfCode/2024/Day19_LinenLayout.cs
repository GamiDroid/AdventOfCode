namespace AdventOfCode._2024;
[Challenge(2024, 19, "Linen Layout")]
internal class Day19_LinenLayout
{
    private readonly string[] _patterns;
    private readonly string[] _designs;

    public Day19_LinenLayout()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();
        List<string> patterns = [];
        List<string> designs = [];
        bool isPatterns = true;
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                isPatterns = false;
                continue;
            }

            if (isPatterns)
            {
                var patternsOnLine = line.Split(", ");
                patterns.AddRange(patternsOnLine);
            }
            else
            {
                designs.Add(line);
            }
        }

        _patterns = [.. patterns];
        _designs = [.. designs];
    }

    [Part(1)]
    public void Part01()
    {
        int possibleDesignCount = 0;
        foreach (var design in _designs)
        {
            possibleDesignCount += IsPossible(design, _patterns) ? 1 : 0;
        }

        Console.WriteLine($"Part 1: {possibleDesignCount}");
    }

    [Part(2)]
    public void Part02()
    {
        long totalPossibleDesignCount = 0;
        foreach (var design in _designs)
        {
            var possibleDesignCount = CountPossibleArrangements(design, _patterns);
            totalPossibleDesignCount += possibleDesignCount;
        }

        Console.WriteLine($"Part 2: {totalPossibleDesignCount}");
    }

    private static bool IsPossible(string design, string[] patterns)
    {
        return IsPossibleUtil(design, patterns, 0);
    }

    private static bool IsPossibleUtil(string design, string[] patterns, int idx)
    {
        if (idx >= design.Length - 1)
        {
            return true;
        }

        var possiblePatterns = patterns.Where(p => p.Length <= design.Length - idx && p.StartsWith(design[idx])).ToArray();
        foreach (var pattern in possiblePatterns.OrderByDescending(p => p.Length))
        {
            if (design.Substring(idx, pattern.Length) == pattern)
            {
                if (IsPossibleUtil(design, patterns, idx + pattern.Length))
                    return true;
            }
        }

        return false;
    }

    private static long CountPossibleArrangements(string design, string[] patterns)
    {
        return CountPossibleArrangementsUtil(design, patterns, 0, 0, []);
    }

    private static long CountPossibleArrangementsUtil(string design, string[] patterns, int idx, long current, Dictionary<string, long> found)
    {
        if (idx >= design.Length - 1)
        {
            return current + 1;
        }

        var possiblePatterns = patterns.Where(p => p.Length <= design.Length - idx && p.StartsWith(design[idx])).ToArray();
        foreach (var pattern in possiblePatterns.OrderBy(p => p.Length))
        {
            if (design.Substring(idx, pattern.Length) == pattern)
            {
                if (found.ContainsKey(design[idx..]))
                {
                    current += found[design[idx..]];
                }
                else
                {
                    current = CountPossibleArrangementsUtil(design, patterns, idx + pattern.Length, current, found);
                    found[design[idx..]] = current;
                }
            }
        }

        return current;
    }
}
