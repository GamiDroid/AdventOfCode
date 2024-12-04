using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace AdventOfCode._2024;

[Challenge(2024, 4, "Ceres Search")]
internal class Day04_CeresSearch
{
    private readonly Map<char> _map;

    public Day04_CeresSearch()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();
        _map = Map.Create(lines);
    }

    [Part(1)]
    public void Part1()
    {
        List<string> strings = [];

        strings.AddRange(Enumerable.Range(0, _map.LengthX).Select(GetColumn));
        strings.AddRange(Enumerable.Range(0, _map.LengthY).Select(GetRow));
        strings.AddRange(Enumerable.Range(0, _map.LengthX - 3).Select(x => GetForwardSlope(x, 0)));
        strings.AddRange(Enumerable.Range(0, _map.LengthY - 3).Skip(1).Select(y => GetForwardSlope(0, y)));
        strings.AddRange(Enumerable.Range(0, _map.LengthX).Skip(3).Select(x => GetBackwardSlope(x, 0)));
        strings.AddRange(Enumerable.Range(0, _map.LengthY-3).Skip(1).Select(y => GetBackwardSlope(_map.LengthX-1, y)));

        var sum = strings.Sum(x => Regex.Matches(x, "XMAS").Count);
        var sumReversed = strings.Select(x => new string(x.Reverse().ToArray())).Sum(x => Regex.Matches(x, "XMAS").Count);

        var answer = sum + sumReversed;
        Console.WriteLine($"Part 1: {answer}");
    }

    [Part(2)]
    public void Part2()
    {
        int count = 0;
        for (int x = 0; x < _map.LengthX-2; x++)
        {
            for (int y = 0; y < _map.LengthY-2; y++)
            {
                if (IsCrossMAS(x, y) || 
                    IsCrossSAM(x, y) ||
                    IsCrossMASVertical(x, y) ||
                    IsCrossSAMVertical(x, y))
                    count++;
            }
        }

        Console.WriteLine($"Part 2: {count}");
    }

    #region Part 2 helpers
    private bool IsCrossMAS(int x, int y)
    {
        return (_map[x, y] == 'M' &&
            _map[x + 2, y] == 'S' &&
            _map[x, y + 2] == 'M' &&
            _map[x + 2, y + 2] == 'S' &&
            _map[x + 1, y + 1] == 'A');
    }

    private bool IsCrossMASVertical(int x, int y)
    {
        return (_map[x, y] == 'M' &&
            _map[x + 2, y] == 'M' &&
            _map[x, y + 2] == 'S' &&
            _map[x + 2, y + 2] == 'S' &&
            _map[x + 1, y + 1] == 'A');
    }

    private bool IsCrossSAM(int x, int y)
    {
        return (_map[x, y] == 'S' &&
            _map[x + 2, y] == 'M' &&
            _map[x, y + 2] == 'S' &&
            _map[x + 2, y + 2] == 'M' &&
            _map[x + 1, y + 1] == 'A');
    }

    private bool IsCrossSAMVertical(int x, int y)
    {
        return (_map[x, y] == 'S' &&
            _map[x + 2, y] == 'S' &&
            _map[x, y + 2] == 'M' &&
            _map[x + 2, y + 2] == 'M' &&
            _map[x + 1, y + 1] == 'A');
    }
    #endregion
    
    #region Part 1 helpers
    private string GetRow(int y)
    {
        List<char> chars = [];
        for (int x = 0; x < _map.LengthX; x++)
        {
            chars.Add(_map[x, y]);
        }
        return new string([.. chars]);
    }

    private string GetColumn(int x)
    {
        List<char> chars = [];
        for (int y = 0; y < _map.LengthY; y++)
        {
            chars.Add(_map[x, y]);
        }
        return new string([.. chars]);
    }

    private string GetForwardSlope(int x, int y)
    {
        List<char> chars = [];
        while (y <= _map.LengthY - 1 && x <= _map.LengthX - 1)
        {
            chars.Add(_map[x++, y++]);
        }
        return new string([.. chars]);
    }

    private string GetBackwardSlope(int x, int y)
    {
        List<char> chars = [];
        while (x >= 0 && y <= _map.LengthY - 1)
        {
            chars.Add(_map[x--, y++]);
        }
        return new string([.. chars]);
    } 
    #endregion
}
