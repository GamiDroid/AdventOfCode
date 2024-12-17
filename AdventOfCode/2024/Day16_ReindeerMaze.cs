using System.Diagnostics;

namespace AdventOfCode._2024;
[Challenge(2024, 16, "Reindeer Maze")]
internal class Day16_ReindeerMaze
{
    private Map<char> _map;

    public Day16_ReindeerMaze()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();
        _map = Map.Create(lines);
    }

    [Part(1)]
    public void Part01()
    {
        var sw = Stopwatch.StartNew();
        int score = FindEndWithLowestScore();
        sw.Stop();

        Console.WriteLine($"Part 1: {score} - it took {sw.Elapsed}");
    }

    [Part(2)]
    public void Part02()
    {
        var sw = Stopwatch.StartNew();
        int score = FindEndWithLowestScore();
        int visited = FindVisitedTilesToLowestScore(score);
        sw.Stop();

        Console.WriteLine($"Part 2: {visited} - it took {sw.Elapsed}");
    }

    private int FindEndWithLowestScore()
    {
        List<MazeState> queue = [];

        var startLocation = _map.Enumerate().Where(x => x.Value == 'S').Select(x => x.Location).Single();
        queue.Add(new MazeState(startLocation, 1, 0));

        while (queue.Count > 0)
        {
            var mazeState = queue.OrderBy(q => q.Score).First();
            queue.Remove(mazeState);

            if (_map[mazeState.Location] == 'E')
                return mazeState.Score;

            for (int d = 0; d < 4; d++)
            {
                var next = GetNextLocation(mazeState.Location, d);
                if (!_map.IsValidLocation(next) || _map[next] == '#')
                    continue;
                var increase = GetScoreIncrease(mazeState.Direction, d);
                if (increase > 1001)
                    continue;
                
                var newScore = mazeState.Score + increase;
                if (queue.Any(q => q.Location == next && q.Direction == d && q.Score <= newScore))
                    continue;
                queue.Add(new MazeState(next, d, newScore));
            }
        }

        return -1;
    }

    private int FindVisitedTilesToLowestScore(int maxScore)
    {
        List<MazeState> queue = [];

        HashSet<Location> visited = [];
        var startLocation = _map.Enumerate().Where(x => x.Value == 'S').Select(x => x.Location).Single();
        var startState = new MazeState(startLocation, 1, 0);
        startState.AddVisited([], startLocation);
        queue.Add(startState);

        while (queue.Count > 0)
        {
            var mazeState = queue.OrderBy(q => q.Score).First();
            queue.Remove(mazeState);

            if (mazeState.Score > maxScore)
            {
                return visited.Count;
            }

            if (_map[mazeState.Location] == 'E')
            {
                foreach (var v in mazeState.Visited)
                    visited.Add(v);
                continue;
            }

            for (int d = 0; d < 4; d++)
            {
                var next = GetNextLocation(mazeState.Location, d);
                if (!_map.IsValidLocation(next) || _map[next] == '#')
                    continue;
                var increase = GetScoreIncrease(mazeState.Direction, d);
                if (increase > 1001)
                    continue;

                var newScore = mazeState.Score + increase;
                if (newScore > maxScore)
                {
                    continue;
                }

                if (queue.Any(q => q.Location == next && q.Direction == d && q.Score < newScore))
                    continue;

                var intersection = queue.FirstOrDefault(q => q.Location == next && q.Direction == d && q.Score == newScore);
                if (intersection is not null)
                {
                    foreach (var v in mazeState.Visited)
                        intersection.Visited.Add(v);
                    continue;
                }

                var newState = new MazeState(next, d, newScore);
                newState.AddVisited(mazeState.Visited, next);
                queue.Add(newState);
            }
        }

        return visited.Count;
    }

    private static Location GetNextLocation(Location location, int direction)
    {
        return direction switch
        {
            0 => new Location(location.X, location.Y - 1),
            1 => new Location(location.X + 1, location.Y),
            2 => new Location(location.X, location.Y + 1),
            3 => new Location(location.X - 1, location.Y),
            _ => throw new ArgumentException("Invalid value", nameof(direction))
        };
    }

    private static int GetScoreIncrease(int oldDir, int newDir)
    {
        if (oldDir == newDir) return 1;
        else if ((newDir == 3 && oldDir == 0) || newDir == 0 && oldDir == 3)
            return 1001;
        else if (Math.Abs(oldDir - newDir) == 1)
            return 1001;
        return 2001;
    }

    public record MazeState(Location Location, int Direction, int Score)
    {
        public Location Location { get; private set; } = Location;
        public int Direction { get; private set; } = Direction;
        public int Score { get; private set; } = Score;
        public HashSet<Location> Visited { get; private set; } = [];

        public void AddVisited(HashSet<Location> visited, Location location)
        {
            Visited = [.. visited];
            Visited.Add(location);
        }
    }
}
