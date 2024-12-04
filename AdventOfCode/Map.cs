namespace AdventOfCode;
internal class Map<T> where T : struct
{
    private readonly T[,] _map;
    private readonly int _lengthX;
    private readonly int _lengthY;

    public Map(T[,] map)
    {
        _map = map;
        _lengthX = _map.GetLength(0);
        _lengthY = _map.GetLength(1);
    }

    //public Map(T[][] map)
    //{
    //    var tempMap = new();
    //}

    public int LengthX => _lengthX;
    public int LengthY => _lengthY;

    public T this[int x, int y] => _map[x, y];
    public T this[Location location] => _map[location.X, location.Y];

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

internal static class Map
{
    public static Map<char> Create(string[] strings)
    {
        var map = new char[strings[0].Length, strings.Length];

        for (int y = 0; y < strings.Length; y++)
        {
            for (int x = 0; x < strings[y].Length; x++)
            {
                map[x, y] = strings[y][x];
            }
        }

        return new Map<char>(map);
    }
}
