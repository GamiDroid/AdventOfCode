using System.Text.RegularExpressions;

namespace AdventOfCode._2024;
[Challenge(2024, 3, "Mull It Over")]
internal class Day03_MullItOver
{
    private readonly string _input;
    public Day03_MullItOver()
    {
        _input = ChallengeHelper.ReadAllTextFromResourceFile();
    }

    [Part(1)]
    public void Part01()
    {
        long sum = 0;
        foreach (Match match in Regex.Matches(_input, @"mul\((\d{1,3}),(\d{1,3})\)"))
        {
            var n1 = match.Groups[1].ToInt32();
            var n2 = match.Groups[2].ToInt32();

            sum += n1 * n2;
        }

        Console.WriteLine($"Part 1: {sum}");
    }

    [Part(2)]
    public void Part02()
    {
        long sum = 0;
        bool doMul = true;

        foreach (Match match in Regex.Matches(_input, @"(?<t>mul)\((?<n1>\d{1,3}),(?<n2>\d{1,3})\)|(?<t>do(n't)?)\(\)"))
        {
            if (doMul && match.Groups["t"].Value == "mul")
            {
                var n1 = match.Groups["n1"].ToInt32();
                var n2 = match.Groups["n2"].ToInt32();

                sum += n1 * n2;
            }

            if (match.Groups["t"].Value == (doMul ? "don't" : "do"))
                doMul = !doMul;
        }

        Console.WriteLine($"Part 2: {sum}");
    }
}
