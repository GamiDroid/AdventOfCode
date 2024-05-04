namespace AdventOfCode._2021;

[Challenge(2021, 6)]
public class Day06_Lanternfish
{
    private readonly string _testData;

    public Day06_Lanternfish()
    {
        _testData = GetTestData();
    }

    [Part(1)]
    public void ExecutePart01()
    {
        var initialState = _testData;

        var state = new FishState(initialState);
        state.AddDays(80);

        Console.WriteLine("Amount of fish: {0}", state.Count());
    }

    [Part(2)]
    public void ExecutePart02()
    {
        var initialState = _testData;

        var state = new FishState(initialState);
        state.AddDays(256);

        Console.WriteLine("Amount of fish: {0}", state.Count());
    }

    private static string GetTestData()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        return File.ReadAllText(filePath);
    }

    public class FishState
    {
        private readonly long[] _timers = new long[9];

        public FishState(string? beginState)
        {
            _timers = Parse(beginState);
        }

        public void AddDays(int days)
        {
            Repeat(days, AddDay);
        }

        public void AddDay()
        {
            var babies = _timers[0];
            for (int i = 1; i <= 8; i++)
            {
                _timers[i - 1] = _timers[i];
            }

            _timers[8] = babies;
            _timers[6] = _timers[6] + babies;
        }

        public long Count() => _timers.Sum(t => t);

        private static void Repeat(int count, Action action)
        {
            for (int i = 0; i < count; i++)
                action();
        }

        private static long[] Parse(string? input)
        {
            var result = new long[9];

            if (string.IsNullOrWhiteSpace(input))
                return result;

            var strValues = input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var count = strValues.Select(s => int.Parse(s))
                .GroupBy(i => i, (i, c) => new { Timer = i, Count = c.LongCount() });

            foreach (var t in count)
                result[t.Timer] = t.Count;

            return result;
        }
    }
}
