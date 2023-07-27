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
    }
}
