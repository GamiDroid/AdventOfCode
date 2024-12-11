using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024;

[Challenge(2024, 11, "Plutonian Pebbles")]
internal class Day11_PlutonianPebbles
{
    private readonly List<ulong> _stones;

    public Day11_PlutonianPebbles()
    {
        var input = ChallengeHelper.ReadAllTextFromResourceFile();
        _stones = input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => ulong.Parse(x)).ToList();
    }

    [Part(1)]
    public void Part01()
    {
        Dictionary<ulong, ulong> stones = _stones.ToDictionary(x => x, x => (ulong)1);
        for (int i = 0; i < 25; i++)
        {
            stones = Blink(stones);
        }
        
        ulong answer = Calc(stones);
        Console.WriteLine($"Part 1: {answer}");
    }

    [Part(2)]
    public void Part02()
    {
        Dictionary<ulong, ulong> stones = _stones.ToDictionary(x => x, x => (ulong)1);
        for (int i = 0; i < 75; i++)
        {
            stones = Blink(stones);
        }

        ulong answer = Calc(stones);
        Console.WriteLine($"Part 2: {answer}");
    }

    private static ulong Calc(Dictionary<ulong, ulong> stones)
    {
        ulong sum = 0;
        foreach (var val in stones)
        {
            sum += val.Value;
        }
        return sum;
    }

    private static Dictionary<ulong, ulong> Blink(Dictionary<ulong, ulong> stones)
    {
        Dictionary<ulong, ulong> newStones = [];
        var stoneNumbers = stones.Where(x => x.Value > 0).Select(x => x.Key).ToList();
        foreach (var stone in stoneNumbers)
        {
            if (stones[stone] == 0)
                continue;

            if (stone == 0)
            {
                AddToDict(newStones, 1, stones[stone]);
                stones[0] = 0;
            }
            else
            {
                var stoneAsText = $"{stone}".AsSpan();
                if (stoneAsText.Length % 2 == 0)
                {
                    var count = stoneAsText.Length / 2;
                    
                    ulong val1 = ulong.Parse(stoneAsText[..count]);
                    ulong val2 = ulong.Parse(stoneAsText.Slice(count, count));

                    var add = stones[stone];

                    AddToDict(newStones, val1, add);
                    AddToDict(newStones, val2, add);

                    stones[stone] = 0;
                }
                else
                {
                    AddToDict(newStones, stone * 2024, stones[stone]);
                    stones[stone] = 0;
                }
            }
        }

        return newStones;
    }

    private static void AddToDict(Dictionary<ulong, ulong> stones, ulong stone, ulong count)
    {
        ref ulong counterRef = ref CollectionsMarshal.GetValueRefOrAddDefault(stones, stone, out bool exists); 
        if (exists) 
            counterRef += count; 
        else 
            counterRef = count;
    }
}
