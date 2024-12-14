using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2024;
[Challenge(2024, 14, "Restroom Redoubt")]
internal class Day14_RestroomRedoubt
{
    private readonly (Location Location, Velocity Velocity)[] _robots;
    private readonly int _tilesWide = 101;
    private readonly int _tilesTall = 103;

    public Day14_RestroomRedoubt()
    {
        var input = ChallengeHelper.ReadAllTextFromResourceFile();
        _robots = Regex.Matches(input, "p=(?<px>-?\\d+),(?<py>-?\\d+) v=(?<vx>-?\\d+),(?<vy>-?\\d+)")
            .Select(m => m.Groups)
            .Select(g => (
                new Location(g["px"].ToInt32(), g["py"].ToInt32()), 
                new Velocity(g["vx"].ToInt32(), g["vy"].ToInt32())))
            .ToArray();
    }

    [Part(1)]
    public void Part01()
    {
        var robots = _robots.Select(x => (x.Location, x.Velocity)).ToArray();

        foreach (var sec in Enumerable.Range(0, 100))
        {
            for (int i = 0; i < robots.Length; i++)
            {
                var robot = robots[i];

                var lookahead = new Location(
                    robot.Location.X + robot.Velocity.X,
                    robot.Location.Y + robot.Velocity.Y);

                if (IsValidLocation(lookahead))
                    robots[i].Location = lookahead;
                else
                {
                    var correction = GetCorrection(lookahead);
                    robots[i].Location = correction;
                }
            }
        }

        int verticalSplit = _tilesWide / 2;
        int horizontalSplit = _tilesTall / 2;

        int answer = 
            robots.Select(r => r.Location).Count(r => r.X < verticalSplit && r.Y < horizontalSplit)
            * robots.Select(r => r.Location).Count(r => r.X < verticalSplit && r.Y > horizontalSplit)
            * robots.Select(r => r.Location).Count(r => r.X > verticalSplit && r.Y < horizontalSplit)
            * robots.Select(r => r.Location).Count(r => r.X > verticalSplit && r.Y > horizontalSplit);

        Console.WriteLine($"Part 1: {answer}");
    }

    [Part(2)]
    public void Part02()
    {
        var answer = FindEasterEgg();
        Console.WriteLine($"Part 2: {answer}");
    }

    private int FindEasterEgg()
    {
        var robots = _robots.Select(x => (x.Location, x.Velocity)).ToArray();
        var map = new Map<char>(_tilesWide, _tilesTall, '.');

        HashSet<(Location, Velocity)[]> locations = [];

        int sec = 0;
        while (true)
        {
            for (int i = 0; i < _robots.Length; i++)
            {
                var robot = robots[i];
                var currentLocation = robot.Location;

                var lookahead = new Location(
                    currentLocation.X + robot.Velocity.X,
                    currentLocation.Y + robot.Velocity.Y);

                if (IsValidLocation(lookahead))
                {
                    robots[i].Location = lookahead;
                    MoveOnMap(robots, map, currentLocation, lookahead);
                }
                else
                {
                    var correction = GetCorrection(lookahead);
                    robots[i].Location = correction;
                    MoveOnMap(robots, map, currentLocation, correction);
                }
            }

            sec++;

            foreach (var robot in robots)
            {
                if (TryFindXmasTree(robots.Select(x => x.Location).ToArray(), robot.Location, 5))
                {
                    Console.SetCursorPosition(0, 0);
                    PrintMap(map);
                    return sec;
                }
            }
        }
    }

    private static bool TryFindXmasTree(Location[] robots, Location robot, int deep)
    {
        var leftSlide = robot.X - 1;
        var rightSlide = robot.X + 1;
        var down = robot.Y + 1;

        if (robots.Contains(new Location(leftSlide, down)) && robots.Contains(new Location(rightSlide, down)))
        {
            if (FindLeftSlide(robots, new Location(leftSlide, down), deep - 1) &&
                    FindRightSlide(robots, new Location(rightSlide, down), deep - 1))
                return true;
        }

        return false;
    }

    private static bool FindLeftSlide(Location[] robots, Location robot, int deep)
    {
        if (deep == 0)
            return true;

        var slide = new Location(robot.X - 1, robot.Y + 1);
        if (robots.Contains(slide))
        {
            if (FindLeftSlide(robots, slide, deep -1))
                return true;
        }

        return false;
    }

    private static bool FindRightSlide(Location[] robots, Location robot, int deep)
    {
        if (deep == 0)
            return true;

        var slide = new Location(robot.X + 1, robot.Y + 1);
        if (robots.Contains(slide))
        {
            if (FindRightSlide(robots, slide, deep - 1))
                return true;
        }

        return false;
    }

    private static void MoveOnMap((Location, Velocity)[] robots, Map<char> map, Location location, Location newLocation)
    {
        map[newLocation] = '#';
        map[location] = robots.Any(r => r.Item1 == location) ? '#' : '.';
    }

    private static void PrintMap(Map<char> map)
    {
        for (int y = 0; y < map.LengthY; y++)
        {
            for (int x = 0; x < map.LengthX; x++)
            {
                Console.Write(map[x, y]);
            }
            Console.WriteLine();
        }
    }

    private Location GetCorrection(Location location)
    {
        int correctionX = location.X;
        if (location.X < 0 || location.X >= _tilesWide)
        {
            if (location.X >= _tilesWide)
                correctionX = location.X - _tilesWide;
            else
                correctionX = _tilesWide + location.X;
        }

        int correctionY = location.Y;
        if (location.Y < 0 || location.Y >= _tilesTall)
        {
            if (location.Y >= _tilesTall)
                correctionY = location.Y - _tilesTall;
            else
                correctionY = _tilesTall + location.Y;
        }

        return new Location(correctionX, correctionY);
    }

    private bool IsValidLocation(Location location)
    {
        return location.X > 0 && location.X < _tilesWide &&
            location.Y > 0 && location.Y < _tilesTall;
    }

    public record struct Velocity(int X, int Y);
}
