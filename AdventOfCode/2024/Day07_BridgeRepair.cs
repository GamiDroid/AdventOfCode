using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace AdventOfCode._2024;

[Challenge(2024, 7, "Bridge Repair")]
internal class Day07_BridgeRepair
{
    private readonly List<(ulong Answer, List<ulong> Values)> _equations;

    public Day07_BridgeRepair()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();
        _equations = [];

        foreach (var line in lines)
        {
            var match = Regex.Match(line, @"(\d+):\s(.+)");
            var answer = match.Groups[1].ToUInt64();
            List<ulong> values = match.Groups[2].Value.Split(' ').Select(s => ulong.Parse(s)).ToList();
            _equations.Add((answer, values));
        }
    }

    [Part(1)]
    public void Part01()
    {
        ulong correctSum = 0;
        foreach (var equation in _equations)
        {
            var operatorCount = equation.Values.Count - 1;
            List<List<int>> operatorCombinationIndexes = [];
            FindOperatorCombinations(operatorCount, 2, ref operatorCombinationIndexes);
            
            if (ContainsCorrectAnswer(equation.Answer, equation.Values, operatorCombinationIndexes))
            {
                correctSum += equation.Answer;
            }
        }

        Console.WriteLine($"Part 1: {correctSum}");
    }

    [Part(2)]
    public void Part02()
    {
        ulong correctSum = 0;
        foreach (var equation in _equations)
        {
            var operatorCount = equation.Values.Count - 1;
            List<List<int>> operatorCombinationIndexes = [];
            FindOperatorCombinations(operatorCount, 3, ref operatorCombinationIndexes);

            if (ContainsCorrectAnswer(equation.Answer, equation.Values, operatorCombinationIndexes))
            {
                correctSum += equation.Answer;
            }
        }

        Console.WriteLine($"Part 2: {correctSum}");
    }

    private static bool ContainsCorrectAnswer(ulong expectedAnswer, List<ulong> values, List<List<int>> combinations)
    {
        foreach (var operatorIndex in combinations)
        {
            ulong actualAnswer = 0;
            for (int v = 1; v < values.Count; v++)
            {
                var oper = operatorIndex[v - 1] switch
                {
                    0 => '*',
                    1 => '+',
                    2 => '|',
                    _ => throw new InvalidDataException()
                };


                if (v == 1)
                {
                    actualAnswer = values[v - 1];
                }

                actualAnswer = oper switch
                {
                    '*' => actualAnswer * values[v],
                    '+' => actualAnswer + values[v],
                    '|' => ulong.Parse($"{actualAnswer}{values[v]}"),
                    _ => throw new InvalidDataException()
                };
            }

            if (actualAnswer == expectedAnswer)
                return true;
        }

        return false;
    }

    private static void FindOperatorCombinations(int operatorCount, int optionsCount, ref List<List<int>> operatorCombinationIndexes, List<int>? currentCombination  = null)
    {
        currentCombination ??= [];

        if (operatorCount == 0)
        {
            operatorCombinationIndexes.Add(currentCombination);
            return;
        }

        for (int c = 0; c < optionsCount; c++)
        {
            List<int> newCurrentCombinations = [..currentCombination];
            newCurrentCombinations.Add(c);
            FindOperatorCombinations(operatorCount - 1, optionsCount, ref operatorCombinationIndexes, newCurrentCombinations);
        }
    }
}
