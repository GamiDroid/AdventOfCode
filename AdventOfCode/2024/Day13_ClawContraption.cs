using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2024;
[Challenge(2024, 13, "Claw Contraption")]
internal class Day13_ClawContraption
{

    private readonly Machine[] _machines;

    public Day13_ClawContraption()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();
        int lineCount = 1;

        List<Machine> machines = [];
        List<int> numbers = [];
        for (int i = 0; i <= lines.Length; i++)
        {
            if (i == lines.Length || string.IsNullOrEmpty(lines[i]))
            {
                machines.Add(new Machine(
                    aX: numbers[0],
                    aY: numbers[1],
                    bX: numbers[2],
                    bY: numbers[3],
                    prizeX: numbers[4],
                    prizeY: numbers[5]));

                numbers = [];
                lineCount = 1;
                continue;
            }

            string? line = lines[i];
            if (lineCount is 1 or 2)
            {
                var match = Regex.Match(line, "Button (\\w): X\\+(?<x>\\d+), Y\\+(?<y>\\d+)");
                numbers.Add(match.Groups["x"].ToInt32());
                numbers.Add(match.Groups["y"].ToInt32());
            }
            else
            {
                var match = Regex.Match(line, "Prize: X=(?<x>\\d+), Y=(?<y>\\d+)");
                numbers.Add(match.Groups["x"].ToInt32());
                numbers.Add(match.Groups["y"].ToInt32());
            }

            lineCount++;
        }

        _machines = machines.ToArray();
    }

    public record struct Machine(int aX, int aY, int bX, int bY, int prizeX, int prizeY);

    [Part(1)]
    public void Part01()
    {
        var answer = 0;
        for (int i = 0; i < _machines.Length; i++)
        {
            var machine = _machines[i];

            int prizeX = machine.prizeX;
            int prizeY = machine.prizeY;

            var max = 200;
            var minTokens = 0;
            for (int a = max; a >= 0; a--)
            {
                for (int b = max - a; b >= 0; b--)
                {
                    var valueX = a * machine.aX + b * machine.bX;
                    var valueY = a * machine.aY + b * machine.bY;

                    if (valueX == prizeX && valueY == prizeY)
                    {
                        var tokens = (a * 3) + b;
                        if (minTokens == 0)
                            minTokens = tokens;
                        minTokens = tokens < minTokens ? tokens : minTokens;
                    }
                }
            }

            answer += minTokens;
        }

        Console.WriteLine($"Part 1: {answer}");
    }

    [Part(2)]
    public void Part02()
    {
        var answer = 0L;
        foreach (var machine in _machines)
        {
            int aX = machine.aX;
            int aY = machine.aY;
            int bX = machine.bX;
            int bY = machine.bY;
            long prizeX = machine.prizeX + 10_000_000_000_000;
            long prizeY = machine.prizeY + 10_000_000_000_000;

            var _1 = aX * bY;
            var _2 = bX * aY;

            // bereken de determinator
            // kruis berekening?
            var determinant = (_1) - (_2);

            // Er zijn geen intersections voor deze machine.
            if (determinant == 0)
                continue;

            var _3 = prizeX * bY;
            var _4 = prizeY * bX;

            var _5 = _1 - _2; // is determinant
            var _6 = _3 - _4;

            if (_6 % _5 != 0)
                continue;

            var press_a = _6 / _5;

            if (press_a < 0)
                continue;

            var _7 = press_a * aX;
            var _8 = prizeX - _7;

            if (_8 % bX != 0)
                continue;

            var press_b = _8 / bX;

            if (press_b < 0)
                continue;

            var cost = (press_a * 3) + press_b;

            answer += cost;
        }

        Console.WriteLine($"Part 2: {answer}");
    }
}
