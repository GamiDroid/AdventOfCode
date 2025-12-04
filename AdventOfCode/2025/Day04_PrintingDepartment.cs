namespace AdventOfCode._2025;

[Challenge(2025, 4, "Printing Department")]
internal class Day04_PrintingDepartment
{
    private readonly Map<char> _map;

    public Day04_PrintingDepartment()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();
        _map = Map.Create(lines);
    }

    [Part(1)]
    public void Part1()
    {
        int count = 0;
        foreach (var mapLoc in _map.Enumerate())
        {
            if (mapLoc.Value != '@')
            {
                continue;
            }

            var countAdjancent = CountAdjacentRolls(_map, mapLoc.Location);
            if (countAdjancent < 4)
            {
                count++;
            }
        }

        Console.WriteLine($"Accessable rolls: {count}");
    }

    private static readonly Func<Location, Location>[] s_moves = [
        (Location c) => c.Move(-1, -1),
        (Location c) => c.Move(-1, 0),
        (Location c) => c.Move(-1, 1),
        (Location c) => c.Move(0, -1),
        (Location c) => c.Move(0, 1),
        (Location c) => c.Move(1, -1),
        (Location c) => c.Move(1, 0),
        (Location c) => c.Move(1, 1),
    ];

    private static int CountAdjacentRolls(Map<char> map, Location curr)
    {
        int count = 0;
        foreach (var mov in s_moves)
        {
            var adjacent = mov(curr);
            if (!map.IsValidLocation(adjacent))
                continue;

            if (map[adjacent] == '@')
            {
                count++;
            }
        }

        return count;
    }




}
