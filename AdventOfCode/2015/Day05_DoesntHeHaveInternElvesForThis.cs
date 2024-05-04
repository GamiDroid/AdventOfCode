namespace AdventOfCode._2015;

[Challenge(2015, 5)]
internal class Day05_DoesntHeHaveInternElvesForThis
{
    private readonly string[] _testData;

    public Day05_DoesntHeHaveInternElvesForThis()
    {
        _testData = GetTestData();
    }

    [Part(1)]
    public void ExecutePart01()
    {
        var lines = _testData;

        var counter = 0;
        foreach (var line in lines)
        {
            if (IsNiceString(line))
                counter++;
        }

        Console.WriteLine("Amount of nice strings: {0}", counter);
    }

    [Part(2)]
    public void ExecutePart02()
    {
        var lines = _testData;

        var counter = 0;
        foreach (var line in lines)
        {
            if (IsBetterNiceString(line))
                counter++;
        }

        Console.WriteLine("Amount of nice strings: {0}", counter);
    }

    private static string[] GetTestData()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var lines = File.ReadAllLines(filePath);

        return lines;
    }

    public static bool IsNiceString(string input)
    {
        if (ContainsDisallowedString(input))
            return false;

        var countVowels = CountVowels(input);
        var maxLettersInARow = GetMaxLettersInARow(input);

        return (countVowels >= 3 && maxLettersInARow >= 2);
    }

    public static bool IsBetterNiceString(string input)
    {
        bool isNiceString = ContainsPairTwice(input);
        if (!isNiceString)
            return false;

        isNiceString = ContainsRepeatingLetterWithOneBetween(input);
        if (!isNiceString)
            return false;

        return true;
    }

    public static int CountVowels(ReadOnlySpan<char> input)
    {
        int counter = 0;
        foreach (char c in input)
        {
            if (s_vowels.Contains(c))
                counter++;
        }
        return counter;
    }

    public static int GetMaxLettersInARow(ReadOnlySpan<char> input)
    {
        char? prevChar = null;
        int maxCounter = 0;
        int counter = 0;

        foreach (char c in input)
        {
            if (prevChar != c)
            {
                if (counter > maxCounter)
                    maxCounter = counter;
                counter = 1;
            }
            else
            {
                counter++;
            }

            prevChar = c;
        }

        if (counter > maxCounter)
            maxCounter = counter;

        return maxCounter;
    }

    public static bool ContainsDisallowedString(string input)
    {
        foreach (var part in s_naughtyStrings)
        {
            if (input.Contains(part))
                return true;
        }
        return false;
    }

    public static bool ContainsPairTwice(ReadOnlySpan<char> input)
    {
        for (int i = 0; i < input.Length - 2; i++)
        {
            var pair = input.Slice(i, 2);
            var rest = input[(i + 2)..];

            if (rest.Contains(pair, StringComparison.InvariantCultureIgnoreCase))
                return true;
        }

        return false;
    }

    public static bool ContainsRepeatingLetterWithOneBetween(ReadOnlySpan<char> input)
    {
        for (int i = 0; i < input.Length - 2; i++)
        {
            var c0 = input[i];
            var c1 = input[i + 2];

            if (c0 == c1)
                return true;
        }

        return false;
    }

    private static readonly char[] s_vowels = new char[] { 'a', 'e', 'i', 'o', 'u' };

    private static readonly string[] s_naughtyStrings = new string[] { "ab", "cd", "pq", "xy" };
}
