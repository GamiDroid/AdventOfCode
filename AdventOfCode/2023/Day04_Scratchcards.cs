namespace AdventOfCode._2023;

using System.Text.RegularExpressions;

[Challenge(2023, 4, "Scratchcards")]
public class Day04_Scratchcards
{
    readonly ICollection<Scratchcard> _scratchcards = new List<Scratchcard>();

    [Setup]
    public void Setup()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var match = Regex.Match(line, "Card\\s+(\\d+):(.+)\\|(.+)");
            var scratchcard = new Scratchcard(
                CardNumber: match.Groups[1].ToInt32(),
                WinningNumbers: match.Groups[2].Value
                    .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.Parse(x)).ToArray(),
                Numbers: match.Groups[3].Value
                    .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.Parse(x)).ToArray());

            _scratchcards.Add(scratchcard);
        }
    }

    [Part(1)]
    public void Part01()
    {
        var sum = 0;
        foreach (var scratchcard in _scratchcards)
        {
            var winningNumbersCount = scratchcard.WinningNumbers
                .Intersect(scratchcard.Numbers)
                .Count();

            if (winningNumbersCount > 0)
                sum += 1 << (winningNumbersCount-1);
        }

        Console.WriteLine("Points in total {0}", sum);
    }

    [Part(2)]
    public void Part02()
    {
        var amountOfCards = CountCards(_scratchcards);
        Console.WriteLine("Amount of cards: {0}", amountOfCards);
    }

    public static int CountCards(ICollection<Scratchcard> cards)
    {
        var dict = new Dictionary<int, int>();

        var maxCardNumber = cards.Max(x => x.CardNumber);

        foreach (var scratchcard in cards)
        {
            var cardNumber = scratchcard.CardNumber;
            if (!dict.ContainsKey(cardNumber))
                dict[cardNumber] = 1;

            var winningNumbersCount = CountWinningNumbers(scratchcard);
            var amountCurrentCardNumber = dict[cardNumber];

            for (int i = cardNumber + 1; i <= cardNumber + winningNumbersCount; i++)
            {
                if (i > maxCardNumber)
                    break;

                if (!dict.ContainsKey(i))
                    dict[i] = 1;
                dict[i] += amountCurrentCardNumber;
            }
        }

        var amountOfCards = dict.Values.Aggregate((acc, val) => acc + val);

        return amountOfCards;
    }

    public static int CountWinningNumbers(Scratchcard scratchcard)
    {
        var winningNumbersCount = scratchcard.WinningNumbers
            .Intersect(scratchcard.Numbers)
            .Count();
        return winningNumbersCount;
    }

    public record Scratchcard(int CardNumber, int[] WinningNumbers, int[] Numbers);
}
