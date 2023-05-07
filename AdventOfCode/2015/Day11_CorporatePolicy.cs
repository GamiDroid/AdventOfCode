using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2015;

[Challenge(2015, 11)]
internal class Day11_CorporatePolicy
{
    [Part(1)]
    public void ExecutePart01()
    {
        var passwordGen = new PasswordGenerator("vzbxkghb");
        bool valid;
        do
        {
            passwordGen.Next();
            valid = passwordGen.Validate();
        }
        while (!valid);

        var password = passwordGen.Password;

        Console.WriteLine($"Next valid password: '{password}'");
    }

    [Part(2)]
    public void ExecutePart02()
    {
        var passwordGen = new PasswordGenerator("vzbxxyzz");
        bool valid;
        do
        {
            passwordGen.Next();
            valid = passwordGen.Validate();
        }
        while (!valid);

        var password = passwordGen.Password;

        Console.WriteLine($"Next valid password: '{password}'");
    }

    public class PasswordGenerator
    {
        public PasswordGenerator(ReadOnlySpan<char> password)
        {
            if (password.Length != 8)
                throw new ArgumentException("Given password has incorrect length", nameof(password));

            for (int i = 0; i < password.Length; i++)
            {
                var character = password[i];
                if (character < 'a' || character > 'z')
                    throw new ArgumentException("Given password contains incorrect characters. Valid characters [a-z]");
            }

            _password = password.ToArray();
        }

        public PasswordGenerator()
        {
            _password = new string('a', 8).ToArray();
        }

        public void Next()
        {
            if (_password.All(c => c == 'z'))
                throw new InvalidOperationException("Max password is reached.");

            for (int i = 7; i >= 0; i--)
            {
                var c = _password[i];

                var next = GetNextValidChar(_password[i]);

                _password[i] = next;

                if (next != 'a')
                    break;
            }
        }

        public bool Validate(out ICollection<string> errors)
        {
            errors = new List<string>();

            if (ContainsInvalidCharacters())
                errors.Add($"Passwords may not contain the letters [{string.Join(", ", s_invalidChars)}]");

            if (!ContainsStraight(3))
                errors.Add("Passwords must include one increasing straight of at least three letters");

            if (CountPairs() < 2)
                errors.Add("Passwords must contain at least two different, non-overlapping pairs of letters");

            return !errors.Any();
        }

        public bool Validate() => Validate(out _);

        private bool ContainsInvalidCharacters() => _password.Any(c => s_invalidChars.Contains(c));

        private bool ContainsStraight(int count)
        {
            for (int i = 0; i < _password.Length - count; i++)
            {
                var c = _password[i];
                bool isStraight = true;
                for (int j = 1; j < count; j++)
                {
                    if (_password[i + j] != (char)(c + j))
                    {
                        isStraight = false;
                        break;
                    }
                }

                if (isStraight) return true;
            }

            return false;
        }

        private int CountPairs()
        {
            int count = 0;
            for (int i = 0; i < _password.Length - 1; i++)
            {
                if (_password[i] == _password[i + 1])
                {
                    count++;
                    i += 2;
                }
            }

            return count;
        }

        private static char GetNextValidChar(char current)
        {
            char next = current;
            while (true)
            {
                next = (next < 'z') ? (char)(next + 1) : 'a';

                if (!s_invalidChars.Contains(next))
                    return next;
            }
        }

        public string? Password => new(_password);

        private readonly char[] _password = new char[8];
        private static readonly char[] s_invalidChars = new char[] { 'i', 'l', 'o' };
    }
}
