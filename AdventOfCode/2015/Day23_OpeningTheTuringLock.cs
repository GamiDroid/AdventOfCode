using Microsoft.Win32;

namespace AdventOfCode._2015;

[Challenge(2015, 23)]
internal class Day23_OpeningTheTuringLock
{
    private readonly IInstruction[] _instructions;

    public Day23_OpeningTheTuringLock()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var lines = File.ReadAllLines(filePath);

        var instructions = new List<IInstruction>();
        foreach (var line in lines)
        {
            var instruction = InstructionParser.Parse(line);
            instructions.Add(instruction);
        }

        _instructions = instructions.ToArray();
    }

    [Part(1)]
    public void Part01()
    {
        var computer = new Computer(_instructions);

        while(!computer.IndexBeyondInstructions)
            computer.ExecuteCurrentInstruction();
        var valueRegisterB = computer.GetRegister('b');

        Console.WriteLine($"Register B = '{valueRegisterB}'");
    }

    [Part(2)]
    public void Part02()
    {
        var computer = new Computer(_instructions);
        computer.SetRegister('a', 1);

        while (!computer.IndexBeyondInstructions)
            computer.ExecuteCurrentInstruction();
        var valueRegisterB = computer.GetRegister('b');

        Console.WriteLine($"Register B = '{valueRegisterB}'");
    }

    public class Computer
    {
        private readonly IInstruction[] _instructions;
        private readonly IDictionary<char, uint> _registers;

        public Computer(IInstruction[] instructions)
        {
            _instructions = instructions;
            _registers = new Dictionary<char, uint>();
        }

        public int CurrentInstructionIndex { get; set; }

        public uint GetRegister(char register)
        {
            if (_registers.TryGetValue(register, out var value))
                return value;
            return 0;
        }

        public void SetRegister(char register, uint value)
        {
            if (!_registers.ContainsKey(register))
                _registers.Add(register, value);
            else
                _registers[register] = value;
        }

        public bool IndexBeyondInstructions => CurrentInstructionIndex >= _instructions.Length;

        public void ExecuteCurrentInstruction()
        {
            if (IndexBeyondInstructions)
                throw new InvalidOperationException("Current instruction index is beyond the defined instructions");

            var instruction = _instructions[CurrentInstructionIndex];
            instruction.Execute(this);
        }
    }

    public class InstructionParser
    {
        public static IInstruction Parse(ReadOnlySpan<char> text)
        {
            var instructionType = text.TrimStart()[..3];
            return instructionType switch
            {
                "hlf" => HalfInstruction.Parse(text),
                "tpl" => TripleInstruction.Parse(text),
                "inc" => IncrementInstruction.Parse(text),
                "jmp" => JumpInstruction.Parse(text),
                "jie" => JumpEvenInstruction.Parse(text),
                "jio" => JumpOneInstruction.Parse(text),
                _ => throw new ArgumentException($"Given value '{text}' is no valid instruction.", nameof(text))
            };
        }
    }

    public class HalfInstruction : IInstruction
    {
        public char Register { get; private init; }

        public void Execute(Computer computer)
        {
            var value = computer.GetRegister(Register);
            var newValue = value / 2;
            computer.SetRegister(Register, newValue);

            computer.CurrentInstructionIndex++;
        }

        public static IInstruction Parse(ReadOnlySpan<char> text)
        {
            if (!text.TrimStart()[..3].SequenceEqual("hlf"))
                throw new ArgumentException($"Given instruction '{text}' is not of type 'hlf'", nameof(text));

            var lastSpace = text.LastIndexOf(' ');
            var register = text.TrimEnd()[(lastSpace+1)..][0];
            return new HalfInstruction { Register = register };
        }
    }

    public class TripleInstruction : IInstruction
    {
        public char Register { get; private init; }

        public void Execute(Computer computer)
        {
            var value = computer.GetRegister(Register);
            var newValue = value * 3;
            computer.SetRegister(Register, newValue);

            computer.CurrentInstructionIndex++;
        }

        public static IInstruction Parse(ReadOnlySpan<char> text)
        {
            if (!text.TrimStart()[..3].SequenceEqual("tpl"))
                throw new ArgumentException($"Given instruction '{text}' is not of type 'tpl'", nameof(text));

            var lastSpace = text.LastIndexOf(' ');
            var register = text.TrimEnd()[(lastSpace + 1)..][0];
            return new TripleInstruction{ Register = register };
        }
    }

    public class IncrementInstruction : IInstruction
    {
        public char Register { get; private init; }

        public void Execute(Computer computer)
        {
            var value = computer.GetRegister(Register);
            var newValue = value + 1;
            computer.SetRegister(Register, newValue);

            computer.CurrentInstructionIndex++;
        }

        public static IInstruction Parse(ReadOnlySpan<char> text)
        {
            if (!text.TrimStart()[..3].SequenceEqual("inc"))
                throw new ArgumentException($"Given instruction '{text}' is not of type 'inc'", nameof(text));

            var lastSpace = text.LastIndexOf(' ');
            var register = text.TrimEnd()[(lastSpace + 1)..][0];
            return new IncrementInstruction { Register = register };
        }
    }

    public class JumpInstruction : IInstruction
    {
        public int Offset { get; private init; }

        public void Execute(Computer computer)
        {
            computer.CurrentInstructionIndex += Offset;
        }

        public static IInstruction Parse(ReadOnlySpan<char> text)
        {
            if (!text.TrimStart()[..3].SequenceEqual("jmp"))
                throw new ArgumentException($"Given instruction '{text}' is not of type 'jmp'", nameof(text));

            var lastSpace = text.LastIndexOf(' ');
            var offsetText = text.TrimEnd()[(lastSpace + 1)..];

            if (!int.TryParse(offsetText, out int offset))
                throw new ArgumentException($"Could not parse offset '{offsetText}' to int in '{text}'", nameof(text));

            return new JumpInstruction { Offset = offset };
        }
    }

    public class JumpEvenInstruction : IInstruction
    {
        public int Offset { get; private init; }
        public char Register { get; private init; }

        public void Execute(Computer computer)
        {
            var value = computer.GetRegister(Register);

            var offset = 1;
            if (value % 2 == 0)
                offset = Offset;

            computer.CurrentInstructionIndex += offset;
        }

        public static IInstruction Parse(ReadOnlySpan<char> text)
        {
            if (!text.TrimStart()[..3].SequenceEqual("jie"))
                throw new ArgumentException($"Given instruction '{text}' is not of type 'jie'", nameof(text));

            var lastSpace = text.LastIndexOf(' ');
            var offsetText = text.TrimEnd()[lastSpace..];

            if (!int.TryParse(offsetText, out int offset))
                throw new ArgumentException($"Could not parse offset '{offsetText}' to int in '{text}'", nameof(text));

            var firstSpace = text.IndexOf(' ');
            var register = text[firstSpace+1];

            return new JumpEvenInstruction { Offset = offset, Register = register };
        }
    }

    public class JumpOneInstruction : IInstruction
    {
        public int Offset { get; private init; }
        public char Register { get; private init; }

        public void Execute(Computer computer)
        {
            var value = computer.GetRegister(Register);
            var offset = 1;
            if (value == 1)
                offset = Offset;

            computer.CurrentInstructionIndex += offset;
        }

        public static IInstruction Parse(ReadOnlySpan<char> text)
        {
            if (!text.TrimStart()[..3].SequenceEqual("jio"))
                throw new ArgumentException($"Given instruction '{text}' is not of type 'jio'", nameof(text));

            var lastSpace = text.LastIndexOf(' ');
            var offsetText = text.TrimEnd()[(lastSpace + 1)..];

            if (!int.TryParse(offsetText, out int offset))
                throw new ArgumentException($"Could not parse offset '{offsetText}' to int in '{text}'", nameof(text));

            var firstSpace = text.IndexOf(' ');
            var register = text[firstSpace + 1];

            return new JumpOneInstruction { Offset = offset, Register = register };
        }
    }

    public interface IInstruction
    {
        void Execute(Computer computer);
    }
}
