namespace AdventOfCode._2023;

[Challenge(2023, 17, "Clumsy Crucible")]
internal class Day17_ClumsyCrucible
{
    private readonly Map<int> _map;

    public Day17_ClumsyCrucible()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();
        _map = new Map<int>(lines.Select(l => l.ToCharArray().Select(c => int.Parse($"{c}")).ToArray()).ToArray());
    }

    [Part(1)]
    public void Part01()
    {
        var answer = FindPathWithLeastHeatLoss();
        Console.WriteLine($"Part 1: {answer}");
    }

    private int FindPathWithLeastHeatLoss()
    {
        // breath-first-search with prioritized queue
        // cannot move 3 times in the same direction.
        // score of tile is added

        PriorityQueue<PathState, int> queue = new();

        var startLocation = new Location(0, 0);
        var endLocation = new Location(_map.MaxX, _map.MaxY);

        var startHeatLoss = _map[startLocation];

        var beginState = new PathState
        {
            Location = startLocation,
            HeatLoss = startHeatLoss,
            SameDirectionCounter = 0,
        };

        queue.Enqueue(beginState, beginState.HeatLoss);

        char[] directions = ['N', 'E', 'Z', 'W'];

        while (queue.Count > 0)
        {
            var state = queue.Dequeue();

            if (state.Location == endLocation)
                return state.HeatLoss;

            foreach (var direction in directions)
            {
                if (CanMove(state, direction))
                {
                    var newState = Move(state, direction);
                    queue.Enqueue(newState, newState.HeatLoss);
                }
            }
        }

        return -1;
    }

    bool CanMove(PathState state, char direction)
    {
        if (IsOppisite(state.Direction, direction))
            return false;
        if (state.Direction == direction && state.SameDirectionCounter == 3)
            return false;
        return _map.IsValidLocation(NextLocation(state.Location, direction));
    }

    bool IsOppisite(char dir1, char dir2)
    {
        if (dir1 == dir2)
            return false;
        return (dir1 == 'N' && dir2 == 'Z') ||
            (dir1 == 'Z' && dir2 == 'N') ||
            (dir1 == 'E' && dir2 == 'W') ||
            (dir1 == 'W' && dir2 == 'E');
    }

    PathState Move(PathState state, char direction)
    {
        var nextLocation = NextLocation(state.Location, direction);
        var heatLoss = _map[nextLocation];
        return new()
        {
            Direction = direction,
            HeatLoss = state.HeatLoss + heatLoss,
            Location = nextLocation,
            SameDirectionCounter = state.Direction == direction ? state.SameDirectionCounter + 1 : 0,
        };
    }

    private Location NextLocation(Location location, char direction)
    {
        return direction switch
        {
            'N' => new(location.X, location.Y - 1),
            'Z' => new(location.X, location.Y + 1),
            'E' => new(location.X + 1, location.Y),
            'W' => new(location.X - 1, location.Y),
            _ => location
        };
    }

    private struct PathState
    {
        public Location Location { get; set; }
        public char Direction { get; set; }
        public int SameDirectionCounter { get; set; }
        public int HeatLoss { get; set; }
    }
}
