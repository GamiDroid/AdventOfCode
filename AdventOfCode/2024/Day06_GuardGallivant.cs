using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024;
[Challenge(2024, 6, "Guard Gallivant")]
internal class Day06_GuardGallivant
{
    private readonly Map<char> _map;
    private readonly Location _startLocation;

    public Day06_GuardGallivant()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();
        _map = Map.Create(lines);
        _startLocation = FindStart();
    }


    [Part(1)]
    public void Part01()
    {
        var location = _startLocation;
        var direction = '^';

        HashSet<Location> visited = [];
        visited.Add(location);

        while (true)
        {
            var newLocation = Move(location, direction);
            
            if (!_map.IsValidLocation(newLocation))
            {
                break;
            }

            if (_map[newLocation] == '#')
            {
                direction = TurnRight90Degree(direction);
                continue;
            }

            visited.Add(newLocation);
            location = newLocation;
        }

        Console.WriteLine($"Part 1: {visited.Count}");
    }

    [Part(2)]
    public void Part02()
    {
        var counter = 0;
        for (var x = 0; x < _map.LengthX; x++)
        {
            for (var y = 0; y < _map.LengthY; y++)
            {
                if (_map[x, y] != '^' && _map[x, y] != '#')
                {
                    _map[x, y] = '#';
                    if (IsClosing())
                        counter++;
                    _map[x, y] = '.';
                }
            }
        }

        Console.WriteLine($"Part 2: {counter}");
    }

    private bool IsClosing()
    {
        var location = _startLocation;
        var direction = '^';

        HashSet<(Location, char)> visited = [];
        visited.Add((location, direction));

        while (true)
        {
            var newLocation = Move(location, direction);

            if (!_map.IsValidLocation(newLocation))
            {
                break;
            }

            if (_map[newLocation] == '#')
            {
                if (!visited.Add((newLocation, direction)))
                    return true;

                direction = TurnRight90Degree(direction);
                continue;
            }

            location = newLocation;
        }

        return false;
    }

    private Location FindStart()
    {
        for (var x = 0; x < _map.LengthX; x++)
        {
            for (var y = 0; y < _map.LengthY; y++)
            {
                if (_map[x, y] == '^')
                    return new Location(x, y);
            }
        }

        throw new InvalidDataException();
    }

    private static Location Move(Location location, char direction)
    {
        return direction switch
        {
            '>' => new(location.X + 1, location.Y),
            '<' => new(location.X - 1, location.Y),
            '^' => new(location.X, location.Y - 1),
            'V' => new(location.X, location.Y + 1),
            _ => throw new ArgumentException($"Direction '{direction}' is not valid", nameof(direction))
        };
    }

    private static char TurnRight90Degree(char currentDirection)
    {
        return currentDirection switch
        {
            '^' => '>',
            '>' => 'V',
            'V' => '<',
            '<' => '^',
            _ => throw new ArgumentException("Invalid direction")
        };
    }
}
