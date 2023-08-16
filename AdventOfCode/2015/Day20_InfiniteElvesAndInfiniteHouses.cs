using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2015;

[Challenge(2015, 20)]
internal class Day20_InfiniteElvesAndInfiniteHouses
{
    [Part(1)]
    public static void Part01()
    {
        var house = Solve();
        Console.WriteLine($"House {house} is lowest number with gets presents");
    }

    [Part(2)]
    public static void Part02()
    {
        var house = Solve(isPart2: true);
        Console.WriteLine($"House {house} is lowest number with gets presents");
    }

    private static long Solve(bool isPart2 = false)
    {
        var puzzleInput = 34_000_000;

        // For part A we don't have to multiply, since we also divided the input by 10
        var divide = isPart2 ? 1 : 10;
        var multiply = isPart2 ? 11 : 1;

        var input = puzzleInput / divide;
        var houses = new int[input];

        var results = new List<int>();
        for (var elf = 1; elf < input; elf++)
        {
            var visits = 0;

            for (var house = elf; house < input; house += elf)
            {
                houses[house] += elf * multiply;

                if (houses[house] >= input)
                    results.Add(house);

                visits++;

                if (isPart2 && visits == 50)
                    break;
            }
        }

        return results.Min();
    }
}
