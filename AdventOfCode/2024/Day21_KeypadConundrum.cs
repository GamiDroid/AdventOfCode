using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024;
[Challenge(2024, 21, "Keypad Conundrum")]
internal class Day21_KeypadConundrum
{
    private readonly string[] _codes;

    public Day21_KeypadConundrum()
    {
        _codes = ChallengeHelper.ReadAllLinesFromResourceFile();
    }

    [Part(1)]
    public void Part01()
    {
        var npad = GetNumericKeypad();
        var dpad = GetDirectionalKeypad();
        var enter = npad.Enumerate().Where(x => x.Value == 'A').Select(x => x.Location).Single();

        var start = enter;
        foreach (var code in _codes)
        {
            var path = new List<char>();
            foreach (var key in code)
            {
                var (end, pathToKey) = GetPath(npad, start, key);
                path.AddRange(pathToKey);
                start = end;
                path.Add('A');
            }

            var startdpad = dpad.Enumerate().Where(x => x.Value == 'A').Select(x => x.Location).Single();
            start = startdpad;
            var path2 = new List<char>();
            foreach (var key in path)
            {
                var (end, pathToKey) = GetPath(dpad, start, key);
                path2.AddRange(pathToKey);
                start = end;

                var (end2, pathToKey2) = GetPath(dpad, start, key);
                path2.AddRange(pathToKey);
                start = end2;


            }

            start = startdpad;
            var path3 = new List<char>();
            foreach (var key in path2)
            {
                var (end, pathToKey) = GetPath(dpad, start, key);
                path3.AddRange(pathToKey);
                start = end;

                var (end2, pathToKey2) = GetPath(dpad, start, key);
                path3.AddRange(pathToKey);
                start = end2;
            }

            start = startdpad;
            var path4 = new List<char>();
            foreach (var key in path3)
            {
                var (end, pathToKey) = GetPath(dpad, start, key);
                path4.AddRange(pathToKey);
                start = end;

                var (end2, pathToKey2) = GetPath(dpad, start, key);
                path4.AddRange(pathToKey);
                start = end2;
            }
        }

        Console.WriteLine($"Part 1: ");
    }

    private static Map<char> GetNumericKeypad()
    {
        var keypad = new Map<char>(3, 4, '#');

        keypad[0, 0] = '7';
        keypad[1, 0] = '8';
        keypad[2, 0] = '9';

        keypad[0, 1] = '4';
        keypad[1, 1] = '5';
        keypad[2, 1] = '6';

        keypad[0, 2] = '1';
        keypad[1, 2] = '2';
        keypad[2, 2] = '3';

        keypad[0, 3] = '#';
        keypad[1, 3] = '0';
        keypad[2, 3] = 'A';

        return keypad;
    }

    private static Map<char> GetDirectionalKeypad()
    {
        var keypad = new Map<char>(3, 2, '#');
        keypad[0, 0] = '#';
        keypad[1, 0] = '^';
        keypad[2, 0] = 'A';

        keypad[0, 1] = '<';
        keypad[1, 1] = 'v';
        keypad[2, 1] = '>';

        return keypad;
    }

    private static (Location End, List<char> Path) GetPath(Map<char> map, Location start, char end)
    {
        Queue<(Location Location, List<char> Path)> queue = [];
        HashSet<Location> visited = [];

        queue.Enqueue((start, new List<char>()));

        while (queue.TryDequeue(out var state))
        {
            if (map[state.Location] == end)
            {
                return (state.Location, state.Path);
            }

            char[] directions = { '^', '>', 'v', '<' };
            for (int d = 0; d < directions.Length; d++)
            {
                var dir = directions[d];
                var next = GetNextLocation(state.Location, dir);
                if (!map.IsValidLocation(next) || map[next] == '#' || visited.Contains(next))
                {
                    continue;
                }

                List<char> nextSequence = [.. state.Path];
                nextSequence.Add(dir);

                queue.Enqueue((next, nextSequence));
            }
        }

        throw new InvalidDataException("No path found");
    }
    
    private static Location GetNextLocation(Location location, char direction)
    {
        return direction switch
        {
            '^' => new Location(location.X, location.Y - 1),
            '>' => new Location(location.X + 1, location.Y),
            'v' => new Location(location.X, location.Y + 1),
            '<' => new Location(location.X - 1, location.Y),
            _ => throw new ArgumentException("Invalid value", nameof(direction))
        };
    }

    //[Part(2)]
    //public void Part02()
    //{
    //    Console.WriteLine($"Part 2: ");
    //}
}
