using AdventOfCode._2015;

namespace AdventOfCode.Tests;

public class Day11_CorporatePolicyTests
{
    [Fact]
    public void PasswordGenerator_Create_ShouldHaveSamePassword()
    {
        var expected = "abcdefgh";
        // Arrange 
        var passwordGen = new Day11_CorporatePolicy.PasswordGenerator(expected);

        // Act
        var actual = passwordGen.Password;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void PasswordGenerator_Create_ShouldHaveSamePasswordWithEightAs()
    {
        var expected = "aaaaaaaa";
        // Arrange 
        var passwordGen = new Day11_CorporatePolicy.PasswordGenerator();

        // Act
        var actual = passwordGen.Password;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("aaa aaaa")]
    public void PasswordGenerator_Create_ShouldThrowArgumentException(string password)
    {
        // Arrange 
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => new Day11_CorporatePolicy.PasswordGenerator(password));
    }

    [Theory]
    [InlineData("aaaaaaaa", "aaaaaaab")]
    [InlineData("aaaaaaaz", "aaaaaaba")]
    [InlineData("azzzzzzz", "baaaaaaa")]
    public void PasswordGenerator_Next_ShouldGiveNextPassword(string password, string expected)
    {
        // Arrange
        var passwordGen = new Day11_CorporatePolicy.PasswordGenerator(password);
        // Act
        passwordGen.Next();
        var actual = passwordGen.Password;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void PasswordGenerator_Next_ShouldThrow()
    {
        // Arrange
        var passwordGen = new Day11_CorporatePolicy.PasswordGenerator("zzzzzzzz");
        // Acts
        // Assert
        Assert.ThrowsAny<Exception>(passwordGen.Next);
    }

    [Theory]
    [InlineData("aabccxmp")]
    public void PasswordGenerator_Validate_ShouldReturnTrue(string password)
    {
        // Arrange
        var passwordGen = new Day11_CorporatePolicy.PasswordGenerator(password);

        // Act
        var actual = passwordGen.Validate();

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [InlineData("aaaaaaaa")]
    [InlineData("aaaaaaaz")]
    public void PasswordGenerator_Validate_ShouldReturnFalse(string password)
    {
        // Arrange
        var passwordGen = new Day11_CorporatePolicy.PasswordGenerator(password);

        // Act
        var actual = passwordGen.Validate();

        // Assert
        Assert.False(actual);
    }
}