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
                    string newMolecule = ReplaceAt(_molecule, m, r, index);

                    generatedMolocules.Add(newMolecule);
                    index++;
                }
            }
            while (index > 0 && index <= _molecule.Length - 1);
        }

        Console.WriteLine("Unique molecules: {0}", generatedMolocules.Count);
    }

    private static string ReplaceAt(string original, string oldStr, string newStr, int index)
    {
        var span = original.AsSpan();
        var begin = span[..index];
        var end = span[(index + oldStr.Length)..];

        return string.Concat(begin, newStr, end);
    }

    [Part(2)]
    public void Part02()
    {
        var startValue = "e";

        var depth = BreadthFirstSearch(_molecule, startValue);

        Console.WriteLine("Depth found: {0}", depth);
    }

    private int BreadthFirstSearch(string molecule, string startValue)
    {
        var visited = new HashSet<string>();

        var queue = new Queue<(string, int)>();
        queue.Enqueue((startValue, 0));

        while (queue.TryDequeue(out var o))
        {
            var current = o.Item1;
            var depth = o.Item2;

            if (current == molecule)
                return depth;

            foreach ((var m2, var r) in _replacements.Where(i => current.Contains(i.Item1)).ToList())
            {
                var index = 0;
                do
                {
                    index = current.IndexOf(m2, index);
                    if (index >= 0)
                    {
                        string newMolecule = ReplaceAt(current, m2, r, index);

                        if (!visited.Contains(newMolecule) && (newMolecule.Length <= molecule.Length))
                            queue.Enqueue((newMolecule, depth + 1));

                        visited.Add(newMolecule);
                        index++;
                    }
                }
                while (index > 0 && index <= _molecule.Length - 1);
            }
        }

        return -1;
    }
}
