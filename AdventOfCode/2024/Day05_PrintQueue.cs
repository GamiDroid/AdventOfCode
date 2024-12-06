using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024;

[Challenge(2024, 5, "Print Queue")]
internal class Day05_PrintQueue
{
    private readonly int[][] _updatesList;
    private readonly ReadOnlyDictionary<int, List<int>> _pageOrderRules;

    public Day05_PrintQueue()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();
        Dictionary<int, List<int>> pageOrderRules = [];
        List<int[]> updatesList = [];

        bool b = false;
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                b = true;
                continue;
            }

            if (b)
            {
                updatesList.Add(line.Split(',').Select(n => int.Parse(n)).ToArray());
            }
            else
            {
                var numbers = line.Split('|').Select(n => int.Parse(n)).ToArray();
                if (!pageOrderRules.TryGetValue(numbers[0], out var list))
                {
                    list = [];
                    pageOrderRules[numbers[0]] = list;
                }
                list.Add(numbers[1]);
            }
        }

        _updatesList = [.. updatesList];
        _pageOrderRules = pageOrderRules.AsReadOnly();
    }

    [Part(1)]
    public void Part01()
    {
        List<int> correctOrderedUpdateIndexes = [];
        for (int i = 0; i < _updatesList.Length; i++)
        {
            bool correct = true;
            for (int j = 1; j < _updatesList[i].Length; j++)
            {
                var updates = _updatesList[i];
                var update = updates[j];

                if (_pageOrderRules.TryGetValue(update, out var list))
                {
                    if (updates[..j].Any(x => list.Contains(x)))
                    {
                        correct = false;
                        break;
                    }
                }
            }

            if (correct)
            {
                correctOrderedUpdateIndexes.Add(i);
            }
        }

        int sum = 0;
        foreach (var index in correctOrderedUpdateIndexes)
        {
            var updates = _updatesList[index];
            sum += updates[updates.Length / 2]; 
        }

        Console.WriteLine($"Part 1: {sum}");
    }

    [Part(2)]
    public void Part02()
    {
        int sum = 0;
        foreach (var updates in _updatesList)
        {
            List<int> newUpdates = [.. updates];
            bool correct = true;
            for (int j = 1; j < updates.Length; j++)
            {
                var update = newUpdates[j];

                if (_pageOrderRules.TryGetValue(update, out var list))
                {
                    if (newUpdates[..j].Any(x => list.Contains(x)))
                    {
                        correct = false;
                        var valueIndex = updates
                            .Select((v, i) => (Value: v, Index: i))
                            .First(x => list.Contains(x.Value));
                        var newIndex = valueIndex.Index > 0 ? valueIndex.Index - 1 : 0;
                        newUpdates.RemoveAt(j);
                        newUpdates.Insert(newIndex, update);
                        j = newIndex;
                    }
                }
            }

            if (!correct)
            {
                sum += newUpdates[updates.Length / 2];
            }
        }

        Console.WriteLine($"Part 2: {sum}");
    }
}
