using AdventOfCode._2015;

namespace AdventOfCode.Tests;
public class Day08_MatchsticksTests
{
    [Theory]
    [InlineData("\"\"", 2)]
    [InlineData("\"abc\"", 5)]
    [InlineData("\"aaa\\\"aaa\"", 10)]
    [InlineData("\"\\x27\"", 6)]
    public void NumberOfInCodeChars_ShouldReturnNumberOfChars(string input, int expected)
    {
        // Arrange 

        // Act
        var actual = Day08_Matchsticks.NumberOfInCodeChars(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("\"\"", 0)]
    [InlineData("\"abc\"", 3)]
    [InlineData("\"aaa\\\"aaa\"", 7)]
    [InlineData("\"\\x27\"", 1)]
    public void NumberOfInMemoryChars_ShouldReturnNumberOfChars(string input, int expected)
    {
        // Arrange 

        // Act
        var actual = Day08_Matchsticks.NumberOfInMemoryChars(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("\"\"", 6)]
    [InlineData("\"abc\"", 9)]
    [InlineData("\"aaa\\\"aaa\"", 16)]
    [InlineData("\"\\x27\"", 11)]
    public void NumberOfEncodedChars_ShouldReturnNumberOfChars(string input, int expected)
    {
        // Arrange 

        // Act
        var actual = Day08_Matchsticks.NumberOfEncodedChars(input);

        // Assert
        Assert.Equal(expected, actual);
    }
}
