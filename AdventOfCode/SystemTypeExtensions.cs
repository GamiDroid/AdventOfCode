namespace AdventOfCode;
internal static class SystemTypeExtensions
{
    internal static IEnumerable<(T Item, int X, int Y)> ToEnumerable<T>(this T[,] map)
    {
        var yLength = map.GetLength(1);
        var xLength = map.GetLength(0);

        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLength; x++)
            {
                yield return (map[x, y], x, y);
            }
        }
    }
}
