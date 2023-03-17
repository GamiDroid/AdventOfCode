using AdventOfCode.ConsoleApp._2015;

namespace AdventOfCode.Tests;

public class NotQuiteLispTests
{
    [Theory]
    [InlineData("(())", 0)]
    [InlineData("()()", 0)]
    [InlineData("(((", 3)]
    [InlineData("(()(()(", 3)]
    [InlineData("))(((((", 3)]
    [InlineData("())", -1)]
    [InlineData("))(", -1)]
    [InlineData(")))", -3)]
    [InlineData(")())())", -3)]
    public void GetFloorNumber_ShouldReturnFloorNumber(string input, int expected)
    {
        // Arrange
        // Act 
        int actual = Day01_NotQuiteLisp.GetFloorNumber(input);

        // Assert
        Assert.Equal(expected, actual);
    }
}