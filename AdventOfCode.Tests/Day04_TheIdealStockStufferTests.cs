using AdventOfCode._2015;

namespace AdventOfCode.Tests;
public class Day04_TheIdealStockStufferTests
{
    [Theory]
    [InlineData("abcdef", 609043)]
    [InlineData("pqrstuv", 1048970)]
    [InlineData("bgvyzdsv", 254575)]
    public void FindNumberWhereMD5HashBeginsWith_ShouldReturnLowestNumberFor5Zeros(string secretKey, int expected)
    {
        //Act
        var actual = Day04_TheIdealStockingStuffer.FindNumberWhereMD5HashBeginsWith(secretKey, "00000");

        //Assert
        Assert.Equal(expected, actual);
    }
}
