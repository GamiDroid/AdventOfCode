using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventOfCode._2015;

[Challenge(2015, 17)]
internal class Day17_NoSuchThingAsTooMuch
{
    private int[] _containerSizes = Array.Empty<int>();

    [Setup]
    public void Setup()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var lines = File.ReadAllLines(filePath);

        _containerSizes = GetContainerSizes(lines);
    }

    private static int[] GetContainerSizes(string[] lines)
    {
        var sizes = new List<int>();
        foreach (var line in lines)
        {
            if (int.TryParse(line, out int size))
                sizes.Add(size);
        }
        return sizes.ToArray();
    }

    [Part(1)]
    public void Part01()
    {
        Console.WriteLine($"There are {_containerSizes.Length} container sizes");

        var count = CountCombinations(_containerSizes, 150);

        Console.WriteLine("Amount of combinations: {0}", count);
    }

    private static int CountCombinations(int[] containers, int neededSize)
    {
        var count = Permutations(containers, neededSize, 0, 0, 0);

        return count;
    }

    private static int Permutations(int[] containerSizes, int neededSize, int index, int size, int count)
    {
        if (index == containerSizes.Length || size >= neededSize)
        {
            if (size == neededSize)
            {
                return count + 1;
            }
        }
        else
        {
            for (int i = index; i < containerSizes.Length; i++)
            {
                int newSize = size + containerSizes[i];
                count = Permutations(containerSizes, neededSize, i + 1, newSize, count);
            }
        }

        return count;
    }
}
