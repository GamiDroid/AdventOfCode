namespace AdventOfCode._2023;

[Challenge(2023, 13, "Point of Incidence")]
internal class Day13_PointofIncidence
{
    private readonly char[][][] _patterns;

    public Day13_PointofIncidence()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();

        List<char[][]> tempPatterns = [];
        List<char[]> pattern = [];

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                // add pattern to total list 
                tempPatterns.Add([.. pattern]);

                // create new instance
                pattern = [];

                // continue to next line
                continue;
            }

            pattern.Add(line.ToCharArray());
        }

        if (pattern.Count > 0)
            tempPatterns.Add([.. pattern]);

        _patterns = [.. tempPatterns];
    }

    private (int Left, int Top) StartCursor;
    private (int Left, int Top) EndCursor;

    [Part(1)]
    public void Part01()
    {
        var sum = 0;
        int patternCount = 1;
        foreach (var pattern in _patterns)
        {
            // pattern[x] is row x
            // pattern[x][y] is cell column x, row y

            StartCursor = Console.GetCursorPosition();
            PrintPattern(pattern);
            EndCursor = Console.GetCursorPosition();

            var verticalMirror = FindVerticalReflection(pattern);
            sum += verticalMirror;
            
            var horizontalMirror = FindHorizontalReflection(pattern);
            sum += horizontalMirror * 100;

            Console.WriteLine($"v: {verticalMirror} h: {horizontalMirror}");

            if (verticalMirror == 0 &&  horizontalMirror == 0)
                Console.WriteLine($"{patternCount} does not have a mirror");
            patternCount++;
        }

        Console.WriteLine($"Sum is {sum}");
    }

    private void PrintPattern(char[][] pattern)
    {
        foreach (var line in pattern)
        {
            Console.WriteLine(line);
        }
    }

    private int FindVerticalReflection(char[][] pattern)
    {
        int start = 0;
        if (pattern[0].Length % 2 != 0)
            start = 1;

        for (int col1 = start; col1 < pattern[0].Length; col1++)
        {
            HighlightColumn(pattern, col1);
            for (int col2 = col1 + 1;  col2 < pattern[0].Length; col2++)
            {
                HighlightColumn(pattern, col2);

                if (ColumnEqual(pattern, col1, col2))
                {
                    var distance = col2 - col1;
                    var distanceToMirror = distance / 2;

                    Console.WriteLine($"found same col at {col1} and {col2}");


                    Console.WriteLine($"Check vertical mirror at rows {col1 + distanceToMirror} {col2 - distanceToMirror}");

                    if (IsMirroredVertically(pattern, col1 + distanceToMirror, col2 - distanceToMirror))
                    {
                        return col1 + distanceToMirror + 1;
                    }
                }

                ResetColumn(pattern, col2);

                Thread.Sleep(1000);
            }

            ResetColumn(pattern, col1);
        }

        return 0;
    }

    private void HighlightColumn(char[][] pattern, int column)
    {
        Console.BackgroundColor = ConsoleColor.Cyan;
        Console.ForegroundColor = ConsoleColor.Black;

        ReprintColumn(pattern, column);
        Console.ResetColor();
    }

    private void ResetColumn(char[][] pattern, int column)
    {
        Console.ResetColor();
        ReprintColumn(pattern, column);
    }

    private void ReprintColumn(char[][] pattern, int column)
    {
        for (int row = 0; row < pattern.Length; row++)
        {
            Console.CursorLeft = StartCursor.Left + column;
            Console.CursorTop = StartCursor.Top + row - 1;

            Console.Write(pattern[row][column]);
        }
    }

    private int FindHorizontalReflection(char[][] pattern)
    {
        int start = 0;
        if (pattern.Length % 2 != 0)
            start = 1;

        for (int row1 = start; row1 < pattern.Length; row1++)
        {
            for (int row2 = row1 + 1; row2 < pattern.Length; row2++)
            {
                if (RowEqual(pattern, row1, row2))
                {
                    var distance = row2 - row1;
                    var distanceToMirror = distance / 2;

                    Console.WriteLine($"found same row at {row1} and {row2}");

                    Console.WriteLine($"Check horizontal mirror at rows {row1 + distanceToMirror} {row2 - distanceToMirror}");

                    if (IsMirroredHorizontally(pattern, row1 + distanceToMirror, row2 - distanceToMirror))
                    {
                        return row1 + distanceToMirror + 1;
                    }
                }
            }
        }

        return 0;
    }

    private static bool IsMirroredVertically(char[][] pattern, int col1, int col2)
    {
        do
        {
            if (!ColumnEqual(pattern, col1, col2))
            {
                Console.WriteLine($"not equal between column {col1} and {col2}");
                return false;
            }
            col1--;
            col2++;
        }
        while (col1 >= 0 && col2 < pattern[0].Length); 

        return true;
    }

    private static bool IsMirroredHorizontally(char[][] pattern, int row1, int row2)
    {
        do
        {
            if (!RowEqual(pattern, row1, row2))
            {
                Console.WriteLine($"not equal between row {row1} and {row2}");
                return false;
            }
            row1--;
            row2++;
        }
        while (row1 >= 0 && row2 < pattern.Length);

        return true;
    }

    private static bool RowEqual(char[][] pattern, int row1, int row2)
    {
        return pattern[row1].SequenceEqual(pattern[row2]);
    }

    private static bool ColumnEqual(char[][] pattern, int column1, int column2)
    {
        for (int row = 0; row < pattern.Length; row++)
        {
            var c1 = pattern[row][column1];
            var c2 = pattern[row][column2];
            if (c1 != c2)
                return false;
        }
        return true;
    }
}
