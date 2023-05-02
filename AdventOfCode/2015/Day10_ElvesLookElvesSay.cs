using System.Text;

namespace AdventOfCode._2015;

[Challenge(2015, 10)]
internal class Day10_ElvesLookElvesSay
{
    [Part(1)]
    public void ExecutePart01()
    {
        var input = "3113322113";
        for (int i = 1; i <= 40; i++)
        {
            var result = LookAndSay(input);

            WriteAction(i, input, result);

            input = result;
        }
    }

    [Part(2)]
    public void ExecutePart02()
    {
        var input = "3113322113";
        for (int i = 1; i <= 50; i++)
        {
            var result = LookAndSay(input);

            WriteAction(i, input, result);

            input = result;
        }
    }

    private static void WriteAction(int index, string input, string result)
    {
        Console.Write($"{index}: ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{input} ({input.Length})");
        Console.ResetColor();
        Console.Write(" becomes ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{result} ({result.Length})");
        Console.ResetColor();
    }

    public static string LookAndSay(string text)
    {
        var sb = new StringBuilder();

        char? prevChar = null;
        int counter = 0;

        foreach (var c in text)
        {
            if (c != prevChar)
            {
                if (prevChar != null)
                {
                    sb.Append(counter);
                    sb.Append(prevChar);

                    counter = 0;
                }

                prevChar = c;
            }

            counter++;
        }

        if (counter > 0)
        {
            sb.Append(counter);
            sb.Append(prevChar);
        }

        return sb.ToString();
    }
}
