using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2023;

[Challenge(year: 2023, day: 15, "Lens Library")]
internal class Day15_LensLibrary
{
    public Day15_LensLibrary()
    {
        ChallengeHelper.ReadAllTextFromResourceFile();
    }

    [Part(1)]
    public void Part01()
    {
        var input = ChallengeHelper.ReadAllTextFromResourceFile();
        var words = input.Split(',');
        var value = words.Select(w => Hash(w)).Sum();
        Console.WriteLine($"Part 1: {value}");
    }

    [Part(2)]
    public void Part02()
    {
        Dictionary<int, List<(string label, int focalLength)>> boxes = [];

        var input = ChallengeHelper.ReadAllTextFromResourceFile();

        foreach (Match match in Regex.Matches(input, "(?<w>\\w+)(?<s>[=-])(?<n>\\d+)?"))
        {
            var word = match.Groups["w"];
            var symbol = match.Groups["s"];
            
            var boxNr = Hash(word.Value);
            if (!boxes.TryGetValue(boxNr, out var box))
            {
                box = [];
                boxes[boxNr] = box;
            }

            if (symbol.Value == "=")
            {
                var number = match.Groups["n"];
                var index = box.FindIndex(x => x.label == word.Value);
                if (index >= 0)
                {
                    box[index] = (word.Value, int.Parse(number.Value));
                }
                else
                {
                    box.Add((word.Value, int.Parse(number.Value)));
                }
            }
            else
            {
                box.RemoveAll(x => x.label == word.Value);
            }
        }

        long sum = 0;
        foreach (var box in boxes)
        {
            for (int i = 0; i < box.Value.Count; i++)
            {
                var power = (box.Key + 1) * (i + 1) * box.Value[i].focalLength;
                sum += power;

                Console.WriteLine($"{box.Value[i].label}: {(box.Key + 1)} (box {box.Key}) * {i + 1} * {box.Value[i].focalLength} = {power}");
            }
        }

        Console.WriteLine($"Part 2: {sum}");
    }

    private static int Hash(string word)
    {
        int value = 0;
        foreach (char c in word)
        {
            value += c;
            value *= 17;
            value %= 256;
        }
        return value;
    }
}
