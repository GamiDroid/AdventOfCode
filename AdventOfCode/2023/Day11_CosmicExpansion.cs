
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Data.Common;

namespace AdventOfCode._2023;

[Challenge(2023, 11, "Cosmic Expansion")]
internal class Day11_CosmicExpansion
{
    private readonly char[,] _cosmos;

    public Day11_CosmicExpansion()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();
        var lineLength = lines[0].Length;
        _cosmos = new char[lines[0].Length, lines.Length];

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lineLength; x++)
            {
                _cosmos[x, y] = lines[y][x];
            }
        }
    }

    [Part(1)]
    public void Part01()
    {
        var originalGalaxies = FindGalaxies(_cosmos);

        var cosmosColumnLength = _cosmos.GetLength(0);
        var columnsWithoutGalaxies = FindColumnsWithoutGalaxies(originalGalaxies, cosmosColumnLength);

        var cosmosRowLength = _cosmos.GetLength(1);
        var rowsWithoutGalaxies = FindRowsWithoutGalaxies(originalGalaxies, cosmosRowLength);

        HashSet<(int, int)> calculatedDistances = [];

        long sum = 0;
        for (int g1 = 0; g1 < originalGalaxies.Length; g1++)
        {
            for (int g2 = 0; g2 < originalGalaxies.Length; g2++)
            {
                if (g1 == g2)
                    continue;

                (int k1, int k2) = (g1, g2);
                if (k2 > k1)
                    (k1, k2) = (k2, k1);

                if (calculatedDistances.Contains((k1, k2)))
                    continue;

                var galaxy1 = originalGalaxies[g1];
                var galaxy2 = originalGalaxies[g2];

                var minX = int.Min(galaxy1.X, galaxy2.X);
                var maxX = int.Max(galaxy1.X, galaxy2.X);
                var amountOfColumnsWithoutGalaxiesBetweenGalaxies = columnsWithoutGalaxies.Count(x => minX < x && maxX > x);

                var minY = int.Min(galaxy1.Y, galaxy2.Y);
                var maxY = int.Max(galaxy1.Y, galaxy2.Y);
                var amountOfRowsWithoutGalaxiesBetweenGalaxies = rowsWithoutGalaxies.Count(y => minY < y && maxY > y);

                var distance = CalculateDistance(
                    galaxy1,
                    galaxy2,
                    amountOfColumnsWithoutGalaxiesBetweenGalaxies,
                    amountOfRowsWithoutGalaxiesBetweenGalaxies);

                sum += distance;

                calculatedDistances.Add((k1, k2));
            }
        }

        Console.WriteLine($"total sum is '{sum}'");
    }

    [Part(2)]
    public void Part02()
    {
        var originalGalaxies = FindGalaxies(_cosmos);

        var cosmosColumnLength = _cosmos.GetLength(0);
        var columnsWithoutGalaxies = FindColumnsWithoutGalaxies(originalGalaxies, cosmosColumnLength);

        var cosmosRowLength = _cosmos.GetLength(1);
        var rowsWithoutGalaxies = FindRowsWithoutGalaxies(originalGalaxies, cosmosRowLength);

        HashSet<(int, int)> calculatedDistances = [];

        long sum = 0;
        for (int g1 = 0; g1 < originalGalaxies.Length; g1++)
        {
            for (int g2 = 0; g2 < originalGalaxies.Length; g2++)
            {
                if (g1 == g2)
                    continue;

                (int k1, int k2) = (g1, g2);
                if (k2 > k1)
                    (k1, k2) = (k2, k1);

                if (calculatedDistances.Contains((k1, k2)))
                    continue;

                var galaxy1 = originalGalaxies[g1];
                var galaxy2 = originalGalaxies[g2];

                var minX = int.Min(galaxy1.X, galaxy2.X);
                var maxX = int.Max(galaxy1.X, galaxy2.X);
                var amountOfColumnsWithoutGalaxiesBetweenGalaxies = columnsWithoutGalaxies.Count(x => minX < x && maxX > x);

                var minY = int.Min(galaxy1.Y, galaxy2.Y);
                var maxY = int.Max(galaxy1.Y, galaxy2.Y);
                var amountOfRowsWithoutGalaxiesBetweenGalaxies = rowsWithoutGalaxies.Count(y => minY < y && maxY > y);

                var distance = CalculateDistance(
                    galaxy1, 
                    galaxy2, 
                    amountOfColumnsWithoutGalaxiesBetweenGalaxies, 
                    amountOfRowsWithoutGalaxiesBetweenGalaxies,
                    multiplier: 1_000_000);

                sum += distance;

                calculatedDistances.Add((k1, k2));
            }
        }

        Console.WriteLine($"total sum is '{sum}'");
    }

    private static long CalculateDistance(Point p1, Point p2, int emptyColumns, int emptyRows, int multiplier = 2)
    {
        var distanceX = Math.Abs(p2.X - p1.X) + (emptyColumns * multiplier) - emptyColumns;
        var distanceY = Math.Abs(p2.Y - p1.Y) + (emptyRows * multiplier) - emptyRows;

        return (distanceX + distanceY);
    }

    private static Point[] GetGalaxiesInExpandedCosmos(char[,] cosmos)
    {
        var galaxies = FindGalaxies(cosmos);

        var cosmosColumnLength = cosmos.GetLength(0);
        var columnsWithoutGalaxies = FindColumnsWithoutGalaxies(galaxies, cosmosColumnLength);

        var cosmosRowLength = cosmos.GetLength(1);
        var rowsWithoutGalaxies = FindRowsWithoutGalaxies(galaxies, cosmosRowLength);

        var newCosmosColumnSize = cosmosColumnLength + columnsWithoutGalaxies.Length;
        var newCosmosRowSize = cosmosRowLength + rowsWithoutGalaxies.Length;
        var newCosmos = new char[newCosmosColumnSize, newCosmosRowSize];

        int columnOffset = 0;
        foreach (var column in columnsWithoutGalaxies)
        {
            for (int i = 0; i < galaxies.Length; i++)
            {
                if (galaxies[i].X - columnOffset > column)
                {
                    galaxies[i] = new Point(galaxies[i].X + 1, galaxies[i].Y);
                }
            }

            columnOffset++;
        }

        int rowOffset = 0;
        foreach (var row in rowsWithoutGalaxies)
        {
            for (int i = 0; i < galaxies.Length; i++)
            {
                if (galaxies[i].Y - rowOffset > row)
                {
                    galaxies[i] = new Point(galaxies[i].X, galaxies[i].Y + 1);
                }
            }

            rowOffset++;
        }

        return galaxies;
    }

    private static int[] FindColumnsWithoutGalaxies(Point[] galaxies, int columnsLength)
    {
        var columnsWithoutGalaxies = Enumerable.Range(0, columnsLength)
            .Where(x => !galaxies.Select(g => g.X).Contains(x))
            .ToArray();
        return columnsWithoutGalaxies;
    }

    private static int[] FindRowsWithoutGalaxies(Point[] galaxies, int rowsLength)
    {
        var rowsWithoutGalaxies = Enumerable.Range(0, rowsLength)
            .Where(y => !galaxies.Select(g => g.Y).Contains(y))
            .ToArray();
        return rowsWithoutGalaxies;
    }

    private static Point[] FindGalaxies(char[,] cosmos)
    {
        var galaxies = cosmos.ToEnumerable()
            .Where(x => x.Item == '#')
            .Select(x => new Point(x.X, x.Y))
            .ToArray();
        return galaxies;
    }

    private record struct Point(int X, int Y);
}
