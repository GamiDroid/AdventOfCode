using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2015;

[Challenge(2015, 19)]
internal class Day19_MedicineForRudolph
{
    private ICollection<(string, string)> _replacements = Enumerable.Empty<(string, string)>().ToList();
    private string _molecule = default!;

    [Setup]
    public void Setup()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var lines = File.ReadAllLines(filePath);

        var replacements = new List<(string, string)>();

        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) break;
            
            var split = lines[i].Split("=>", StringSplitOptions.TrimEntries);

            replacements.Add((split[0], split[1]));
        }

        _replacements = replacements;
        _molecule = lines[^1];
    }

    [Part(1)]
    public void Part01()
    {
        Console.WriteLine("Executing part 1...");

        var generatedMolocules = new HashSet<string>();
        foreach ((var m, var r) in _replacements)
        {
            var index = 0;
            do
            {
                index = _molecule.IndexOf(m, index);
                if (index >= 0)
                {
                    var newMolecule = ReplaceAt(_molecule, m, r, index).ToString();

                    generatedMolocules.Add(newMolecule);
                    index++;
                }
            }
            while (index > 0 && index <= _molecule.Length - 1);
        }

        Console.WriteLine("Unique molecules: {0}", generatedMolocules.Count);
    }

    private static ReadOnlySpan<char> ReplaceAt(ReadOnlySpan<char> original, ReadOnlySpan<char> oldStr, ReadOnlySpan<char> newStr, int index)
    {
        var begin = original[..index];
        var end = original[(index + oldStr.Length)..];

        return string.Concat(begin, newStr, end);
    }

    [Part(2)]
    public void Part02()
    {
        var startValue = "e";

        var molecule = "HOHOHO";
        var replacements = new List<(string, string)>()
        {
            ("e", "O"),
            ("e", "H"),
            ("H", "OH"),
            ("H", "HO"),
            ("O", "HH"),
        };

        var depth = BreadthFirstSearch(startValue, _replacements, _molecule);

        Console.WriteLine("Depth found: {0}", depth);
    }

    private static int BreadthFirstSearch(string molecule, IEnumerable<(string Old, string New)> replacements, string startValue)
    {
        var visited = new HashSet<string>();

        var queue = new PriorityQueue<(string, int), int>();
        queue.Enqueue((startValue, 0), startValue.Length);

        while (queue.TryDequeue(out var i, out var _))
        {
            var current = i.Item1;
            var depth = i.Item2;

            if (current == molecule)
                return depth;

            foreach ((var o, var n) in replacements)
            {
                var index = 0;
                do
                {
                    index = current.IndexOf(n, index);
                    if (index >= 0)
                    {
                        var newMolecule = ReplaceAt(current, n, o, index).ToString();

                        if (newMolecule == molecule) return depth+1;

                        if (!visited.Contains(newMolecule))
                            queue.Enqueue((newMolecule, depth + 1), newMolecule.Length);

                        visited.Add(newMolecule);
                        index++;
                    }
                }
                while (index > 0 && index <= molecule.Length - 1);
            }
        }

        return -1;
    }
}
