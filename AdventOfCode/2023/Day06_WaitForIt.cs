using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2023;
[Challenge(2023, 6, "Wait For It")]
internal class Day06_WaitForIt
{
    private static readonly int[][] s_races = [
        [46, 214], 
        [80, 1177], 
        [78, 1402], 
        [66, 1024]
    ];

    [Part(1)]
    public static void Part01()
    {
        int result = 1;

        // for each race...
        for (int r = 0; r < s_races.Length; r++)
        {
            var time = s_races[r][0];
            var recordDistance = s_races[r][1];

            int winCounter = CountWins(time, (ulong)recordDistance);

            result *= winCounter;
        }

        Console.WriteLine($"Result part 1: {result}");
    }

    [Part(1)]
    public static void Part02()
    {
        int result = CountWins(time: 46807866, recordDistance: 214117714021024);
        Console.WriteLine($"Result part 2: {result}");
    }

    public static int CountWins(int time, ulong recordDistance)
    {
        int minTimeHoldToWin = -1;
        int maxTimeHoldToWin = -1;

        for (int i = 1; i < time; i++)
        {
            if (minTimeHoldToWin != -1 && maxTimeHoldToWin != -1)
                break;

            var distance = CalculateDistance(i, time);
            if (minTimeHoldToWin == -1)
            {
                // find min
                if (distance > recordDistance)
                {
                    minTimeHoldToWin = i;
                }
            }
            else if (maxTimeHoldToWin == -1)
            {
                // find max
                if (distance < recordDistance)
                {
                    maxTimeHoldToWin = i;
                }
            }
        }

        return maxTimeHoldToWin - minTimeHoldToWin;
    }

    public static ulong CalculateDistance(int chargeHoldTime, int totalRaceTime)
    {
        // not charged...
        if (chargeHoldTime <= 0)
            return 0;

        var timeToTravel = totalRaceTime - chargeHoldTime;
        // no time to travel...
        if (timeToTravel <= 0)
            return 0;

        return (ulong)chargeHoldTime * (ulong)timeToTravel;
    }
}
