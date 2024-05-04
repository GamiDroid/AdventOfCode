
using System;
using System.Text.RegularExpressions;

namespace AdventOfCode._2023;

[Challenge(2023, 7, "Camel Cards")]
internal class Day07_CamelCards
{
    // you get a list of hands
    // goal: order them based on strength for each hand
    // a hand contains 5 cards
    // with labels: A, K, Q, J, T, 9, 8, 7, 6, 5, 4, 3, or 2
    // where A highest and 2 lowest
    // every hand is one type:
    // Five of a kind : AAAAA
    // Four of a kind : AA8AA
    // Full house : 23332
    // Three of a kind : TTT98
    // Two pair : 23432
    // One pair : A23A4
    // High card : 23456
    // Hands are primarily bases on type.
    // secondarily by label, on each card, starting at the first
    // puzzle input {hand} {bid} : 32T3K 765
    // each hand wins {hand rank} * {bid}. starting with 1
    // Result : total winnings

    private readonly List<HandBid>? _handBids;

    public Day07_CamelCards()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var fileContent = File.ReadAllText(filePath);

        _handBids = Regex.Matches(fileContent, "(.{5}) (\\d+)")
            .Select(m => new HandBid(m.Groups[1].Value, m.Groups[2].ToInt32()))
            .ToList();
    }

    [Part(1)]
    public void Part01()
    {
        var totalScore = GetTotalScore(_handBids!);

        Console.WriteLine("Total score is {0}", totalScore);
    }

    [Part(2)]
    public void Part02()
    {
        var totalScore = GetTotalScoreTwo(_handBids!);

        Console.WriteLine("Total score is {0}", totalScore);
    }

    public static int GetTotalScore(ICollection<HandBid> handBids)
    {
        return handBids
            .OrderByDescending(h => h.Hand, new HandComparer())
            .Select((h, i) => h.Bid * (i+1))
            .Sum();
    }

    public static int GetTotalScoreTwo(ICollection<HandBid> handBids)
    {
        return handBids
            .OrderByDescending(h => h.Hand, new HandComparerTwo())
            .Select((h, i) => h.Bid * (i + 1))
            .Sum();
    }

    public record struct HandBid(string Hand, int Bid);

    public class HandComparer : IComparer<string>
    {
        private const string _labels = "AKQJT98765432";

        public int Compare(string? x, string? y)
        {
            var sx = GetHandTypeStrength(x);
            var sy = GetHandTypeStrength(y);

            if (sx > sy)
            {
                return -1;
            }
            else if (sx < sy)
            {
                return 1;
            }

            return CompareHandsByLabels(x, y);
        }

        public static int GetHandTypeStrength(ReadOnlySpan<char> hand)
        {
            var labelCount = hand.ToArray()
                .GroupBy(c => c, (c, list) => new { Label = c, Count = list.Count() })
                .OrderByDescending(x => x.Count)
                .ToArray();

            if (labelCount[0].Count == 5)
            {
                return 7;
            }
            else if (labelCount[0].Count == 4)
            {
                return 6;
            }
            else if (labelCount[0].Count == 3 && labelCount[1].Count == 2)
            {
                return 5;
            }
            else if (labelCount[0].Count == 3)
            {
                return 4;
            }
            else if (labelCount[0].Count == 2 && labelCount[1].Count == 2)
            {
                return 3;
            }
            else if (labelCount[0].Count == 2)
            {
                return 2;
            }

            return 1;
        }

        public static int CompareHandsByLabels(ReadOnlySpan<char> lhs, ReadOnlySpan<char> rhs)
        {
            for (var i = 0; i < 5; i++)
            {
                if (lhs[i] == rhs[i])
                {
                    continue;
                }

                var li = _labels.IndexOf(lhs[i]);
                var ri = _labels.IndexOf(rhs[i]);

                return (li < ri) ? -1 : 1;
            }

            return 0;
        }
    }

    public class HandComparerTwo : IComparer<string>
    {
        private const string _labels = "AKQT98765432J";

        public int Compare(string? x, string? y)
        {
            if (x == y) return 0;

            var sx = GetHandTypeStrength(x);
            var sy = GetHandTypeStrength(y);

            if (sx > sy)
            {
                return -1;
            }
            else if (sx < sy)
            {
                return 1;
            }

            return CompareHandsByLabels(x, y);
        }

        public static int GetHandTypeStrength(ReadOnlySpan<char> hand)
        {
            var labelCount = hand.ToArray()
                .Where(c => c != 'J')
                .GroupBy(c => c, (c, list) => new { Label = c, Count = list.Count() })
                .OrderByDescending(x => x.Count)
                .ToArray();

            var jokerCount = hand.Count('J');

            var firstHighest = jokerCount;
            if (labelCount.Length > 0)
                firstHighest = labelCount[0].Count + jokerCount;

            if (firstHighest == 5)
            {
                return 7;
            }
            else if (firstHighest == 4)
            {
                return 6;
            }
            else if (firstHighest == 3 && labelCount[1].Count == 2)
            {
                return 5;
            }
            else if (firstHighest == 3)
            {
                return 4;
            }
            else if (firstHighest == 2 && labelCount[1].Count == 2)
            {
                return 3;
            }
            else if (firstHighest == 2)
            {
                return 2;
            }

            return 1;
        }

        public static int CompareHandsByLabels(ReadOnlySpan<char> lhs, ReadOnlySpan<char> rhs)
        {
            for (var i = 0; i < 5; i++)
            {
                if (lhs[i] == rhs[i])
                {
                    continue;
                }

                var li = _labels.IndexOf(lhs[i]);
                var ri = _labels.IndexOf(rhs[i]);

                return (li < ri) ? -1 : 1;
            }

            return 0;
        }
    }

    public enum HandScore
    {
        None,
        HighCard = 1,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind,
    }
}
