namespace AdventOfCode._2015;

[Challenge(2015, 3)]
internal class Day03_PerfectlySphericalHousesInAVacuum
{
    private string _testData = string.Empty;

    [Setup]
    public void Setup() => _testData = GetTestData();

    [Part(1)]
    public void ExecutePart01()
    {
        var input = _testData;
        var totalVisitedHouses = GetTotalVisitedHouses(input);

        Console.WriteLine("Total visited houses: {0}", totalVisitedHouses);
    }

    [Part(2)]
    public void ExecutePart02()
    {
        var input = _testData;
        var totalVisitedHouses = GetTotalVisitedHousesWithRoboSanta(input);

        Console.WriteLine("Total visited houses: {0}", totalVisitedHouses);
    }

    private string GetTestData()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        return File.ReadAllText(filePath);
    }

    public static int GetTotalVisitedHouses(string input)
    {
        var coordinatesBeen = new HashSet<Tuple<int, int>>();
        
        var x = 0;
        var y = 0;

        coordinatesBeen.Add(Tuple.Create(x, y));

        foreach (var c in input) 
        {
            var coor = ProcessInput(ref x, ref y, c);
            coordinatesBeen.Add(coor);
        }

        return coordinatesBeen.Count;
    }

    public static int GetTotalVisitedHousesWithRoboSanta(string input)
    {
        var coordinatesBeen = new HashSet<Tuple<int, int>>();

        var xSanta = 0;
        var ySanta = 0;

        var xRobo = 0;
        var yRobo = 0;

        coordinatesBeen.Add(Tuple.Create(0, 0));

        bool flip = true;
        foreach (var c in input)
        {
            Tuple<int, int> pos;
            if (flip)
                pos = ProcessInput(ref xSanta, ref ySanta, c);
            else
                pos = ProcessInput(ref xRobo, ref yRobo, c);
            coordinatesBeen.Add(pos);
            flip = !flip;
        }

        return coordinatesBeen.Count;
    }

    private static Tuple<int, int> ProcessInput(ref int x, ref int y, char input)
    {
        switch (input)
        {
            case '>': x++; break;
            case '<': x--; break;
            case '^': y++; break;
            case 'v': y--; break;
            default: break;
        }

        return Tuple.Create(x, y);
    }
}
