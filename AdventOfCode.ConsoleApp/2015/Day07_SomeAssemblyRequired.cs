namespace AdventOfCode.ConsoleApp._2015;

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

        var wireA = wires.GetSignal("a");

        Console.WriteLine($"wire a = '{wireA}'");
    }

    private static string[] GetTestData()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var lines = File.ReadAllLines(filePath);
        return lines;
    }

    public class WireSet
    {
        public ushort GetSignal(string wire)
        {
            if (!_connections.ContainsKey(wire))
                throw new ArgumentException($"Wire '{wire}' does not exists", nameof(wire));

            if (_outputs.TryGetValue(wire, out ushort cachedSignal))
            {
                Console.WriteLine($"Get cached signal for wire '{wire}' = '{cachedSignal}'.");
                return cachedSignal;
            }

            var source = _connections[wire];

            if (source.Inputs.Any())
            {
                Console.WriteLine("Source has inputs: " + string.Join(", ", source.Inputs));
            }

            List<ushort> signals = new();
            foreach (var input in source.Inputs)
            {
                var s = GetSignal(input);
                signals.Add(s);
            }

            var signal = source.Type switch
            {
                SourceType.Constant => source.ConstantValue ?? 0,
                SourceType.Wire => signals[0],
                SourceType.NotGate => (ushort)~signals[0],
                SourceType.LShiftGate => (ushort)(signals[0] << source.ConstantValue!.Value),
                SourceType.RShiftGate => (ushort)(signals[0] >> source.ConstantValue!.Value),
                SourceType.AndGate => (ushort)(signals[0] & signals[1]),
                SourceType.OrGate => (ushort)(signals[0] | signals[1]),
                _ => ushort.MinValue,
            };

            Console.WriteLine($"Signal for wire '{wire}' = '{signal}' with input type {source.Type}.");

            return signal;
        }

        public void Connect(string connection)
        {
            var split = connection.Split("->", StringSplitOptions.TrimEntries);

            var source = split[0];
            var target = split[1];

            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(target))
                throw new ArgumentException("Connection is invalid format", nameof(connection));

            var s = GetSource(source);
            _connections[target] = s;
        }

        private static Source GetSource(string source)
        {
            var sourceType = GetSourceType(source);

            if (sourceType == SourceType.Constant)
            {
                var constantValue = ushort.Parse(source);
                return new Source(sourceType, constantValue);
            }

            var split = source.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            switch (sourceType)
            {
                case SourceType.Wire:
                    return new Source(sourceType, null, split[0]);
                case SourceType.NotGate:
                    return new Source(sourceType, null, split[1]);
                case SourceType.LShiftGate:
                case SourceType.RShiftGate:
                    var constantValue = ushort.Parse(split[2]);
                    return new Source(sourceType, constantValue, split[0]);
                case SourceType.AndGate:
                case SourceType.OrGate:
                    return new Source(sourceType, null, split[0], split[2]);
                default:
                    break;
            }

            throw new ArgumentException("Source is invalid format", nameof(source));
        }

        private static SourceType GetSourceType(string source)
        {
            if (source.Contains("NOT"))
                return SourceType.NotGate;
            else if (source.Contains("LSHIFT"))
                return SourceType.LShiftGate;
            else if (source.Contains("RSHIFT"))
                return SourceType.RShiftGate;
            else if (source.Contains("OR"))
                return SourceType.OrGate;
            else if (source.Contains("AND"))
                return SourceType.AndGate;
            if (ushort.TryParse(source, out var _))
                return SourceType.Constant;
            return SourceType.Wire;
        }

        private readonly IDictionary<string, Source> _connections = new Dictionary<string, Source>();
        private readonly IDictionary<string, ushort> _outputs = new Dictionary<string, ushort>();
    }

    public record Source(SourceType Type, ushort? ConstantValue, params string[] Inputs);

    public enum SourceType 
    { 
        Constant = 1,
        Wire = 2,
        NotGate = 3,
        LShiftGate = 4,
        RShiftGate = 5,
        AndGate = 6,
        OrGate = 7,
    }
}
