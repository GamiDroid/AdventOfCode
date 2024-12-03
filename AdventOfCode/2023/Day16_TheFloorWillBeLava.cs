namespace AdventOfCode._2023;

[Challenge(2023, 16, "The Floor Will Be Lava")]
internal class Day16_TheFloorWillBeLava
{
    private readonly Map _map;

    public Day16_TheFloorWillBeLava()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();
        _map = new Map(lines);
    }

    [Part(1)]
    public void Part01()
    {
        var visited = FindEnergizedTiles(new BeamState
        {
            Location = new(0, 0),
            Direction = 'E'
        });

        // PrintVisited(visited);

        Console.WriteLine($"Part 1: {visited.Length}");
    }

    [Part(2)]
    public void Part02()
    {
        List<BeamState> options = [];

        options.AddRange(Enumerable.Range(0, _map.LengthX - 1).Select(x => new BeamState(new(x, 0), 'Z')));
        options.AddRange(Enumerable.Range(0, _map.LengthY - 1).Select(y => new BeamState(new(0, y), 'E')));

        options.AddRange(Enumerable.Range(0, _map.LengthX - 1).Select(x => new BeamState(new(x, _map.LengthY - 1), 'N')));
        options.AddRange(Enumerable.Range(0, _map.LengthY - 1).Select(y => new BeamState(new(_map.LengthX - 1, y), 'W')));

        var maxVisited = options.Select(b => FindEnergizedTiles(b).Length).Max();

        Console.WriteLine($"Part 2: {maxVisited}");
    }

    private Location[] FindEnergizedTiles(BeamState start)
    {
        HashSet<(Location, char)> visitedWithDirection = [];
        HashSet<Location> visited = [];

        var beamsMarkedForRemoval = new List<BeamState>();
        var beamsToAdd = new List<BeamState>();

        List<BeamState> beams = [];
        beams.Add(start);

        do
        {
            foreach (var beam in beams)
            {
                if (visitedWithDirection.Contains((beam.Location, beam.Direction)))
                {
                    beamsMarkedForRemoval.Add(beam);
                    continue;
                }
                visitedWithDirection.Add((beam.Location, beam.Direction));
                visited.Add(beam.Location);
                var currTile = _map[beam.Location];
                if (currTile == '.')
                {
                    var location = Move(beam.Location, beam.Direction);
                    if (_map.IsValidLocation(location))
                    {
                        beam.Location = location;
                    }
                    else
                    {
                        beamsMarkedForRemoval.Add(beam);
                    }
                }
                else if (currTile is '|')
                {
                    if (beam.Direction is 'E' or 'e' or 'W' or 'w')
                    {
                        var beam2 = new BeamState { Location = beam.Location, Direction = 'N' };
                        var location2 = Move(beam2.Location, beam2.Direction);

                        if (_map.IsValidLocation(location2))
                        {
                            beam2.Location = location2;
                            beamsToAdd.Add(beam2);
                        }

                        beam.Direction = 'Z';
                        var location = Move(beam.Location, beam.Direction);

                        if (_map.IsValidLocation(location))
                        {
                            beam.Location = location;
                        }
                        else
                        {
                            beamsMarkedForRemoval.Add(beam);
                        }
                    }
                    else
                    {
                        var location = Move(beam.Location, beam.Direction);
                        if (_map.IsValidLocation(location))
                        {
                            beam.Location = location;
                        }
                        else
                        {
                            beamsMarkedForRemoval.Add(beam);
                        }
                    }
                }
                else if (currTile is '-')
                {
                    if (beam.Direction is 'N' or 'n' or 'Z' or 'z')
                    {
                        var beam2 = new BeamState { Location = beam.Location, Direction = 'W' };
                        var location2 = Move(beam2.Location, beam2.Direction);

                        if (_map.IsValidLocation(location2))
                        {
                            beam2.Location = location2;
                            beamsToAdd.Add(beam2);
                        }

                        beam.Direction = 'E';
                        var location = Move(beam.Location, beam.Direction);

                        if (_map.IsValidLocation(location))
                        {
                            beam.Location = location;
                        }
                        else
                        {
                            beamsMarkedForRemoval.Add(beam);
                        }
                    }
                    else
                    {
                        var location = Move(beam.Location, beam.Direction);
                        if (_map.IsValidLocation(location))
                        {
                            beam.Location = location;
                        }
                        else
                        {
                            beamsMarkedForRemoval.Add(beam);
                        }
                    }
                }
                else if (currTile is '\\')
                {
                    var newDirection = beam.Direction switch
                    {
                        'N' or 'n' => 'W',
                        'E' or 'e' => 'Z',
                        'Z' or 'z' => 'E',
                        'W' or 'w' => 'N',
                        _ => throw new InvalidDataException()
                    };

                    beam.Direction = newDirection;
                    var location = Move(beam.Location, beam.Direction);

                    if (_map.IsValidLocation(location))
                    {
                        beam.Location = location;
                    }
                    else
                    {
                        beamsMarkedForRemoval.Add(beam);
                    }
                }
                else if (currTile is '/')
                {
                    var newDirection = beam.Direction switch
                    {
                        'N' or 'n' => 'E',
                        'E' or 'e' => 'N',
                        'Z' or 'z' => 'W',
                        'W' or 'w' => 'Z',
                        _ => throw new InvalidDataException()
                    };

                    beam.Direction = newDirection;
                    var location = Move(beam.Location, beam.Direction);

                    if (_map.IsValidLocation(location))
                    {
                        beam.Location = location;
                    }
                    else
                    {
                        beamsMarkedForRemoval.Add(beam);
                    }
                }
                else
                {
                    Console.WriteLine($"Unknown tile '{currTile}' found.");
                }
            }

            beamsMarkedForRemoval.ForEach(b => beams.Remove(b));
            beamsMarkedForRemoval.Clear();

            beams.AddRange(beamsToAdd);
            beamsToAdd.Clear();
        }
        while (beams.Count > 0);

        return [.. visited];
    }

    void PrintVisited(Location[] visited)
    {
        for (int y = 0; y < _map.LengthY; y++)
        {
            for (int x = 0; x < _map.LengthX; x++)
            {
                Console.Write(visited.Contains(new Location { X = x, Y = y }) ? "#" : ".");
            }
            Console.WriteLine();
        }
    }

    private static Location Move(Location location, char direction)
    {
        return direction switch
        {
            'E' or 'e' => new(location.X + 1, location.Y),
            'W' or 'w' => new(location.X - 1, location.Y),
            'N' or 'n' => new(location.X, location.Y - 1),
            'Z' or 'z' => new(location.X, location.Y + 1),
            _ => throw new ArgumentException($"Direction '{direction}' is not valid", nameof(direction))
        };
    }

    class BeamState
    {
        public BeamState() { }

        public BeamState(Location location, char direction)
        {
            Location = location;
            Direction = direction;
        }

        public char Direction { get; set; }
        public Location Location { get; set; }
    }
}
