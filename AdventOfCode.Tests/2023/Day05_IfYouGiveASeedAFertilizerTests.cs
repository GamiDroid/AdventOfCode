using AdventOfCode._2023;
using Converter = AdventOfCode._2023.Day05_IfYouGiveASeedAFertilizer.Converter;

namespace AdventOfCode.Tests._2023;
public class Day05_IfYouGiveASeedAFertilizerTests
{
    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(48, false)]
    [InlineData(49, false)]
    [InlineData(98, true)]
    [InlineData(99, true)]
    [InlineData(100, false)]
    public void ConverterIsInRange_ShouldReturnWhetherTheGivenNumberIsInRange(ulong number, bool expected)
    {
        // arrange
        var converter = new Converter(50, 98, 2);

        // act
        var actual = converter.IsInRange(number);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(48, 48)]
    [InlineData(49, 49)]
    [InlineData(98, 50)]
    [InlineData(99, 51)]
    public void ConverterIsInRange_ShouldReturnTheDestinationOfTheGivenSource(ulong source, ulong expected)
    {
        // arrange
        var converter = new Converter(50, 98, 2);

        // act
        var actual = converter.GetDestination(source);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(48, 48)]
    [InlineData(49, 49)]
    [InlineData(50, 52)]
    [InlineData(51, 53)]
    [InlineData(96, 98)]
    [InlineData(97, 99)]
    [InlineData(98, 50)]
    [InlineData(99, 51)]
    public void GetDestination_ShouldReturnTheDestinationOfTheGivenSource(ulong source, ulong expected)
    {
        // arrange
        Converter[] converters = [new Converter(50, 98, 2), new Converter(52, 50, 48)];

        // act
        var actual = Day05_IfYouGiveASeedAFertilizer.GetDestination(source, converters);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("50 98 2", 50, 98, 2)]
    [InlineData("52 50 48", 52, 50, 48)]
    public void Parse_ShouldReturnAConverter(
        string input,
        ulong expectedDestinationStartRange,
        ulong expectedSourceStartRange,
        ulong expectedLengthRange)
    {
        // act
        var actual = Day05_IfYouGiveASeedAFertilizer.Parse(input);

        // assert
        Assert.Equal(expectedDestinationStartRange, actual.DestinationStartRange);
        Assert.Equal(expectedSourceStartRange, actual.SourceStartRange);
        Assert.Equal(expectedLengthRange, actual.LengthRange);
    }
}
