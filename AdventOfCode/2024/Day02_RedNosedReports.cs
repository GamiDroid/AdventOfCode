using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024;

[Challenge(2024, 2, "Red-Nosed Reports")]
internal class Day02_RedNosedReports
{
    private readonly int[][] _reportLevels;

    public Day02_RedNosedReports()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();
        _reportLevels = lines.Select(r => r.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(l => int.Parse(l)).ToArray())
            .ToArray();
    }

    [Part(1)]
    public void Part01()
    {
        int counter = 0;
        foreach (var report in _reportLevels)
        {
            if (IsValidReport(report, out _))
                counter++;
        }

        Console.WriteLine($"Part 1: {counter}");
    }

    [Part(2)]
    public void Part02()
    {
        int counter = 0;
        foreach (var report in _reportLevels)
        {
            if (IsValidReport(report, out int failureIndex))
                counter++;
            else
            {
                var tempList = report.ToList();
                tempList.RemoveAt(failureIndex);

                if (!IsValidReport([.. tempList], out _))
                {
                    tempList = [.. report];
                    tempList.RemoveAt(failureIndex + 1);

                    if (!IsValidReport([.. tempList], out _))
                    {
                        continue;
                    }
                }

                counter++;
            }
        }

        Console.WriteLine($"Part 2: {counter}");
    }

    private static bool IsValidReport(int[] report, out int failureIndex)
    {
        bool isDescending = IsDescending(report);
        failureIndex = -1;
        for (int i = 0; i < report.Length-1; i++)
        {
            var diff = report[i + 1] - report[i];

            if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3)
            {
                failureIndex = i;
                return false;
            }

            if (isDescending && diff > 0)
            {
                failureIndex = i;
                return false;
            }
            else if (!isDescending && diff < 0)
            {
                failureIndex = i;
                return false;
            }
        }

        return true;
    }

    static bool IsDescending(int[] report)
    {
        int countDescending = 0;
        for (int i = 0; i < report.Length - 1; i++)
        {
            int diff = report[i + 1] - report[i];
            if (diff < 0) countDescending++;
        }

        return countDescending > report.Length / 2;
    }
}
