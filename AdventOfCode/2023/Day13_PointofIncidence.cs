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

    [Part(2)]
    public void Part02()
    {
        var sum = 0;
        foreach (var pattern in _patterns)
        {
            var horizontalMirror = FindHorizontalReflection(pattern, ignoreSmudge: true);
            sum += horizontalMirror * 100;
            var verticalMirror = FindVerticalReflection(pattern, ignoreSmudge: true);
            sum += verticalMirror;
        }

        Console.WriteLine($"Sum is {sum}");
    }

    private static int FindVerticalReflection(char[][] pattern, bool ignoreSmudge = false)
    {
        var colCount = pattern[0].Length;
        for (int col1 = 0; col1 < colCount; col1++)
        {
            for (int col2 = col1 + 1;  col2 < colCount; col2++)
            {
                bool useIgnoreSmudge = ignoreSmudge && ((col1 == 0 && col2 == 1) || (col1 == colCount-2 && col2 == colCount-1));

                if (ColumnEqual(pattern, col1, col2, ignoreSmudge: useIgnoreSmudge, out _) >= pattern.Length - (useIgnoreSmudge ? 1 : 0))
                {
                    var distance = col2 - col1;
                    var distanceToMirror = distance / 2;

                    if (IsMirroredVertically(pattern, col1 + distanceToMirror, col2 - distanceToMirror, ignoreSmudge, out bool hasSmudge))
                    {
                        if (!ignoreSmudge || hasSmudge)
                            return col1 + distanceToMirror + 1;
                    }
                }
            }
        }

        return 0;
    }

    private static int FindHorizontalReflection(char[][] pattern, bool ignoreSmudge = false)
    {
        int rowCount = pattern.Length;

        for (int row1 = 0; row1 < rowCount; row1++)
        {
            for (int row2 = row1 + 1; row2 < rowCount; row2++)
            {
                bool useIgnoreSmudge = ignoreSmudge && ((row1 == 0 && row2 == 1) || (row1 == rowCount - 2 && row2 == rowCount - 1));

                int colCount = pattern[0].Length;
                if (RowEqual(pattern, row1, row2, ignoreSmudge: useIgnoreSmudge, out _) >= colCount - (useIgnoreSmudge ? 1 : 0))
                {
                    var distance = row2 - row1;
                    var distanceToMirror = distance / 2;

                    if (IsMirroredHorizontally(pattern, row1 + distanceToMirror, row2 - distanceToMirror, ignoreSmudge, out bool hasSmudge))
                    {
                        if (!ignoreSmudge || hasSmudge)
                            return row1 + distanceToMirror + 1;                            
                    }
                }
            }
        }

        return 0;
    }

    private static bool IsMirroredVertically(char[][] pattern, int col1, int col2, bool ignoreSmudge, out bool hasSmudge)
    {
        int ignoresAllowed = ignoreSmudge ? 1 : 0;
        hasSmudge = false;

        do
        {
            int equalColumnCount = ColumnEqual(pattern, col1, col2, ignoreSmudge, out int smudgeRow);
            int missingCount = (pattern.Length - equalColumnCount);

            if (missingCount > 0)
            {
                hasSmudge = true;
            }

            if (missingCount > ignoresAllowed)
            {
                return false;
            }

            ignoresAllowed -= missingCount;
            col1--;
            col2++;
        }
        while (col1 >= 0 && col2 < pattern[0].Length);

        return true;
    }

    private static bool IsMirroredHorizontally(char[][] pattern, int row1, int row2, bool ignoreSmudge, out bool hasSmudge)
    {
        int ignoresAllowed = ignoreSmudge ? 1 : 0;
        hasSmudge = false;
        do
        {
            int equalRowCount = RowEqual(pattern, row1, row2, ignoreSmudge, out int smudgeCol);
            int missingCount = (pattern[0].Length - equalRowCount);

            if (missingCount > 0)
            {
                hasSmudge = true;
            }

            if (missingCount > ignoresAllowed)
            {
                return false;
            }

            ignoresAllowed -= missingCount;
            row1--;
            row2++;
        }
        while (row1 >= 0 && row2 < pattern.Length);

        return true;
    }

    private static int RowEqual(char[][] pattern, int row1, int row2, bool ignoreSmudge, out int smudge)
    {
        int ignoresAllowed = ignoreSmudge ? 1 : 0;
        int ignores = 0;
        smudge = -1;

        for (int col = 0; col < pattern[0].Length; col++)
        {
            var c1 = pattern[row1][col];
            var c2 = pattern[row2][col];
            if (c1 != c2)
            {
                if (ignoresAllowed == ignores)
                    return col - ignores;
                ignores++;
                smudge = col;
            }
        }
        return pattern[0].Length - ignores;
    }

    private static int ColumnEqual(char[][] pattern, int column1, int column2, bool ignoreSmudge, out int smudge)
    {
        int ignoresAllowed = ignoreSmudge ? 1 : 0;
        int ignores = 0;
        smudge = -1;

        for (int row = 0; row < pattern.Length; row++)
        {
            var c1 = pattern[row][column1];
            var c2 = pattern[row][column2];
            if (c1 != c2)
            {
                if (ignoresAllowed == ignores)
                    return row - ignores;
                ignores++;
                smudge = row;
            }
        }
        return pattern.Length - ignores;
    }
}
