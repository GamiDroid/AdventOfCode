using System.Text.RegularExpressions;

namespace AdventOfCode._2023;

[Challenge(2023, 2, "Cube Conundrum")]
internal class Day02_CubeConundrum
{
    private readonly Dictionary<int, Game[]> _gameCubes = [];

    public Day02_CubeConundrum()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var match = Regex.Match(line, "Game (\\d+):");
            var gameNumber = match.Groups[1].ToInt32();

            var indexColon = line.AsSpan().IndexOf(':');
            var gamesCombined = line.AsSpan()[indexColon..].ToString();

            var games = new List<Game>();
            var gamesStr = gamesCombined.Split(';');
            foreach (var gameStr in gamesStr)
            {
                var game = new Game();
                var matches = Regex.Matches(gameStr, "(\\d+) (red|green|blue)");

                foreach (var m in matches.Cast<Match>())
                {
                    var numberOfCubes = m.Groups[1].ToInt32();
                    var cubeColor = m.Groups[2].ValueSpan;

                    if (cubeColor is "red") game.AddRed(numberOfCubes);
                    else if (cubeColor is "green") game.AddGreen(numberOfCubes);
                    else if (cubeColor is "blue") game.AddBlue(numberOfCubes);
                }

                games.Add(game);
            }

            _gameCubes.Add(gameNumber, games.ToArray());
        }
    }

    [Part(1)]
    public void Part01()
    {
        int maxRed = 12, maxGreen = 13, maxBlue = 14;

        var sum = 0;
        foreach (var game in _gameCubes)
        {
            if (game.Value.All(g => g.Red <= maxRed && g.Green <= maxGreen && g.Blue <= maxBlue))
                sum += game.Key;
        };

        Console.WriteLine("Sum of possible game numbers: {0}", sum);
    }

    [Part(2)]
    public void Part02()
    {
        var sum = 0;
        foreach (var game in _gameCubes)
        {
            var red = game.Value.Max(e => e.Red);
            var green = game.Value.Max(e => e.Green);
            var blue = game.Value.Max(e => e.Blue);

            sum += (red * green * blue);
        };

        Console.WriteLine("Sum of the powers of sets: {0}", sum);
    }

    public class Game
    {
        public int Red { get; private set; }
        public int Green { get; private set; }
        public int Blue { get; private set; }

        public void AddRed(int cubes) => Red += cubes;
        public void AddGreen(int cubes) => Green += cubes;
        public void AddBlue(int cubes) => Blue += cubes;
    }
}
