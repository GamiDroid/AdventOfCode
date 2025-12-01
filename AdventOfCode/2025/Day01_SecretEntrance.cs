using System.Text.RegularExpressions;

namespace AdventOfCode._2025;

[Challenge(2025, 1, "Secret Entrance")]
internal class Day01_SecretEntrance
{
    private readonly (string Direction, int Times)[] _commands;

    public Day01_SecretEntrance()
    {
        var input = ChallengeHelper.ReadAllTextFromResourceFile();

        var matches = Regex.Matches(input, "(\\w)(\\d+)");

        _commands = [.. matches.Select(m => (m.Groups[1].Value, m.Groups[2].ToInt32()))];
    }

    [Part(1)]
    public void Part01()
    {
        var codeLock = new CodeLock(50);

        var counter = 0;
        foreach (var (direction, times) in _commands)
        {
            var newValue = (direction) switch
            {
                "L" => codeLock.Update(-times),
                "R" => codeLock.Update(times),
                _ => throw new InvalidOperationException(),
            };

            if (newValue == 0)
                counter++;
        }

        Console.WriteLine($"Lock was {counter} times on zero.");
    }

    public class CodeLock(int value)
    {
        private int _value = value;

        public int Update(int changes)
        {
            changes %= 100;

            _value += changes;

            if (_value < 0)
                _value += 100;

            if (_value > 99)
                _value %= 100;

            return _value;
        }
    }
}
