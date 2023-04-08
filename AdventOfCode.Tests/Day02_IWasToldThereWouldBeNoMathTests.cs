using AdventOfCode.ConsoleApp._2015;

namespace AdventOfCode.Tests;
public class Day02_IWasToldThereWouldBeNoMathTests
{
    [Theory]
    [InlineData(2, 3, 4, 58)]
    [InlineData(1, 1, 10, 43)]
    public void GetSquareFeetOfWrappingPaper_ShouldReturnSquareFeet(int l, int h, int w, int expected)
    {
        // Act
        var actual = Day02_IWasToldThereWouldBeNoMath.GetSquareFeetOfWrappingPaper(l, h, w);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(2, 3, 4, 34)]
    [InlineData(1, 1, 10, 14)]
    public void GetFeetOfRibbon_ShouldReturnFeet(int l, int h, int w, int expected)
    {
        // Act
        var actual = Day02_IWasToldThereWouldBeNoMath.GetFeetOfRibbon(l, h, w);

        // Assert
        Assert.Equal(expected, actual);
    }
}
