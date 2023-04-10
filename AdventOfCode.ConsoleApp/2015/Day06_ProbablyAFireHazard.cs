using System.Drawing;
using System.Text.RegularExpressions;

namespace AdventOfCode.ConsoleApp._2015;
public partial class Day06_ProbablyAFireHazard
{
    private Action[] _testData = default!;

    [Setup]
    public void Setup() => _testData = GetTestData();

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

    private static Action[] GetTestData()
    {
        var rootFolder = App.ProjectRootFolder;
        var filePath = Path.Combine(rootFolder, "2015", "Resources", "ProbablyAFireHazard.txt");
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
            foreach (var light in lights)
                _litLights.Add(light);
        }

        public void TurnOff(int fromX, int fromY, int toX, int toY)
        {
            var lights = GetRange(fromX, fromY, toX, toY);
            foreach (var light in lights)
                _litLights.Remove(light);
        }

        public void Toggle(int fromX, int fromY, int toX, int toY)
        {
            var lights = GetRange(fromX, fromY, toX, toY);
            foreach (var light in lights)
            {
                if (_litLights.Contains(light))
                {
                    _litLights.Remove(light);
                }
                else
                {
                    _litLights.Add(light);
                }
            }
        }

        private static IEnumerable<Light> GetRange(int fromX, int fromY, int toX, int toY)
        {
            var set = new HashSet<Light>();

            var tempFromX = fromX < toX ? fromX : toX;
            var tempToX = fromX >= toX ? fromX : toX;

            var tempFromY = fromY < toY ? fromY : toY;
            var tempToY = fromY >= toY ? fromY : toY;

            for (int x = tempFromX; x <= tempToX; x++)
            {
                for (int y = tempFromY; y <= tempToY; y++)
                {
                    set.Add(new Light(x, y));
                }
            }

            return set;
        }

        public int CountLit() => _litLights.Count;

        private readonly HashSet<Light> _litLights = new();
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

    public record struct Light(int X, int Y);
}
