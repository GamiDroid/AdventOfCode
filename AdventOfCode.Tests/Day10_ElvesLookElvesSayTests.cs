using AdventOfCode.ConsoleApp._2015;

namespace AdventOfCode.Tests;

public class Day10_ElvesLookElvesSayTests
{
    [Theory]
    [InlineData("1", "11")]
    [InlineData("11", "21")]
    [InlineData("21", "1211")]
    [InlineData("1211", "111221")]
    [InlineData("111221", "312211")]
    public void LookAndSay_ShouldReturnParsedValue(string input, string expected)
    {
        // Arrange

        // Act 
        var actual = Day10_ElvesLookElvesSay.LookAndSay(input);

        // Assert
        Assert.Equal(expected, actual);
    }
}