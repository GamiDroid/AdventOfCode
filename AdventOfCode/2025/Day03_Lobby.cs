namespace AdventOfCode._2025;

[Challenge(2025, 3, "Lobby")]
internal class Day03_Lobby
{
    private readonly string[] _banks;

    public Day03_Lobby()
    {
        _banks = ChallengeHelper.ReadAllLinesFromResourceFile();
    }

    [Part(1)]
    public void Part1()
    {
        int sum = 0;

        foreach (var bank in _banks)
        {
            int first = 0;
            int idxFirst = 0;

            for (int i = 0; i < bank.Length - 1; i++)
            {
                var val = int.Parse(bank[i].ToString());
                if (val > first)
                {
                    first = val;
                    idxFirst = i;
                }
            }

            int second = 0;
            for (int i = idxFirst + 1; i < bank.Length; i++)
            {
                var val = int.Parse(bank[i].ToString());
                if (val > second)
                {
                    second = val;
                }
            }

            var joltage = int.Parse($"{first}{second}");
            sum += joltage;
        }

        Console.WriteLine($"Total joltage {sum}");
    }

    [Part(2)]
    public void Part2()
    {
        long sum = 0;

        foreach (var bank in _banks)
        {
            int[] batteries = new int[12];
            int[] batteryIdx = new int[batteries.Length];

            for (int b = 0; b < batteries.Length; b++)
            {
                var bankStart = (b > 0) ? batteryIdx[b - 1] + 1 : 0;
                var bankEnd = bank.Length - (batteries.Length - 1) + b;

                for (int i = bankStart; i < bankEnd; i++)
                {
                    var val = int.Parse(bank[i].ToString());
                    if (val > batteries[b])
                    {
                        batteries[b] = val;
                        batteryIdx[b] = i;
                    }
                }
            }

            var joltage = long.Parse(string.Join("", batteries));
            sum += joltage;
        }

        Console.WriteLine($"Total joltage {sum}");
    }
}
