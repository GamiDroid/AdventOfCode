using AdventOfCode._2023;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Tests._2023;
public class Day06_WaitForItTests
{
    [Theory]
    [InlineData(0, 7, 0)]
    [InlineData(1, 7, 6)]
    [InlineData(2, 7, 10)]
    [InlineData(3, 7, 12)]
    [InlineData(4, 7, 12)]
    [InlineData(5, 7, 10)]
    [InlineData(6, 7, 6)]
    [InlineData(7, 7, 0)]
    public void CalculateDistance_ShouldResultTheExpectedDistanceWhenTheChargeIsHold(int timeHold, int totalTime, ulong expected)
    {
        // arrange

        // act
        var actual = Day06_WaitForIt.CalculateDistance(timeHold, totalTime);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(7, 9, 4)]
    [InlineData(15, 40, 8)]
    [InlineData(71530, 940200, 71503)]
    public void CountWins_ShouldReturnAmountOfPossibleWins(int time, ulong record, int expected)
    {
        // act
        var actual = Day06_WaitForIt.CountWins(time, record);

        // assert
        Assert.Equal(expected, actual);
    }

}
