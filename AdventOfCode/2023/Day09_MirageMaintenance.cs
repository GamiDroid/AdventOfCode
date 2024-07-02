namespace AdventOfCode._2023;
[Challenge(2023, 9, "Mirage Maintenance")]
internal class Day09_MirageMaintenance
{
    private readonly int[][] _dataset;

    public Day09_MirageMaintenance()
    {
        var inputPath = ChallengeHelper.GetResourceFilePath();
        var lines = File.ReadAllLines(inputPath);

        _dataset = lines
            .Select(l => l.Split(' ').Select(s => int.Parse(s)).ToArray())
            .ToArray();
    }

    [Part(1)]
    public void Part01()
    {
        var sum = _dataset.Sum(arr => GetNextPredictedValue(arr));
        Console.WriteLine($"The sum of the extrapolates values is {sum}");
    }

    [Part(2)]
    public void Part02()
    {
        var sum = _dataset.Sum(arr => GetPreviousPredictedValue(arr));
        Console.WriteLine($"The sum of the extrapolates values is {sum}");
    }

    private static int GetPreviousPredictedValue(int[] values)
    {
        if (values.All(v => v == 0))
            return 0;

        var differences = GetDifferenceAtEachStep(values);
        return values[0] - GetPreviousPredictedValue([.. differences]);
    }

    private static int GetNextPredictedValue(int[] values)
    {
        if (values.All(v => v == 0))
            return 0;

        var differences = GetDifferenceAtEachStep(values);
        return values[^1] + GetNextPredictedValue([.. differences]);
    }

    private static int[] GetDifferenceAtEachStep(int[] values) 
    {
        List<int> differences = [];
        for (int i = 0; i < values.Length - 1; i++)
        {
            var diff = values[i + 1] - values[i];
            differences.Add(diff);
        }

        return [..differences];
    }
}
