using AdventOfCode.ConsoleApp._2015;

namespace AdventOfCode.Tests;
public class Day03_PerfectlySphericalHousesInAVacuumTests
{
    [Theory]
    [InlineData(">", 2)]
    [InlineData("^>v<", 4)]
    [InlineData("^v^v^v^v^v", 2)]
    public void GetTotalVisitedHouses_ShouldReturnTotalVisitedHouses(string input, int expected)
    {
        // Act
        var actual = Day03_PerfectlySphericalHousesInAVacuum.GetTotalVisitedHouses(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("^v", 3)]
    [InlineData("^>v<", 3)]
    [InlineData("^v^v^v^v^v", 11)]
    public void GetTotalVisitedHousesWithRoboSanta_ShouldReturnTotalVisitedHouses(string input, int expected)
    {
        // Act
        var actual = Day03_PerfectlySphericalHousesInAVacuum.GetTotalVisitedHousesWithRoboSanta(input);

        // Assert
        Assert.Equal(expected, actual);
    }
}
