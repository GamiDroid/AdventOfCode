using System.Text.RegularExpressions;

namespace AdventOfCode._2016;
[Challenge(2016, 3, "Squares With Three Sides")]
internal class Day03_SquaresWithThreeSides
{
    private int[][] _numbers = Array.Empty<int[]>();

    [Setup]
    public void Setup()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var lines = File.ReadAllLines(filePath);

        var numbers = new int[lines.Length][];
        for(int i = 0; i < lines.Length; i++)
        {
            var matches = Regex.Matches(lines[i], "\\d+");
            if (!matches.Any(m => m.Success))
                continue;

            numbers[i] = matches.Select(c => int.Parse(c.ValueSpan)).ToArray();
        }

        _numbers = numbers;
    }

    [Part(1)]
    public void Part01()
    {
        int possible = 0;
        foreach (var numbers in _numbers)
        {
            var orderedNumbers = numbers.Order();
            if (orderedNumbers.Take(2).Sum() > orderedNumbers.TakeLast(1).Sum())
                possible++;
        }

        Console.WriteLine("Of the {0} combinations only {1} are possible", _numbers.Length, possible);
    }

    [Part(2)]
    public void Part02()
    {
        var firstColGroups = _numbers.Select(x => x[0]).GroupBy(x => x % 10);
        var secondColGroups = _numbers.Select(x => x[1]).GroupBy(x => x % 10);
        var thirdColGroups = _numbers.Select(x => x[2]).GroupBy(x => x % 10);

        

        int possible = 0;
        foreach (var numbers in _numbers)
        {
            var orderedNumbers = numbers.Order();
            if (orderedNumbers.Take(2).Sum() <= orderedNumbers.TakeLast(1).Sum())
                continue;

            if (numbers.Select(n => n % 10).Distinct().Count() > 1)
                continue;

            possible++;
        }

        Console.WriteLine("Of the {0} combinations only {1} are possible", _numbers.Length, possible);
    }
}
