using Microsoft.VisualBasic;
using System.Text.RegularExpressions;

namespace AdventOfCode._2023;

[Challenge(2023, 12, "Hot Springs")]
internal class Day12_HotSprings
{
    private readonly SpringRecord[] _springRecords;

    public Day12_HotSprings()
    {
        var text = ChallengeHelper.ReadAllTextFromResourceFile();
        var matches = Regex.Matches(text, "([.#?]+) ([\\d,]+)");

        _springRecords = matches
            .Select(m => new SpringRecord(
                Springs: m.Groups[1].Value,
                Groups: m.Groups[2].Value.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c.ToString())).ToArray()))
            .ToArray();
    }

    [Part(1)]
    public void Part01()
    {
        var sum = _springRecords.Sum(r => FindAmountOfDifferentArrangements(r.Springs, r.Groups, out _));
        Console.WriteLine($"Sum of diffent arrangements is {sum}");
    }

    [Part(2)]
    public void Part02()
    {
        long sum = 0;
        foreach (var (spring, groups) in _springRecords.Select(x => (x.Springs, x.Groups)))
        {
            long temp = 1;
            string prev= "?", next = "?";
            for (int i = 1; i <= 5; i++)
            {
                if (i == 1) prev = "";
                if (i == 5) next = "";

                var springSolve = prev + spring + next;
                var different = FindAmountOfDifferentArrangements(springSolve, groups, out string newSprings);
                prev = newSprings[^1].ToString();

                temp *= different;
            }

            sum += temp;
        }

        Console.WriteLine($"Sum: {sum}");
    }

    internal static long FindAmountOfDifferentArrangements(string springs, int[] groups, out string newSprings)
    {
        int springsCount = springs.Length;
        int minimumSpringsNeeded = groups.Sum() + groups.Length - 1;

        if (springsCount == minimumSpringsNeeded)
        {
            newSprings = new(springs);
            return 1;
        }

        var damagedSpringsCount = springs.Count(x => x == '#');
        if (damagedSpringsCount == groups.Sum())
        {
            newSprings = springs.Replace('?', '.');
            return 1;
        }

        SpringSolver solver = new(springs, groups);

        bool solvedSpring;
        do
        {
            solvedSpring = false;

            solvedSpring = solvedSpring || solver.TrySolveLeftToRight();
            solvedSpring = solvedSpring || solver.TrySolveRightToLeft();
            
            solver.TrySolveLargestGroups();

        } while (solvedSpring);

        newSprings = springs = solver.GetSprings();
        groups = solver.GetGroups();

        damagedSpringsCount = springs.Count(x => x == '#');
        var damagedSpringsMissingCount = groups.Sum() - damagedSpringsCount;
        var unknownSpringLocations = Regex.Matches(springs, "\\?").Select(m => m.Index).ToArray();

        List<List<int>> combinations = [];
        FindCombinations(unknownSpringLocations, damagedSpringsMissingCount, ref combinations);

        long matches = 0;
        foreach (var combination in combinations)
        {
            var tempSprings = SetSprings(springs, combination, '#');
            var ranges = FindDamagedSpringGroups(tempSprings).Select(x => x.Length).ToArray();
            if (groups.SequenceEqual(ranges))
            {
                matches++;
            }
        }

        return matches;
    }

    private static string SetSprings(string springs, IEnumerable<int> indexes, char springKind)
    {
        var springsAsArr = springs.ToCharArray();
        foreach (var i in indexes)
        {
            springsAsArr[i] = '#';
        }
        return new string(springsAsArr);
    }

    private static List<GroupRange> FindDamagedSpringGroups(string springs)
    {
        var damagedSpringGroups = Regex.Matches(springs, "#+");
        var damagedGroups = damagedSpringGroups
            .Select(m => new GroupRange(m.Index, m.Length))
            .ToList();
        return damagedGroups;
    }

    static void FindCombinations(int[] arr, int x, ref List<List<int>> result, int start = 0, List<int>? path = null)
    {
        path ??= [];

        if (path.Count == x)
        {
            result.Add(new List<int>(path));
            return;
        }

        for (int i = start; i < arr.Length; i++)
        {
            path.Add(arr[i]);
            FindCombinations(arr, x, ref result, i + 1, path);
            path.RemoveAt(path.Count - 1);
        }
    }

    internal record SpringRecord(string Springs, int[] Groups);
    internal readonly record struct GroupRange
    {
        public GroupRange(int start, int length)
        {
            Start = start;
            Length = length;
            End = start + length - 1;
        }

        public int Start { get; }
        public int End { get; }
        public int Length { get; }
    }

    public class SpringSolver(string springs, int[] groups)
    {
        private readonly char[] _springs = springs.ToCharArray();
        private readonly List<int> _groups = [..groups];
        private int _start = 0;
        private int _end = springs.Length - 1;

        public int Length => _end - _start + 1;

        public void TrySolveLargestGroups()
        {
            var damagedGroups = FindDamagedSpringGroups(new(_springs));

            var groupsDescending = _groups.GroupBy(x => x, (k, c) => (Group: k, Count: c.Count())).OrderByDescending(x => x.Group).ToList();

            foreach (var gl in groupsDescending)
            {
                var damagedGroupsWithLengthX = damagedGroups.Where(x => x.Length == gl.Group).ToList();

                foreach (var g in damagedGroupsWithLengthX)
                {
                    RangeSet(_springs, g.Start-1, g.Start-1, '.');
                    RangeSet(_springs, g.End+1, g.End+1, '.');
                }

                if (damagedGroupsWithLengthX.Count != gl.Count)
                    break;
            }
        }

        public bool TrySolveLeftToRight()
        {
            char[] startSprings = [.._springs];

            int start = _start;
            for (int gi = 0; gi < _groups.Count; gi++)
            {
                var group = _groups[gi];
                var springIndex = Array.IndexOf(_springs, '#', start);

                if (springIndex == -1 || springIndex-start > group) 
                {
                    break;
                }

                var unknownSpringCount = _springs
                    .Select((c, i) => (Spring: c, Index: i))
                    .Count(x => x.Index >= start && x.Index < springIndex && x.Spring == '?');
                var correction = group > 1 ? unknownSpringCount : 0;

                var rangeLength = RangeSet(_springs, springIndex, springIndex + (group - correction) - 1, '#');

                if (rangeLength < group || _springs.Length <= springIndex + group)
                {
                    break;
                }
                _springs[springIndex + group] = '.';

                start = springIndex + group;
            }

            return !startSprings.SequenceEqual(_springs);
        }

        public bool TrySolveRightToLeft()
        {
            char[] startSprings = [.. _springs];

            int start = _end;
            for (int gi = _groups.Count - 1; gi >= 0; gi--)
            {
                var group = _groups[gi];
                var springIndex = Array.LastIndexOf(_springs, '#', start);

                if (springIndex == -1 || springIndex < _start || (start - springIndex) > group)
                {
                    break;
                }

                var unknownSpringCount = _springs
                    .Select((c, i) => (Spring: c, Index: i))
                    .Count(x => x.Index <= start && x.Index > springIndex && x.Spring == '?');
                var correction = group > 1 ? unknownSpringCount : 0;

                var startRange = springIndex - (group - correction) + 1;
                var rangeLength = RangeSet(_springs, startRange, springIndex, '#');
                if (rangeLength < group || springIndex - group < 0)
                {
                    break;
                }
                
                _springs[springIndex - group] = '.';
                start = springIndex - group;
            }

            return !startSprings.SequenceEqual(_springs);
        }

        public void Trim()
        {
            TrimLeft();
            TrimRight();
        }

        private void TrimLeft()
        {
            for (int i = _start; i <= _end; i++)
            {
                _start = i;
                if (_springs[i] != '.')
                {
                    break;
                }
            }
        }

        private void TrimRight()
        {
            for (int i = _end; i >= _start; i--)
            {
                _end = i;
                if (_springs[i] != '.')
                {
                    break;
                }
            }
        }

        private static int RangeSet(char[] spring, int start, int end, char value)
        {
            start = int.Max(start, 0);
            end = int.Min(end, spring.Length-1);

            for (int i = start; i <= end; i++)
                spring[i] = value;
            return end - start + 1;
        }

        public string GetSprings()
        {
            return new string(_springs, _start, Length);
        }

        public int[] GetGroups() => [.._groups];
    }
}