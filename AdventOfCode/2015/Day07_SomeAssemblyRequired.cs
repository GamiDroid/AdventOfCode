using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Challenge(2015, 7)]
internal class Day07_SomeAssemblyRequired
{
    private string[] _testData = default!;

    [Setup]
    public void Setup() => _testData = GetTestData();

    [Part(1)]
    public void ExecutePart01()
    {
        var wires = new WireSet();
        foreach (var line in _testData)
            wires.Connect(line);

        var signalA = wires.Signal("a");
        Console.WriteLine($"wire a = '{signalA}'");
    }

    [Part(2)]
    public void ExecutePart02()
    {
        var wires = new WireSet();
        foreach (var line in _testData)
            wires.Connect(line);
        wires.Signal("a");
        wires.Connect("3176 -> b");

        wires.Clear();

        var a = wires.Signal("a");

        Console.WriteLine("a: " + a);
    }

    private static string[] GetTestData()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var lines = File.ReadAllLines(filePath);
        return lines;
    }

    public class WireSet
    {
        public void Connect(string connection)
        {
            var split = connection.Split("->", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var source = split[0];
            var target = split[1];

            var b = Gate(target, source, "(\\w+) AND (\\w+)", v => Signal(v[0]) & Signal(v[1])) || 
                    Gate(target, source, "(\\w+) OR (\\w+)", v => Signal(v[0]) | Signal(v[1])) ||
                    Gate(target, source, "(\\w+) LSHIFT (\\w+)", v => Signal(v[0]) << Signal(v[1])) || 
                    Gate(target, source, "(\\w+) RSHIFT (\\w+)", v => Signal(v[0]) >> Signal(v[1])) || 
                    Gate(target, source, "NOT (\\w+)", v => ~Signal(v[0])) || 
                    Gate(target, source, "(\\w+)", v => Signal(v[0]));
        }

        public bool Gate(string target, string source, string regex, Func<string[], int> rhs)
        {
            var match = Regex.Match(source, regex);
            if (!match.Success)
            {
                return false;
            }

            var groups = match.Groups.Values.Select(g => g.Value).ToArray()[1..];

            _connections[target] = new Gate(groups, rhs);

            return true;
        }

        public ushort Signal(string input)
        {
            if (ushort.TryParse(input, out ushort constant))
                return constant;
            if (_signals.TryGetValue(input, out ushort cached))
                return cached;

            var gate = _connections[input];
            var signal = (ushort)gate.Func(gate.Values);

            _signals[input] = signal;

            return signal;
        }

        public void Clear() => _signals.Clear();

        private readonly Dictionary<string, Gate> _connections = new();
        private readonly Dictionary<string, ushort> _signals = new();
    }

    public class Gate
    {
        public Gate(string[] values, Func<string[], int> func)
        {
            Func = func;
            Values = values;
        }

        public Func<string[], int> Func { get; init; }
        public string[] Values { get; init; }
    }
}
