namespace AdventOfCode._2024;
[Challenge(2024, 12, "Garden Groups")]
internal class Day12_GardenGroups
{
    private readonly Map<char> _map;

    public Day12_GardenGroups()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();
        _map = Map.Create(lines);
    }

    [Part(1)]
    public void Part01()
    {
        int prize = 0;
        HashSet<Location> totalVisited = [];
        foreach (var mapLocation in _map.Enumerate())
        {
            if (totalVisited.Contains(mapLocation.Location))
                continue;

            var gardenGroup = FindGardenGroup(mapLocation.Location, mapLocation.Value);
            var fenceCount = CountFences(gardenGroup);

            foreach (var garden in gardenGroup)
                totalVisited.Add(garden);

            prize += gardenGroup.Count * fenceCount;
        }

        var answer = prize;
        Console.WriteLine($"Part 1: {answer}");
    }

    [Part(2)]
    public void Part02()
    {
        int prize = 0;
        HashSet<Location> totalVisited = [];
        foreach (var mapLocation in _map.Enumerate())
        {
            if (totalVisited.Contains(mapLocation.Location))
                continue;

            var gardenGroup = FindGardenGroup(mapLocation.Location, mapLocation.Value);
            foreach (var garden in gardenGroup)
            {
                totalVisited.Add(garden);
            }

            var fences = GetFences(gardenGroup);

            int totalSides = 0;
            var allVerticalX = fences.Select(x => new { x.Location.X, x.Direction }).Where(x => x.Direction is 1 or 3).DistinctBy(x => new { x.X, x.Direction }).ToList();
            foreach (var v in allVerticalX)
            {
                int sides = 1;
                var verticalFences = fences.Where(f => f.Location.X == v.X && f.Direction == v.Direction).OrderBy(l => l.Location.Y).ToList();
                for (int i = 0; i < verticalFences.Count - 1; i++)
                {
                    var fence1 = verticalFences[i];
                    var fence2 = verticalFences[i + 1];
                    var gap = fence2.Location.Y - fence1.Location.Y;
                    if (gap > 1)
                        sides++;
                }
                totalSides += sides;
            }

            var allHorizontalY = fences.Select(y => new { y.Location.Y, y.Direction }).Where(x => x.Direction is 0 or 2).DistinctBy(y => new { y.Y, y.Direction }).ToList();
            foreach (var h in allHorizontalY)
            {
                int sides = 1;
                var horizontalFences = fences.Where(f => f.Location.Y == h.Y && f.Direction == h.Direction).OrderBy(l => l.Location.X).ToList();
                for (int i = 0; i < horizontalFences.Count - 1; i++)
                {
                    var fence1 = horizontalFences[i];
                    var fence2 = horizontalFences[i + 1];
                    var gap = fence2.Location.X - fence1.Location.X;
                    if (gap > 1)
                        sides++;
                }


                totalSides += sides;
            }

            prize += gardenGroup.Count * totalSides;
        }

        var answer = prize;
        Console.WriteLine($"Part 2: {answer}");
    }

    private int CountFences(HashSet<Location> locations)
    {
        int fenceCount = 0;
        foreach (var location in locations)
        {
            for (int d = 0; d < 4; d++)
            {
                var nextLocation = GetNewLocation(location, d);
                if (!_map.IsValidLocation(nextLocation) ||
                    !locations.Contains(nextLocation))
                {
                    fenceCount++;
                }
            }
        }

        return fenceCount;
    }

    private HashSet<(Location Location, int Direction)> GetFences(HashSet<Location> gardenGroup)
    {
        HashSet<(Location, int)> fences = [];
        foreach (var location in gardenGroup)
        {
            for (int d = 0; d < 4; d++)
            {
                var nextLocation = GetNewLocation(location, d);
                if (!_map.IsValidLocation(nextLocation) ||
                    !gardenGroup.Contains(nextLocation))
                {
                    fences.Add((nextLocation, d));
                }
            }
        }

        return fences;
    }

    private HashSet<Location> FindGardenGroup(Location startLocation, char value)
    {
        var gardenGroup = new HashSet<Location>();
        FindGardenGroupUtil(startLocation, value, gardenGroup);
        return gardenGroup;
    }

    private void FindGardenGroupUtil(Location location, char value, HashSet<Location> visited)
    {
        visited.Add(location);

        for (int d = 0; d < 4; d++)
        {
            var nextLocation = GetNewLocation(location, d);

            if (!_map.IsValidLocation(nextLocation) ||
                visited.Contains(nextLocation))
            {
                continue;
            }

            var nextValue = _map[nextLocation];
            if (nextValue != value)
            {
                continue;
            }

            FindGardenGroupUtil(nextLocation, value, visited);
        }
    }

    private static Location GetNewLocation(Location location, int directionIndex)
    {
        return directionIndex switch
        {
            0 => new Location(location.X, location.Y - 1),
            1 => new Location(location.X + 1, location.Y),
            2 => new Location(location.X, location.Y + 1),
            3 => new Location(location.X - 1, location.Y),
            _ => throw new ArgumentException("Invalid value", nameof(directionIndex))
        };
    }
}
