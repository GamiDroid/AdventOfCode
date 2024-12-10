namespace AdventOfCode._2024;
[Challenge(2024, 10, "Hoof It")]
internal class Day10_HoofIt
{
    private readonly Map<int> _map;

    public Day10_HoofIt()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();
        _map = new Map<int>(lines.Select(l => l.ToCharArray().Select(c => int.Parse($"{c}")).ToArray()).ToArray());
    }

    [Part(1)]
    public void Part01()
    {
        var totalScore = 0;
        foreach (var mapLocation in _map.Enumerate())
        {
            if (mapLocation.Value == 0)
            {
                HashSet<Location> ends = [];
                GetTrailheadScorePart1(mapLocation.Location, ends);
                totalScore += ends.Count;
            }
        }

        Console.WriteLine($"Part 1: {totalScore}");
    }

    [Part(2)]
    public void Part02()
    {
        var totalScore = 0;
        foreach (var mapLocation in _map.Enumerate())
        {
            if (mapLocation.Value == 0)
            {
                totalScore += GetTrailheadScorePart2(mapLocation.Location);
            }
        }

        Console.WriteLine($"Part 2: {totalScore}");
    }

    private void GetTrailheadScorePart1(Location location, HashSet<Location> ends, int height = 0)
    {
        if (height == 9)
        {
            ends.Add(location);
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            var newLocation = GetNewLocation(location, i);
            if (_map.IsValidLocation(newLocation))
            {
                var value = _map[newLocation];
                if (value == height + 1)
                {
                    GetTrailheadScorePart1(newLocation, ends, height + 1);
                }
            }
        }
    }

    private int GetTrailheadScorePart2(Location location, int height = 0, int score = 0)
    {
        if (height == 9)
        {
            return score + 1;
        }

        for (int i = 0; i < 4; i++)
        {
            var newLocation = GetNewLocation(location, i);
            if (_map.IsValidLocation(newLocation))
            {
                var value = _map[newLocation];
                if (value == height + 1)
                {
                    score = GetTrailheadScorePart2(newLocation, height + 1, score);
                }
            }
        }

        return score;
    }

    private static Location GetNewLocation(Location location, int direction)
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

}
