using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Challenge(2015, 6)]
internal partial class Day06_ProbablyAFireHazard
{
    private Action[] _testData = default!;

    public Day06_ProbablyAFireHazard()
    {
        _testData = GetTestData();
    }

    [Part(1)]
    public void ExecutePart01()
    {
        var data = _testData;

        var lights = new Lights();

        foreach (var action in data)
        {
            switch (action.Command)
            {
                case "on": lights.TurnOn(action.FromX, action.FromY, action.ToX, action.ToY); break;
                case "off": lights.TurnOff(action.FromX, action.FromY, action.ToX, action.ToY); break;
                case "toggle": lights.Toggle(action.FromX, action.FromY, action.ToX, action.ToY); break;
                default: break;
            }
        }

        var count = lights.CountLit();

        Console.WriteLine("Amount of lights lit: {0}", count);
    }

    [Part(2)]
    public void ExecutePart02()
    {
        var data = _testData;

        var lights = new Lights();

        foreach (var action in data)
        {
            switch (action.Command)
            {
                case "on": lights.TurnUp(action.FromX, action.FromY, action.ToX, action.ToY, 1); break;
                case "off": lights.TurnDown(action.FromX, action.FromY, action.ToX, action.ToY); break;
                case "toggle": lights.TurnUp(action.FromX, action.FromY, action.ToX, action.ToY, 2); break;
                default: break;
            }
        }

        var brightness = lights.CountBrightness();

        Console.WriteLine("Brightness of the lights: {0}", brightness);
    }

    private static Action[] GetTestData()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var lines = File.ReadAllLines(filePath);

        var actions = new List<Action>();
        foreach (var line in lines)
        {
            var action = Action.Parse(line);
            if (action != null)
                actions.Add(action.Value);
        }

        return actions.ToArray();
    }

    public class Lights
    {
        public void TurnOn(int fromX, int fromY, int toX, int toY)
        {
            var lights = GetRange(fromX, fromY, toX, toY);
            foreach (var (x, y) in lights)
                _lights[x, y] = 1;
        }

        public void TurnOff(int fromX, int fromY, int toX, int toY)
        {
            var lights = GetRange(fromX, fromY, toX, toY);
            foreach (var (x, y) in lights)
                _lights[x, y] = 0;
        }

        public void Toggle(int fromX, int fromY, int toX, int toY)
        {
            var lights = GetRange(fromX, fromY, toX, toY);
            foreach (var (x, y) in lights)
            {
                var lit = _lights[x, y];
                if (lit == 0)
                    _lights[x, y] = 1;
                else
                    _lights[x, y] = 0;
            }
        }

        public void TurnUp(int fromX, int fromY, int toX, int toY, int increase)
        {
            var lights = GetRange(fromX, fromY, toX, toY);
            foreach (var (x, y) in lights)
            {
                var brightness = _lights[x, y];
                brightness += increase;
                _lights[x, y] = brightness;
            }
        }

        public void TurnDown(int fromX, int fromY, int toX, int toY)
        {
            var lights = GetRange(fromX, fromY, toX, toY);
            foreach (var (x, y) in lights)
            {
                var brightness = _lights[x, y];
                if (brightness > 0)
                {
                    brightness -= 1;
                    _lights[x, y] = brightness;
                }
            }
        }

        private static IEnumerable<(int x, int y)> GetRange(int fromX, int fromY, int toX, int toY)
        {
            var set = new HashSet<(int x, int y)>();

            var tempFromX = fromX < toX ? fromX : toX;
            var tempToX = fromX >= toX ? fromX : toX;

            var tempFromY = fromY < toY ? fromY : toY;
            var tempToY = fromY >= toY ? fromY : toY;

            for (int x = tempFromX; x <= tempToX; x++)
            {
                for (int y = tempFromY; y <= tempToY; y++)
                {
                    set.Add((x, y));
                }
            }

            return set;
        }

        public int CountLit()
        {
            var count = 0;
            for (int x = 0; x < c_maxX; x++)
            {
                for (int y = 0; y < c_maxY; y++)
                {
                    if (_lights[x, y] > 0)
                        count++;
                }
            }

            return count;
        }

        public int CountBrightness()
        {
            var count = 0;
            for (int x = 0; x < c_maxX; x++)
            {
                for (int y = 0; y < c_maxY; y++)
                {
                    count += _lights[x, y];
                }
            }

            return count;
        }

        private readonly int[,] _lights = new int[c_maxX, c_maxY];

        private const int c_maxX = 1000;
        private const int c_maxY = 1000;
    }

    public partial record struct Action(string Command, int FromX, int FromY, int ToX, int ToY)
    {
        public static Action? Parse(string text)
        {
            var match = ActionRegex().Match(text);
            if (!match.Success)
                return null;
            var command = match.Groups["command"].Value;
            var fromx = Convert.ToInt32(match.Groups["fromx"].Value);
            var fromy = Convert.ToInt32(match.Groups["fromy"].Value);
            var tox = Convert.ToInt32(match.Groups["tox"].Value);
            var toy = Convert.ToInt32(match.Groups["toy"].Value);

            return new Action(command, fromx, fromy, tox, toy);
        }

        [GeneratedRegex("(?<command>\\w+) (?<fromx>\\d+),(?<fromy>\\d+) through (?<tox>\\d+),(?<toy>\\d+)")]
        private static partial Regex ActionRegex();
    }
}
