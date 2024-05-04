
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;
[Challenge(2015, 12)]
internal class Day12_JSAbacusFramework_io
{
    private readonly string _input;

    public Day12_JSAbacusFramework_io()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        _input = File.ReadAllText(filePath);
    }

    [Part(1)]
    public void ExecutePart01()
    {
        int sum = SumAllNumbers(_input ?? string.Empty);
        Console.WriteLine("Sum of all numbers: " + sum);
    }

    [Part(2)]
    public void ExecutePart02()
    {
        using var document = JsonDocument.Parse(_input ?? string.Empty);

        var sum = SumAllNonRedNumbers(document.RootElement);

        Console.WriteLine($"Sum of non-reds numbers: {sum}");
    }

    public int SumAllNonRedNumbers(JsonElement element, int currentSum = 0)
    {
        int sum = currentSum;

        if (element.ValueKind == JsonValueKind.Number)
            sum += element.GetInt32();
        else if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (var arrVal in element.EnumerateArray())
            {
                sum += SumAllNonRedNumbers(arrVal);
            }
        }
        else if (element.ValueKind == JsonValueKind.Object)
        {
            int tempSum = 0;
            foreach (var props in element.EnumerateObject())
            {
                if (props.Value.ValueKind == JsonValueKind.String && props.Value.GetString() == "red")
                    return sum;

                tempSum = SumAllNonRedNumbers(props.Value, tempSum);
            }

            sum += tempSum;
        }

        return sum;
    }

    private static int SumAllNumbers(string input)
    {
        var numbers = GetAllNumbers(input);
        return numbers.Sum();
    }

    private static ICollection<int> GetAllNumbers(string input)
    {
        var matches = Regex.Matches(input, "-?\\d+");
        var numbers = matches.Select(m => int.Parse(m.Value)).ToList();
        return numbers;
    }
}
