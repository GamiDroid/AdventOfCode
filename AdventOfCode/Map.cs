
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

    public Map(T[][] map)
    {
        _map = new T[map[0].Length, map.Length];
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                _map[x, y] = map[y][x];
            }
        }
        _lengthX = _map.GetLength(0);
        _lengthY = _map.GetLength(1);
    }

    public int LengthX => _lengthX;
    public int LengthY => _lengthY;
    public int MaxX => _lengthX-1;
    public int MaxY => _lengthY-1;

    public T this[int x, int y] { get => _map[x, y]; set => _map[x, y] = value; }
    public T this[Location location] { get => _map[location.X, location.Y]; set => _map[location.X, location.Y] = value; }

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

    public static IEnumerable<MapLocation<T>> Enumerate<T>(this Map<T> map) where T : struct
    {
        for (var x = 0; x < map.LengthX; x++)
        {
            for (var y = 0; y < map.LengthY; y++)
            {
                var location = new Location(x, y);
                yield return new MapLocation<T>(map[location], location);
            }
        }
    }

    public static IEnumerable<T> EnumerateValues<T>(this Map<T> map) where T : struct
    {
        for (var x = 0; x < map.LengthX; x++)
        {
            for (var y = 0; y < map.LengthY; y++)
            {
                yield return map[x, y];
            }
        }
    }
}

internal record struct MapLocation<T>(T Value, Location Location) where T : struct;
