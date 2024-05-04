namespace AdventOfCode._2016;
[Challenge(2016, 2, "Bathroom Security")]
internal class Day02_BathroomSecurity
{
    private readonly string[] _lines;

    private static readonly int[,] s_numberGrid = new[,]
    {
        { 1, 2, 3 },
        { 4, 5, 6 },
        { 7, 8, 9 },
    };

    private static readonly char[,] s_codeGrid = new[,]
    {
        { 'X', 'X', '1', 'X', 'X' },
        { 'X', '2', '3', '4', 'X' },
        { '5', '6', '7', '8', '9' },
        { 'X', 'A', 'B', 'C', 'X' },
        { 'X', 'X', 'D', 'X', 'X' },
    };

    public Day02_BathroomSecurity()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var lines = File.ReadAllLines(filePath);

        _lines = lines;
    }

    [Part(1)]
    public void Part01()
    {
        int x = 1, y = 1;
        var code = new int[_lines.Length];
        for (var i = 0; i < _lines.Length; i++)
        {
            var line = _lines[i].AsSpan();
            foreach (var c in line)
                (x, y) = Move(x, y, c);
            code[i] = s_numberGrid[x, y];
        }

        Console.WriteLine("the code for the bathroom is {0}", string.Join("", code));
    }

    private static (int x, int y) Move(int x, int y, char direction)
    {
        return direction switch
        {
            'U' => (Math.Max(x - 1, 0), y),
            'D' => (Math.Min(x + 1, 2), y),
            'L' => (x, Math.Max(y - 1, 0)),
            'R' => (x, Math.Min(y + 1, 2)),
            _ => throw new ArgumentException("Unknown direction", nameof(direction))
        };
    }

    [Part(2)]
    public void Part02()
    {
        int x = 2, y = 0;
        var code = new char[_lines.Length];
        for (var i = 0; i < _lines.Length; i++)
        {
            var line = _lines[i].AsSpan();
            foreach (var c in line)
                (x, y) = MoveOverCodeGrid(x, y, c);
            code[i] = s_codeGrid[x, y];
        }

        Console.WriteLine("the code for the bathroom is {0}", string.Join("", code));
    }

    private static (int x, int y) MoveOverCodeGrid(int x, int y, char direction)
    {
        (int newX, int newY) = direction switch
        {
            'U' => (Math.Max(x - 1, 0), y),
            'D' => (Math.Min(x + 1, 4), y),
            'L' => (x, Math.Max(y - 1, 0)),
            'R' => (x, Math.Min(y + 1, 4)),
            _ => throw new ArgumentException("Unknown direction", nameof(direction))
        };

        return s_codeGrid[newX, newY] == 'X' ? (x, y) : (newX, newY);
    }
}
