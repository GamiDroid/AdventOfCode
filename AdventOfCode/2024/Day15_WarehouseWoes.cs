namespace AdventOfCode._2024;

[Challenge(2024, 15, "Warehouse Woes")]
internal class Day15_WarehouseWoes
{
    private readonly Map<char> _map;
    private readonly char[] _movements;

    public Day15_WarehouseWoes()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();
        var index = lines
            .Select((l, i) => new { Line = l, Index = i })
            .Where(x => string.IsNullOrWhiteSpace(x.Line))
            .Select(x => x.Index)
            .First();
        
        _map = Map.Create(lines.Take(index).ToArray());

        List<char> movements = [];
        foreach (var line in lines.Skip(index+1))
        {
            movements.AddRange(line.ToCharArray());
        }

        _movements = [.. movements];
    }

    [Part(1)]
    public void Part01()
    {
        List<MapObject> mapObjects = [];
        MapObject robot = null!;
        foreach (var mapLocation in _map.Enumerate())
        {
            if (mapLocation.Value != '.')
            {
                MapObject mapObject = new(mapLocation.Value, mapLocation.Location);
                if (mapLocation.Value == '@')
                    robot = mapObject;
                mapObjects.Add(mapObject);
            }
        }

        foreach (var movement in _movements)
        {
            TryMove(_map, mapObjects, robot, movement);
        }

        var answer = mapObjects.Where(o => o.Type == 'O').Sum(o => 100 * o.Location.Y + o.Location.X);
        Console.WriteLine($"Part 1: {answer}");
    }

    private static Map<char> ExpandMap(Map<char> map)
    {
        var temp = new char[map.LengthX * 2, map.LengthY];
        foreach (var mapLocation in map.Enumerate())
        {
            var tile = mapLocation.Value;
            var location = mapLocation.Location;
            int newX = location.X * 2;

            var (tileL, tileR) = tile switch
            {
                '#' => ('#', '#'),
                'O' => ('[', ']'),
                '.' => ('.', '.'),
                '@' => ('@', '.'),
                _ => throw new ArgumentException("Invalid tile in map", nameof(map))
            };

            temp[newX, location.Y] = tileL;
            temp[newX + 1, location.Y] = tileR;
        }

        return new Map<char>(temp);
    }

    private static void PrintMap(Map<char> map)
    {
        for (int y = 0; y < map.LengthY; y++)
        {
            for (int x = 0; x < map.LengthX; x++)
            {
                Console.Write(map[x, y]);
            }
            Console.WriteLine();
        }
    }

    private static bool TryMove(Map<char> map, List<MapObject> allObjects, MapObject mapObject, char direction)
    {
        var nextLocation = GetNextLocation(mapObject.Location, direction);

        if (!map.IsValidLocation(nextLocation))
            return false;

        var neighborObject = allObjects.FirstOrDefault(o => o.Location == nextLocation);
        if (neighborObject is not null)
        {
            if (neighborObject.Type is '#' || !TryMove(map, allObjects, neighborObject, direction))
                return false;
        }

        mapObject.Location = nextLocation;
        return true;
    }

    private static Location GetNextLocation(Location location, char direction)
    {
        return direction switch
        {
            '^' => new Location(location.X, location.Y - 1),
            '>' => new Location(location.X + 1, location.Y),
            'v' => new Location(location.X, location.Y + 1),
            '<' => new Location(location.X - 1, location.Y),
            _ => throw new ArgumentException("Invalid value", nameof(direction))
        };
    }

    public class MapObject(char type, Location location)
    {
        public char Type { get; } = type;
        public Location Location { get; set; } = location;
    }
}
