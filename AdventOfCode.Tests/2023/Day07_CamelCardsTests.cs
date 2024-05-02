using AdventOfCode._2023;

namespace AdventOfCode.Tests._2023;
public class Day07_CamelCardsTests
{
    [Theory]
    [InlineData("AAAAA", 7)]
    [InlineData("AA8AA", 6)]
    [InlineData("23332", 5)]
    [InlineData("TTT98", 4)]
    [InlineData("23432", 3)]
    [InlineData("A23A4", 2)]
    [InlineData("23456", 1)]
    public void Should_Get_Hand_Strength(string hand, int expected)
    {
        var actual = Day07_CamelCards.HandComparer.GetHandTypeStrength(hand);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("33332", "2AAAA")]
    [InlineData("77888", "77788")]
    public void Should_Return_Hand1_Less_Than_Zero(string hand1, string hand2)
    {
        var actual = Day07_CamelCards.HandComparer.CompareHandsByLabels(hand1, hand2);
        Assert.True(actual < 0);
    }

    [Fact]
    public void Should_Result_In_Order_Highest_To_Lowest()
    {
        string[] hands = ["32T3K", "T55J5", "KK677", "KTJJT", "QQQJA"];
        var actual = hands.Order(new Day07_CamelCards.HandComparer()).ToArray();

        string[] expected = ["QQQJA", "T55J5", "KK677", "KTJJT", "32T3K"];

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Should_Return_Total_Score()
    {
        Day07_CamelCards.HandBid[] handbids = [
            new("32T3K" , 765),
            new("T55J5" , 684),
            new("KK677" , 28),
            new("KTJJT" , 220),
            new("QQQJA" , 483),
        ];

        var actual = Day07_CamelCards.GetTotalScore(handbids);
        var expected = 6440;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Should_Result_In_Order_Highest_To_Lowest_Puzzle_Two()
    {
        string[] hands = ["32T3K", "T55J5", "KK677", "KTJJT", "QQQJA"];
        var actual = hands.Order(new Day07_CamelCards.HandComparerTwo()).ToArray();

        string[] expected = ["KTJJT", "QQQJA", "T55J5", "KK677", "32T3K"];

        Assert.Equal(expected, actual);
    }
}
