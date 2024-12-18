using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2024;
[Challenge(2024, 18, "RAM Run")]
internal class Day18_RAMRun
{
    private readonly Location[] _locations;

    public Day18_RAMRun()
    {
        var input = ChallengeHelper.ReadAllTextFromResourceFile();
        var matches = Regex.Matches(input, @"(\d+),(\d+)");
        _locations = matches.Select(m => new Location(m.Groups[1].ToInt32(), m.Groups[2].ToInt32())).ToArray();
    }

    [Part(1)]
    public void Part01()
    {
        int fallCount = 1024;
        Map<bool> map = new Map<bool>(71, 71, true);
        for (int i = 0; i < fallCount; i++)
        {
            map[_locations[i].X, _locations[i].Y] = false;
        }

        var shortest = FindShortestRoute(map, new Location(0, 0), new Location(map.MaxX, map.MaxY));
        Console.WriteLine($"Part 1: {shortest}");
    }

    [Part(2)]
    public void Part02()
    {
        Location location = new Location(0, 0);
        for (int x = 0; x < _locations.Length; x++)
        {
            int fallCount = x;
            Map<bool> map = new Map<bool>(71, 71, true);
            for (int i = 0; i < fallCount; i++)
            {
                map[_locations[i].X, _locations[i].Y] = false;
            }

            var shortest = FindShortestRoute(map, new Location(0, 0), new Location(map.MaxX, map.MaxY));
            if (shortest == -1)
            {
                location = new Location(_locations[x - 1].X, _locations[x - 1].Y);
                break;
            }
        }

        Console.WriteLine($"Part 2: {location}");
    }

    private static int FindShortestRoute(Map<bool> map, Location current, Location end)
    {
        HashSet<Location> visited = [];
        visited.Add(current);
        var queue = new Queue<(Location location, int steps)>();
        queue.Enqueue((current, 0));


        while (queue.TryDequeue(out var state))
        {
            if (state.location == end)
            {
                return state.steps;
            }

            for (int i = 0; i < 4; i++)
            {
                var next = GetNextLocation(state.location, i);
                if (map.IsValidLocation(next) && map[next.X, next.Y] && !visited.Contains(next))
                {
                    map[next.X, next.Y] = false;
                    queue.Enqueue((next, state.steps + 1));
                }
            }
        }

        return -1;
    }

    private static Location GetNextLocation(Location location, int directionIndex)
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
