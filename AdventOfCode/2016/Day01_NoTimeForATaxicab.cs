using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2016;
[Challenge(2016, 1, "No Time for a Taxicab")]
internal class Day01_NoTimeForATaxicab
{
    private readonly ICollection<Instruction>? _instructions;

    public Day01_NoTimeForATaxicab()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var fileContent = File.ReadAllText(filePath);

        var instructions = new List<Instruction>();
        var instructionsAsStrings = fileContent.Split(',');
        foreach (var instuctionAsString  in instructionsAsStrings)
        {
            var match = Regex.Match(instuctionAsString, "([LR])(\\d+)");
            if (match.Success)
            {
                var turnTo = match.Groups[1].ValueSpan[0];
                var steps = int.Parse(match.Groups[2].Value);

                var instruction = new Instruction(turnTo, steps);
                instructions.Add(instruction);
            }
        }

        _instructions = instructions;
    }

    [Part(1)]
    public void Part01()
    {
        if (_instructions is null)
            throw new InvalidDataException("Instructions are not set");

        Console.WriteLine("Following the instructions...");

        int x = 0, y = 0;
        var direction = Direction.North;
        foreach (var instruction in _instructions)
        {
            direction = Turn(instruction.TurnTo, direction);
            (x, y) = GoForward(x, y, direction, instruction.Steps);
        }

        Console.WriteLine("After the instructions the position is [ X: {0}, Y: {1} ]", x, y);

        var distance = Math.Abs(x) + Math.Abs(y);
        Console.WriteLine("This is {0} blocks away", distance);
    }

    [Part(2)]
    public void Part02()
    {
        if (_instructions is null)
            throw new InvalidDataException("Instructions are not set");

        (int x, int y) = GetFirstTwiceVisitedLocation(_instructions);

        Console.WriteLine("The first position that is visited twice is [ X: {0}, Y: {1} ]", x, y);

        var distance = Math.Abs(x) + Math.Abs(y);
        Console.WriteLine("This is {0} blocks away", distance);
    }

    private static (int x, int y) GetFirstTwiceVisitedLocation(IEnumerable<Instruction> instructions)
    {
        Console.WriteLine("Following the instructions...");

        // add unique list of visited positions
        // directly add the first position
        HashSet<(int x, int y)> visited = new() { (0, 0) };

        int x = 0, y = 0;
        var direction = Direction.North;
        foreach (var instruction in instructions)
        {
            direction = Turn(instruction.TurnTo, direction);
            (int newX, int newY) = GoForward(x, y, direction, instruction.Steps);

            if (newX == x)
            {
                var numbers = GetNumbersBetween(y, newY);
                foreach (var num in numbers)
                {
                    if (!visited.Add((x, num)))
                        return (x, num);
                }
            }
            else if (newY == y)
            {
                var numbers = GetNumbersBetween(x, newX);
                foreach (var num in numbers)
                {
                    if (!visited.Add((num, y)))
                        return (num, y);
                }
            }

            (x, y) = (newX, newY);

            if (!visited.Add((x, y)))
                return (x, y);
        }

        return (0, 0);
    }

    private static int[] GetNumbersBetween(int start, int end)
    {
        int numbersBetween = Math.Abs(end - start) - 1;

        if (numbersBetween == 0)
            return Array.Empty<int>();

        var numbers = new int[numbersBetween];

        int i = 0;
        while (i < numbersBetween)
        {
            numbers[i] = start < end ? start + (i + 1) : start - (i + 1);
            i++;
        }

        return numbers;
    }

    private static Direction Turn(char turn, Direction direction)
    {
        var directionAsInt = (int)direction;
        directionAsInt += turn == 'R' ? 1 : -1;

        if (directionAsInt < 0)
            directionAsInt = 3;
        else if (directionAsInt > 3)
            directionAsInt = 0;

        return (Direction)directionAsInt;
    }

    private static (int x, int y) GoForward(int x, int y, Direction direction, int steps)
    {
        return direction switch
        {
            Direction.North => (x, y + steps),
            Direction.South => (x, y - steps),
            Direction.East => (x + steps, y),
            Direction.West => (x - steps, y),
            _ => throw new ArgumentException("Given direction is unknown", nameof(direction))
        };
    }

    private enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3,
    }

    private record struct Instruction(char TurnTo, int Steps);
}
