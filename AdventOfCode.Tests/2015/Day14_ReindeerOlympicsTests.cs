using AdventOfCode._2015;

namespace AdventOfCode.Tests;
public class Day14_ReindeerOlympicsTests
{
    [Fact]
    public void Reindeer_Create_ShouldHaveNoDistanceTraveled()
    {
        // Arrange
        var reindeer = new Day14_ReindeerOlympics.Reindeer("", 0, 0, 0);

        // Act
        var actual = reindeer.DistanceTraveled;

        // Assert
        Assert.Equal(0, actual);
    }

    [Fact]
    public void Reindeer_ElapseTime_ShouldAddDistanceTraveled()
    {
        // Arrange
        var expected = 5;
        var reindeer = new Day14_ReindeerOlympics.Reindeer("", expected, 0, 0);

        // Act
        reindeer.ElapseTime();
        var actual = reindeer.DistanceTraveled;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Reindeer_IsRunning_ShouldBeFalseWhenRunTimeIsUp()
    {
        // Arrange
        var reindeer = new Day14_ReindeerOlympics.Reindeer("", 0, 1, 10);

        // Act 
        reindeer.ElapseTime(2);
        var actual = reindeer.IsRunning;

        // Assert 
        Assert.False(actual);
    }

    [Fact]
    public void Reindeer_ElapseTime_ShouldNotIncrementDistanceTraveledWhenIsRunningIsFalse()
    {
        // Arrange
        var reindeer = new Day14_ReindeerOlympics.Reindeer("", 5, 1, 10);

        // Act 
        reindeer.ElapseTime(3);

        // Assert 
        Assert.False(reindeer.IsRunning);
        Assert.Equal(5, reindeer.DistanceTraveled);
    }
}
