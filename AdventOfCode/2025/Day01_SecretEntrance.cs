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
    public void Part1()
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

    [Part(2)]
    public void Part2()
    {
        var codeLock = new CodeLock(50);
        var codeLock2 = new CodeLock(50);

        var counter = 0;
        foreach (var (direction, ticks) in _commands)
        {
            var timesZero = (direction) switch
            {
                "L" => codeLock.ForceRotate(-ticks),
                "R" => codeLock.ForceRotate(ticks),
                _ => throw new InvalidOperationException(),
            };

            var timesZero2 = (direction) switch
            {
                "L" => codeLock2.Rotate(-ticks),
                "R" => codeLock2.Rotate(ticks),
                _ => throw new InvalidOperationException(),
            };

            if (timesZero != timesZero2 ||
                codeLock.Value != codeLock2.Value)
            {
                Console.WriteLine("DBG: Value differ");
            }

            counter += timesZero;
        }

        // 5937
        Console.WriteLine($"Lock was {counter} times on zero.");
    }

    public class CodeLock(int value)
    {
        private int _value = value;

        public int Value => _value;

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

        public int Rotate(int changes)
        {
            int timesZero = Math.Abs(changes / 100);

            changes %= 100;

            int prev = _value;

            _value += changes;

            if (_value < 0)
            {
                _value += 100;

                if (prev != 0)
                    timesZero++;
            }

            if (_value > 99 || _value == 0)
            {
                _value %= 100;
                timesZero++;
            }

            return timesZero;
        }

        public int ForceRotate(int changes)
        {
            int timesZero = 0;

            for (int i = 0; i < Math.Abs(changes); i++)
            {
                if (changes > 0)
                    _value++;
                else
                    _value--;

                if (_value == 100)
                    _value = 0;

                if (_value < 0)
                    _value = 99;

                if (_value == 0)
                    timesZero++;
            }

            return timesZero;
        }
    }
}
