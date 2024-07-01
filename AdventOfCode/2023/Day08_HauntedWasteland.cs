using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static AdventOfCode._2015.Day23_OpeningTheTuringLock;

namespace AdventOfCode._2023;
[Challenge(2023, 8, "Haunted Wasteland")]
internal class Day08_HauntedWasteland
{
    private readonly string _instructions;
    private readonly Dictionary<string, Node> _nodeMap;

    public Day08_HauntedWasteland()
    {
        var inputPath = ChallengeHelper.GetResourceFilePath();
        var lines = File.ReadAllLines(inputPath);

        _instructions = lines[0];

        var nodes = new List<Node>();
        foreach (var line in lines.Skip(2))
        {
            var match = Regex.Match(line, "(?<n>\\w{3}) = \\((?<l>\\w{3}), (?<r>\\w{3})\\)");
            if (match.Success)
            {
                nodes.Add(new Node(match.Groups["n"].Value, match.Groups["l"].Value, match.Groups["r"].Value));
            }
        }

        _nodeMap = nodes.ToDictionary(n => n.Name);
    }

    [Part(1)]
    public void Part01()
    {
        var currentNode = _nodeMap["AAA"];

        int stepCounter = 0;
        while (true)
        {
            bool found = false;
            foreach (var instruction in _instructions)
            {
                stepCounter++;

                currentNode = _nodeMap[instruction == 'L' ? currentNode.Left : currentNode.Right];

                if (currentNode.Name == "ZZZ")
                {
                    found = true;
                    break;
                }
            }

            if (found) break;
        }

        Console.WriteLine($"It took {stepCounter} steps to reach ZZZ");
    }

    [Part(2)]
    public void Part02()
    {
        var currentNodes = _nodeMap.Values.Where(n => n.Name[2] == 'A').ToArray();
        var endFoundStep = new long[currentNodes.Length];
        var endFoundCounter = new int[currentNodes.Length];
        int stepCounter = 0;

        var foundCounter = 0;
        bool found = false;
        while (true)
        {
            foreach (var instruction in _instructions)
            {
                stepCounter++;
                for (int nodeIndex = 0; nodeIndex < currentNodes.Length; nodeIndex++)
                {
                    currentNodes[nodeIndex] = _nodeMap[instruction == 'L' ? currentNodes[nodeIndex].Left : currentNodes[nodeIndex].Right];

                    if (currentNodes[nodeIndex].Name[2] == 'Z')
                    {
                        if (endFoundCounter[nodeIndex] < 1)
                        {
                            Console.WriteLine($"{nodeIndex} {stepCounter}");

                            endFoundStep[nodeIndex] = stepCounter;
                        }
                        else if (endFoundCounter[nodeIndex] == 1)
                        {
                            endFoundStep[nodeIndex] = stepCounter - endFoundStep[nodeIndex];
                            Console.WriteLine($"found {nodeIndex} {endFoundStep[nodeIndex]} {stepCounter}");
                            foundCounter++;
                        }

                        endFoundCounter[nodeIndex]++;
                    }
                }

                if (foundCounter >= currentNodes.Length)
                {
                    found = true;
                    break;
                }
            }

            if (found) break;
        }

        var lcm = GetLeastCommonMultiple(endFoundStep);

        Console.WriteLine($"It took {lcm} steps to reach ZZZ");
    }


    private static long GetHighestCommonFactor(long a, long b)
    {
        if (b == 0)
            return a;
        return GetHighestCommonFactor(b, a % b);
    }

    private static long GetLeastCommonMultiple(long [] arr)
    {
        return arr.Aggregate((x, y) => x * y / GetHighestCommonFactor(x, y));
    }

    public record Node(string Name, string Left, string Right);
}
