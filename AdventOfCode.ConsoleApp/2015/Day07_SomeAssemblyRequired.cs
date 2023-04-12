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
            wires.DoCommand(line);
        Console.WriteLine($"wire a = '{wires["a"]}'");
    }

    private static string[] GetTestData()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var lines = File.ReadAllLines(filePath);
        return lines;
    }

    public class WireSet
    {
        public ushort this[string wire] 
        {
            get
            {
                if (!_outputs.ContainsKey(wire))
                    return 0;
                return _outputs[wire];
            }
            set
            {
                _outputs[wire] = value;
            }
        }

        public void DoCommand(string command)
        {
            var split = command.Split("->", StringSplitOptions.TrimEntries);
            
            var action = split[0];
            var assign = split[1];

            if (string.IsNullOrWhiteSpace(action) || string.IsNullOrWhiteSpace(assign))
                throw new ArgumentException("Command is invalid format", nameof(command));

            var current = this[assign];
            var output = GetOutput(action);

            Console.WriteLine(command);
            Console.WriteLine($"{assign}: {Convert.ToString(current, 2)} ({current}) -> {Convert.ToString(output, 2)} ({output})");

            this[assign] = output;
        }

        public ushort GetOutput(string action)
        {
            if (action.Contains("NOT"))
                return DoNotAction(action);
            else if (action.Contains("LSHIFT"))
                return DoShiftAction(action, 'L');
            else if (action.Contains("RSHIFT"))
                return DoShiftAction(action, 'R');
            else if (action.Contains("OR"))
                return DoOrAction(action);
            else if (action.Contains("AND"))
                return DoAndAction(action);
            return GetAssign(action);
        }

        public string OutputString(string wire) => Convert.ToString(this[wire], toBase: 2);

        private ushort DoNotAction(string action)
        {
            var wire = action.Split(' ')[1];
            var output = (ushort)~this[wire];
            return output;
        }

        private ushort DoShiftAction(string action, char direction)
        {
            (var lhs, var rhs) = GetValues(action);

            var wire = lhs;
            var shiftConst = ushort.Parse(rhs);

            ushort output;
            var input = this[wire];
            if (direction == 'L')
                output = (ushort)(input << shiftConst);
            else
                output = (ushort)(input >> shiftConst);
            return output;
        }

        private ushort DoAndAction(string action)
        {
            (var lhs, var rhs) = GetValues(action);

            var output = (ushort)(this[lhs] & this[rhs]);
            return output;
        }

        private ushort DoOrAction(string action)
        {
            (var lhs, var rhs) = GetValues(action);

            var output = (ushort)(this[lhs] | this[rhs]);
            return output;
        }

        private ushort GetAssign(string action) => ushort.TryParse(action, out ushort o) ? o : this[action];

        private static (string lhs, string rhs) GetValues(string action)
        {
            var split = action.Split(' ');

            if (split.Length != 3)
                throw new ArgumentException("Action in invalid format", nameof(action));

            return (split[0], split[2]);
        }

        private readonly IDictionary<string, ushort> _outputs = new Dictionary<string, ushort>();
    }
}
