using AdventOfCode.ConsoleApp._2015;

namespace AdventOfCode.Tests;
public class Day06_ProbablyAFireHazardTests
{
    [Theory]
    [InlineData(0, 0, 999, 999, 1_000_000)]
    [InlineData(0, 0, 999, 0, 1_000)]
    [InlineData(499, 499, 500, 500, 4)]
    public void Lights_LitCount_ShouldReturnAmountOfLitLights(int fromX, int fromY, int toX, int toY, int expected)
    {
        // Arrange
        var lights = new Day06_ProbablyAFireHazard.Lights();

        // Act
        lights.TurnOn(fromX, fromY, toX, toY);
        var actual = lights.CountLit();

        // Assert
        Assert.Equal(expected, actual);
    }
}
