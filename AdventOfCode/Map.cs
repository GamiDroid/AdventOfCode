namespace AdventOfCode;
internal class Map
{
    private readonly char[,] _map;
    private readonly int _lengthX;
    private readonly int _lengthY;

    public Map(string[] strings)
    {
        var map = new char[strings[0].Length, strings.Length];

        for (int y = 0; y < strings.Length; y++)
        {
            for (int x = 0; x < strings[y].Length; x++)
            {
                map[x, y] = strings[y][x];
            }
        }

        _map = map;
        _lengthX = _map.GetLength(0);
        _lengthY = _map.GetLength(1);
    }

    public int LengthX => _lengthX;
    public int LengthY => _lengthY;

    public char this[int x, int y] => _map[x, y];
    public char this[Location location] => _map[location.X, location.Y];

    private bool IsValidLocation(int x, int y)
    {
        return x >= 0 &&
            x < LengthX &&
            y >= 0 &&
            y < LengthY;
    }

    public bool IsValidLocation(Location location)
    {
        return IsValidLocation(location.X, location.Y);
    }
}
