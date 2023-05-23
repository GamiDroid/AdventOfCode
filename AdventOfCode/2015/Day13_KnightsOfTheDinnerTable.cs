using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventOfCode._2015;

[Challenge(2015, 13)]
internal class Day13_KnightsOfTheDinnerTable
{
    private string? _testData;

    [Setup]
    public void Setup()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        _testData = File.ReadAllText(filePath);
    }

    [Part(1)]
    public void ExecutePart01()
    {
        if (_testData is null)
            throw new InvalidDataException("No test data available.");

        var invites = GetInvites(_testData);
        var names = invites.Keys.ToArray();

        var permutations = MakePermutations(names);

        var highest = 0;
        foreach (var nms in permutations)
        {
            var happiness = 0;
            for(var i = 0; i < nms.Length; i++)
            {
                var p = nms[i];

                if (invites.TryGetValue(p, out var personHappiness))
                {
                    string? lhs;
                    string? rhs;
                    if (i == 0)
                    {
                        lhs = nms[^1];
                        rhs = nms[i + 1];
                    }
                    else if (i == nms.Length - 1)
                    {
                        lhs = nms[i - 1];
                        rhs = nms[0];
                    }
                    else
                    {
                        lhs = nms[i - 1];
                        rhs = nms[i + 1];
                    }

                    if (personHappiness.TryGetValue(lhs, out int happinessLeft))
                        happiness += happinessLeft;


                    if (personHappiness.TryGetValue(rhs, out int happinessRight))
                        happiness += happinessRight;
                }
            }

            Console.WriteLine($"Combination ({string.Join(", ", nms)}) has combined happiness of {happiness}");
            highest = happiness > highest ? happiness : highest;
        }

        Console.WriteLine($"Highest happiness is {highest}");
    }

    [Part(2)]
    public void ExecutePart02()
    {
        if (_testData is null)
            throw new InvalidDataException("No test data available.");

        var myName = "GamiDroid";

        var invites = GetInvites(_testData);
        invites.Add(myName, new PersonHappiness());

        var highest = FindHighestHappiness(invites);

        Console.WriteLine($"Highest happiness is {highest}");
    }

    private static string[][] MakePermutations(string[] names, string[]? current = null, string[][]? permutations = null)
    {
        current ??= Array.Empty<string>();
        permutations ??= Array.Empty<string[]>();

        if (current.Length == names.Length)
        {
            var newPermutations = NewArrayAndAdd(permutations, current);
            return newPermutations;
        }
        else
        {
            foreach (var name in names)
            {
                if (!current.Contains(name))
                {
                    var newCurrent = NewArrayAndAdd(current, name);
                    permutations = MakePermutations(names, newCurrent, permutations);
                }
            }
        }

        return permutations;
    }

    private static int FindHighestHappiness(IDictionary<string, PersonHappiness> inviteHappiness, string[]? current = null, int currentHightest = 0)
    {
        current ??= Array.Empty<string>();
        if (current.Length == inviteHappiness.Count)
        {
            return GetHappiness(inviteHappiness, current);
        }
        else
        {
            foreach (var name in inviteHappiness.Keys)
            {
                if (!current.Contains(name))
                {
                    var newCurrent = NewArrayAndAdd(current, name);
                    int happiness = FindHighestHappiness(inviteHappiness, newCurrent, currentHightest);
                    if (happiness > currentHightest) 
                        currentHightest = happiness;
                }
            }
        }

        return currentHightest;
    }

    private static int GetHappiness(IDictionary<string, PersonHappiness> invites, string[] positions)
    {
        var happiness = 0;
        var myName = "GamiDroid";
        for (var i = 0; i < positions.Length; i++)
        {
            var p = positions[i];

            if (p == myName)
                continue;

            if (invites.TryGetValue(p, out var personHappiness))
            {
                string? lhs;
                string? rhs;
                if (i == 0)
                {
                    lhs = positions[^1];
                    rhs = positions[i + 1];
                }
                else if (i == positions.Length - 1)
                {
                    lhs = positions[i - 1];
                    rhs = positions[0];
                }
                else
                {
                    lhs = positions[i - 1];
                    rhs = positions[i + 1];
                }

                if (lhs != myName)
                {
                    if (personHappiness.TryGetValue(lhs, out int happinessLeft))
                        happiness += happinessLeft;
                }

                if (rhs != myName)
                {
                    if (personHappiness.TryGetValue(rhs, out int happinessRight))
                        happiness += happinessRight;
                }
            }
        }

        return happiness;
    }

    private static T[] NewArrayAndAdd<T>(T[] source, T item)
    {
        var temp = new T[source.Length + 1];
        Array.Copy(source, temp, source.Length);
        temp[^1] = item;

        return temp;
    }

    private static IDictionary<string, PersonHappiness> GetInvites(string lines)
    {
        var invites = new Dictionary<string, PersonHappiness>();

        var matches = Regex.Matches(lines, "(?<fname>\\w+) would (?<factor>\\w+) (?<value>\\d+) happiness units by sitting next to (?<sname>\\w+)\\.");
        if (matches.Count > 0)
        {
            foreach (Match match in matches.Cast<Match>())
            {
                if (match.Success)
                {
                    var firstName = match.Groups["fname"].Value;
                    var secondName = match.Groups["sname"].Value;
                    var factor = match.Groups["factor"].Value;
                    var value = int.Parse(match.Groups["value"].Value);

                    var happiness = value * ((factor == "lose") ? -1 : 1);

                    if (!invites.ContainsKey(firstName))
                        invites[firstName] = new PersonHappiness();
                    invites[firstName].Add(secondName, happiness);
                }
            }
        }

        return invites;
    }

    private class PersonHappiness : Dictionary<string, int> { }
}
