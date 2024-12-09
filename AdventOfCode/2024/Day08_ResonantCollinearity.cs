using System.Collections.Generic;

namespace AdventOfCode._2024;
[Challenge(2024, 8, "Resonant Collinearity")]
internal class Day08_ResonantCollinearity
{
    private readonly Map<char> _map;

    public Day08_ResonantCollinearity()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();
        _map = Map.Create(lines);
    }

    [Part(1)]
    public void Part01()
    {
        Dictionary<char, List<Location>> antenneLocations = [];
        foreach (var mapLocation in _map.Enumerate())
        {
            if (mapLocation.Value == '.')
            {
                continue;
            }

            if (!antenneLocations.TryGetValue(mapLocation.Value, out var locations))
            {
                locations = [];
                antenneLocations.Add(mapLocation.Value, locations);
            }
            locations.Add(mapLocation.Location);
        }

        HashSet<Location> antinodes = [];
        foreach (var frequencyLocation in antenneLocations)
        {
            for (int i = 0; i < frequencyLocation.Value.Count; i++)
            {
                for (var j = 0; j < frequencyLocation.Value.Count; j++)
                {
                    if (i == j)
                        continue;

                    var distX = frequencyLocation.Value[i].X - frequencyLocation.Value[j].X;
                    var distY = frequencyLocation.Value[i].Y - frequencyLocation.Value[j].Y;

                    var antinodeX = frequencyLocation.Value[i].X + distX;
                    var antinodeY = frequencyLocation.Value[i].Y + distY;

                    var antinode = new Location(antinodeX, antinodeY);
                    if (_map.IsValidLocation(antinode))
                        antinodes.Add(antinode);
                }
            }
        }

        var answer = antinodes.Count;
        Console.WriteLine($"Part 1: {answer}");
    }


    [Part(2)]
    public void Part02()
    {
        Dictionary<char, List<Location>> antenneLocations = [];
        foreach (var mapLocation in _map.Enumerate())
        {
            if (mapLocation.Value == '.')
            {
                continue;
            }

            if (!antenneLocations.TryGetValue(mapLocation.Value, out var locations))
            {
                locations = [];
                antenneLocations.Add(mapLocation.Value, locations);
            }
            locations.Add(mapLocation.Location);
        }



        HashSet<Location> antinodes = [];
        foreach (var antenneLocation in antenneLocations.SelectMany(x => x.Value))
            antinodes.Add(antenneLocation);

        foreach (var frequencyLocation in antenneLocations)
        {
            for (int i = 0; i < frequencyLocation.Value.Count; i++)
            {
                for (var j = 0; j < frequencyLocation.Value.Count; j++)
                {
                    if (i == j)
                        continue;

                    var distX = frequencyLocation.Value[i].X - frequencyLocation.Value[j].X;
                    var distY = frequencyLocation.Value[i].Y - frequencyLocation.Value[j].Y;


                    var currentLocation = frequencyLocation.Value[i];
                    do
                    {
                        var antinodeX = currentLocation.X + distX;
                        var antinodeY = currentLocation.Y + distY;

                        var antinode = new Location(antinodeX, antinodeY);
                        if (_map.IsValidLocation(antinode))
                            antinodes.Add(antinode);
                        currentLocation = antinode;
                    }
                    while (_map.IsValidLocation(currentLocation));
                }
            }
        }

        var answer = antinodes.Count;
        Console.WriteLine($"Part 2: {answer}");
    }
}
