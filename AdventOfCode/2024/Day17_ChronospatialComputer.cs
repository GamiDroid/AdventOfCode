
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace AdventOfCode._2024;
[Challenge(2024, 17, "Chronospatial Computer")]
internal class Day17_ChronospatialComputer
{
    private readonly IReadOnlyDictionary<string, string> _input;

    public Day17_ChronospatialComputer()
    {
        var input = ChallengeHelper.ReadAllTextFromResourceFile();
        _input = Regex.Matches(input, @"((?<t>Register \w)|(?<t>Program)): (?<v>.+)")
            .ToDictionary(m => m.Groups["t"].Value, m => m.Groups["v"].Value);
    }

    [Part(1)]
    public void Part01()
    {
        var computer = new Computer(
            a: int.Parse(_input["Register A"]),
            b: int.Parse(_input["Register B"]),
            c: int.Parse(_input["Register C"]),
            program: _input["Program"].Split(',').Select(n => int.Parse(n)).ToArray());

        var output = string.Join(",", computer.ExecutePrograms());

        Console.WriteLine($"Part 1: {output}");
    }

    public class Computer
    {
        private readonly int[] _values = new int[7];
        private readonly int[] _program;
        private readonly List<int> _output = [];
        private int _instructionPointer = 0;

        private readonly (Action<Computer, int> Execute, string Name, string Source)[] _instructions = [
            ((c, o) => c.Register('A') = (int)Math.Floor(c.Register('A') / Math.Pow(2, c.Combo(o))), "adv", "A = A / 2^combo(o)"),
            ((c, o) => c.Register('B') = c.Register('B') ^ o, "bxl", "B = B ^ literal(o)"),
            ((c, o) => c.Register('B') = c.Combo(o) % 8, "bst", "B = combo(o) % 8"),
            ((c, o) => c._instructionPointer = c.Register('A') == 0 ? c._instructionPointer : o - 2, "jnz", "pointer = literal(0) - 2 [if A==0]"), // decrease pointer by 2 so the jump to the next pointer can still be done.
            ((c, o) => c.Register('B') = c.Register('C') ^ c.Register('B'), "bxc", "B = C ^ B"),
            ((c, o) => c.Output(c.Combo(o) % 8), "out", "out << combo(o) % 8"),
            ((c, o) => c.Register('B') = (int)Math.Floor(c.Register('A') / Math.Pow(2, c.Combo(o))), "bdv", "B = A / 2^combo(o)"),
            ((c, o) => c.Register('C') = (int)Math.Floor(c.Register('A') / Math.Pow(2, c.Combo(o))), "cdv", "C = A / 2^combo(o)"),
        ];

        public Computer(int a, int b, int c, int[] program)
        {
            Combo(0) = 0;
            Combo(1) = 1;
            Combo(2) = 2;
            Combo(3) = 3;
            Register('A') = a;
            Register('B') = b;
            Register('C') = c;
            _program = program;
        }

        public int RegisterA => Register('A');
        public int RegisterB => Register('B');
        public int RegisterC => Register('C');

        public string ExecutePrograms()
        {
            while (true)
            {
                if (_instructionPointer < 0 ||  _instructionPointer > _program.Length - 2)
                    break;

                var instruction = _program[_instructionPointer];
                var operant = _program[_instructionPointer + 1];

                ExecuteProgram(instruction, operant);

                _instructionPointer += 2;
            }

            return string.Join(",", _output);
        }

        private void ExecuteProgram(int instr, int operant)
        {
            var instruction = _instructions[instr];
            instruction.Execute(this, operant);
        }

        private ref int Combo(int index)
        {
            if (index == 7)
                throw new InvalidProgramException();
            return ref _values[index];
        }

        private ref int Register(char register)
        {
            var index = register switch
            {
                'A' => 4,
                'B' => 5,
                'C' => 6,
                _ => throw new ArgumentException("Invalid register", nameof(register))
            };

            return ref _values[index];
        }

        private void Output(int value) => _output.Add(value);
    }
}
