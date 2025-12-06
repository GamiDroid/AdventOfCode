using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode._2025;

[Challenge(2025, 6, "Trash Compactor")]
internal class Day06_TrashCompactor
{
    private readonly int _sumLength;
    private readonly int[][] _nums;
    private readonly char[] _operators;
    private readonly string[] _lines;

    public Day06_TrashCompactor()
    {
        _lines = ChallengeHelper.ReadAllLinesFromResourceFile();

        _sumLength = _lines.Length - 1;
        _nums = new int[_sumLength][];

        for (int i = 0; i < _sumLength; i++)
        {
            _nums[i] = [.. Regex.Matches(_lines[i], "\\d+")
                    .Select(g => g.ToInt32())];
        }

        _operators = [.. Regex.Matches(_lines[_sumLength], "[\\*\\+]")
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
            
            for (var y = 1; y < _sumLength; y++)
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

    [Part(2)]
    public void Part2()
    {
        long total = 0;

        List<long> nums = [];
        StringBuilder sb = new();
        for (int z = _lines[0].Length - 1; z >= 0; z--)
        {
            sb.Clear();
            for (int l = 0; l < _lines.Length - 1; l++)
            {
                sb.Append(_lines[l][z]);
            }

            var num = long.Parse(sb.ToString());
            nums.Add(num);

            var o = _lines[^1][z];

            if (o == '*')
                total += nums.Aggregate((sum, num) => sum *= num);
            else if (o == '+')
                total += nums.Aggregate((sum, num) => sum += num);

            if (o != ' ')
            {
                nums.Clear();
                z--;
            }
        }

        Console.WriteLine($"Total sum: {total}");
    }
    
    public void Part2Wrong()
    {
        long total = 0;

        int mathProblems = _operators.Length;
        for (int i = 0; i < mathProblems; i++)
        {
            var o = _operators[i];
            string[] txtNums = new string[_sumLength];

            for (var y = 0; y < _sumLength; y++)
            {
                txtNums[y] = _nums[y][i].ToString();
            }

            int sum = 0;
            int maxLength = txtNums.Select(x => x.Length).Max();
            var sb = new StringBuilder();
            for (var z = 0; z < maxLength; z++)
            {
                sb.Clear();
                for (var y = 0; y < _sumLength; y++)
                {
                    if (txtNums[y].Length > z)
                        sb.Append(txtNums[y][^(z + 1)]);
                }

                var num = int.Parse(sb.ToString());

                if (sum == 0)
                {
                    sum = num;
                    continue;
                }

                if (o == '*')
                    sum *= num;
                else
                    sum += num;
            }

            total += sum;
        }

        Console.WriteLine($"Total sum: {total}");
    }
}
