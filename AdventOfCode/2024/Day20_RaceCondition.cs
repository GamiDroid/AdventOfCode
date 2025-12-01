namespace AdventOfCode._2024;
[Challenge(2024, 20, "Race Condition")]
internal class Day20_RaceCondition
{
    private readonly Map<char> _map;

    public Day20_RaceCondition()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();
        _map = Map.Create(lines);
    }

    [Part(1)]
    public void Part01()
    {
        var start = FindFirstLocation('S');
        var end = FindFirstLocation('E');

        int steps = SolveMaze(start, end);

        Console.WriteLine($"Part 1: {steps}");
    }

    private int SolveMaze(Location start, Location end)
    {
        var queue = new Queue<MazeState>();
        queue.Enqueue(new MazeState(start, 0, cheats: 0));
        HashSet<(Location, int)> visited = [];
        visited.Add((start, 0));

        while (queue.TryDequeue(out var current))
        {
            if (current.Location == end)
            {
                return current.Steps;
            }

            visited.Add((current.Location, current.Cheats - 1));

            foreach (var dir in Enumerable.Range(0, 4))
            {
                var next = GetNextLocation(current.Location, dir);
                if (!_map.IsValidLocation(next))
                {
                    continue;
                }

                if (visited.Contains((next, current.Cheats)))
                {
                    continue;
                }

                if (_map[next] == '#' && current.Cheats == 0)
                    continue;

                else if (_map[next] == '#' && current.Cheats > 0)
                {

                    queue.Enqueue(new MazeState(next, current.Steps + 1, current.Cheats - 1, true));
                }
                else
                {
                    var nextCheats = current.CheatActivated ? current.Cheats - 1 : current.Cheats;
                    queue.Enqueue(new MazeState(next, current.Steps + 1, nextCheats, current.CheatActivated));
                }
            }
        }

        return -1;
    }

    private static Location GetNextLocation(Location location, int direction)
    {
        return direction switch
        {
            0 => location.Move(0, -1),
            1 => location.Move(1, 0),
            2 => location.Move(0, 1),
            3 => location.Move(-1, 0),
            _ => throw new ArgumentException("Invalid value", nameof(direction))
        };
    }

    private Location FindFirstLocation(char c)
    {
        foreach (var mapLocation in _map.Enumerate())
        {
            if (mapLocation.Value == c)
            {
                return mapLocation.Location;
            }
        }

        throw new InvalidDataException();
    }

    public class MazeState
    {
        public MazeState(Location location, int steps, int cheats, bool cheatActivated = false)
        {
            Location = location;
            Steps = steps;
            Cheats = cheats;
            CheatActivated = cheatActivated;
        }

        public Location Location { get; set; }
        public int Steps { get; set; }
        public int Cheats { get; set; }
        public bool CheatActivated { get; set; }
    }
}
