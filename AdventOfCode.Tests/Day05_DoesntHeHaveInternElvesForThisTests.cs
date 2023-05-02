using AdventOfCode._2015;

namespace AdventOfCode.Tests;
public class Day05_DoesntHeHaveInternElvesForThisTests
{
    [Theory]
    [InlineData("ugknbfddgicrmopn")]
    [InlineData("aaa")]
    public void IsNiceString_ShouldReturnTrue(string input)
    {
        // Act 
        var actual = Day05_DoesntHeHaveInternElvesForThis.IsNiceString(input);

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [InlineData("jchzalrnumimnmhp")]
    [InlineData("haegwjzuvuyypxyu")]
    [InlineData("dvszwmarrgswjxmb")]
    public void IsNiceString_ShouldReturnFalse(string input)
    {
        // Act 
        var actual = Day05_DoesntHeHaveInternElvesForThis.IsNiceString(input);

        // Assert
        Assert.False(actual);
    }

    [Theory]
    [InlineData("qjhvhtzxzqqjkmpb")]
    [InlineData("xxyxx")]
    public void IsBetterNiceString_ShouldReturnTrue(string input)
    {
        // Act 
        var actual = Day05_DoesntHeHaveInternElvesForThis.IsBetterNiceString(input);

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [InlineData("uurcxstgmygtbstg")]
    [InlineData("ieodomkazucvgmuy")]
    public void IsBetterNiceString_ShouldReturnFalse(string input)
    {
        // Act 
        var actual = Day05_DoesntHeHaveInternElvesForThis.IsBetterNiceString(input);

        // Assert
        Assert.False(actual);
    }

    [Theory]
    [InlineData("aaa", 3)]
    [InlineData("aei", 3)]
    [InlineData("xazegov", 3)]
    [InlineData("aeiouaeiouaeiou", 15)]
    public void CountVowels_ShouldReturnAmountOfVowels(string input, int expected)
    {
        // Act
        var actual = Day05_DoesntHeHaveInternElvesForThis.CountVowels(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("aaa", 3)]
    [InlineData("aei", 1)]
    [InlineData("aabbccdd", 2)]
    [InlineData("aaaabbb", 4)]
    public void GetMaxLettersInARow_ShouldReturnMaximumAmountOfLettersInARow(string input, int expected)
    {
        // Act
        var actual = Day05_DoesntHeHaveInternElvesForThis.GetMaxLettersInARow(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("xyxy", true)]
    [InlineData("aaa", false)]
    public void ContainsPairTwice_ShouldReturnTrueWhenInputHasAPairTwice(string input, bool expected)
    {
        // Act
        var actual = Day05_DoesntHeHaveInternElvesForThis.ContainsPairTwice(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("xyx")]
    [InlineData("abcdefeghi")]
    [InlineData("aaa")]
    public void ContainsRepeatingLetterWithOneBetween_ShouldReturnTrueWhenInputHasRepeatingLetterWithOneBetween(string input)
    {
        // Act
        var actual = Day05_DoesntHeHaveInternElvesForThis.ContainsRepeatingLetterWithOneBetween(input);

        // Assert
        Assert.True(actual);
    }
}
