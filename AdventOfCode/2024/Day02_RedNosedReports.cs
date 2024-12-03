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
                for (int i = 0; i < report.Length; i++)
                {
                    var tempList = report.ToList();
                    tempList.RemoveAt(i);

                    if (IsValidReport([.. tempList], out _))
                    {
                        counter++;
                        break;
                    }
                }
            }
        }

        Console.WriteLine($"Part 2: {counter}");
    }

    private static bool IsValidReport(int[] report, out int failureIndex)
    {
        bool isDescending;
        int countDescending = 0;
        for (int i1 = 0; i1 < report.Length - 1; i1++)
        {
            int diff = report[i1 + 1] - report[i1];
            if (diff < 0) countDescending++;
        }

        isDescending = (report.Length - countDescending) < (report.Length / 2);

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

    //private static bool IsValidReportWithTolerance(int[] report)
    //{
    //    bool isDescending;
    //    int countDescending = 0;

    //    for (int i1 = 0; i1 < report.Length-1; i1++)
    //    {
    //        int diff = report[i1 + 1] - report[i1];
    //        if (diff < 0) countDescending++;
    //    }

    //    isDescending = (report.Length - countDescending) < (report.Length / 2);

    //    int i;
    //    for (i = 0; i < report.Length-1; i++)
    //    {
    //        var diff = report[i+1] - report[i];
    //        if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3)
    //        {
    //            break;
    //        }

    //        if (isDescending && diff > 0)
    //            break;
    //        else if (!isDescending && diff < 0)
    //            break;
    //    }

    //    if (i < report.Length - 1)
    //    {
    //        var list1 = report.ToList();
    //        list1.RemoveAt(i);
    //        var arr1 = list1.ToArray();

    //        if (IsValidReport(arr1))
    //            return true;

    //        list1 = [.. report];
    //        list1.RemoveAt(i + 1);
    //        arr1 = [.. list1];

    //        if (IsValidReport(arr1))
    //            return true;
    //    }

    //    return (i == report.Length - 1);
    //}
}
