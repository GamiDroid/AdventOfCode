using System.Text.RegularExpressions;

namespace AdventOfCode._2025;

[Challenge(2025, 6, "Trash Compactor")]
internal class Day06_TrashCompactor
{
    private const int c_sumLength = 4;
    private int[][] _nums = new int[c_sumLength][];
    private char[] _operators;

    public Day06_TrashCompactor()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();

        for (int i = 0; i < c_sumLength; i++)
        {
            _nums[i] = [.. Regex.Matches(lines[i], "\\d+")
                    .Select(g => g.ToInt32())];
        }

        _operators = [.. Regex.Matches(lines[c_sumLength], "[\\*\\+]")
            .Select(m => m.Value[0])];
    }

    [Part(1)]
    public void Part1()
    {
        long total = 0;

        int mathProblems = _operators.Length;
        for (int i = 0; i < mathProblems; i++)
        {
            var o = _operators[i];
            long sum = _nums[0][i];
            
            for (var y = 1; y < c_sumLength; y++)
            {
                if (o == '*')
                    sum *= _nums[y][i];
                else
                    sum += _nums[y][i];
            }

            total += sum;
        }

        Console.WriteLine($"Total sum: {total}");
    }
}
