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

    [Part(1)]
    public void Part01()
    {
        var sum = 0;
        foreach (var pattern in _patterns)
        {
            var verticalMirror = FindVerticalReflection(pattern);
            sum += verticalMirror;
            
            var horizontalMirror = FindHorizontalReflection(pattern);
            sum += horizontalMirror * 100;
        }

        Console.WriteLine($"Sum is {sum}");
    }

    private static int FindVerticalReflection(char[][] pattern)
    {
        for (int col1 = 0; col1 < pattern[0].Length; col1++)
        {
            for (int col2 = col1 + 1;  col2 < pattern[0].Length; col2++)
            {
                if (ColumnEqual(pattern, col1, col2))
                {
                    var distance = col2 - col1;
                    var distanceToMirror = distance / 2;

                    if (IsMirroredVertically(pattern, col1 + distanceToMirror, col2 - distanceToMirror))
                    {
                        return col1 + distanceToMirror + 1;
                    }
                }
            }
        }

        return 0;
    }

    private static int FindHorizontalReflection(char[][] pattern)
    {
        for (int row1 = 0; row1 < pattern.Length; row1++)
        {
            for (int row2 = row1 + 1; row2 < pattern.Length; row2++)
            {
                if (RowEqual(pattern, row1, row2))
                {
                    var distance = row2 - row1;
                    var distanceToMirror = distance / 2;

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
