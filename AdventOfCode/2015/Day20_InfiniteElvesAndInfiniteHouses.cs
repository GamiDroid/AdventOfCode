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
    public void Part01()
    {
        var house = Solve();
        Console.WriteLine($"House {house} is lowest number with gets presents");
    }

    private static int Solve()
    {
        var puzzleInput = 34000000;

        // For part A we don't have to multiply, since we also divided the input by 10
        var divide =  10;
        var multiply = 1;

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
            }
        }

        return results.Min();
    }
}
