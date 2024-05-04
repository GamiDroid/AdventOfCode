using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2015;

[Challenge(2015, 14)]
internal class Day14_ReindeerOlympics
{
    private readonly string[] _lines;

    public Day14_ReindeerOlympics()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        _lines = File.ReadAllLines(filePath);
    }

    private static ICollection<Reindeer> GetReindeers(string[] lines)
    {
        var reindeers = new List<Reindeer>();
        foreach (var line in lines)
        {
            var match = Regex.Match(line, "(?<name>\\w+) can fly (?<speed>\\d+) km\\/s for (?<runTime>\\d+) seconds, but then must rest for (?<restTime>\\d+) seconds\\.");
            if (match.Success)
            {
                var name = match.Groups["name"].Value;
                var speed = int.Parse(match.Groups["speed"].Value);
                var runTime = int.Parse(match.Groups["runTime"].Value);
                var restTime = int.Parse(match.Groups["restTime"].Value);

                var reindeer = new Reindeer(name, speed, runTime, restTime);
                reindeers.Add(reindeer);
            }
        }

        return reindeers;
    }

    [Part(1)]
    public void ExecutePart01()
    {
        var reindeers = GetReindeers(_lines);
        foreach (var reindeer in reindeers)
        {
            reindeer.ElapseTime(2503);
            Console.WriteLine($"Reindeer {reindeer.Name} traveled {reindeer.DistanceTraveled} km.");
        }

        var winningDistance = reindeers.Max(r => r.DistanceTraveled);
        Console.WriteLine($"The winning distance is {winningDistance} km.");
    }

    [Part(2)]
    public void ExecutePart02()
    {
        var reindeers = GetReindeers(_lines);
        for (int i = 0; i < 2503; i++)
        {
            foreach (var reindeer in reindeers)
                reindeer.ElapseTime();
            var leadDistance = reindeers.Max(r => r.DistanceTraveled);
            var leadReindeers = reindeers.Where(r => r.DistanceTraveled == leadDistance).ToList();
            foreach (var leadReindeer in leadReindeers)
                leadReindeer.AddPoint();
        }

        foreach (var reindeer in reindeers)
            Console.WriteLine($"Reindeer {reindeer.Name} has {reindeer.Points} points.");

        var winningPoints = reindeers.Max(r => r.Points);
        Console.WriteLine($"The winning points is {winningPoints} points.");
    }

    public record Reindeer(string Name, int Speed, int RunTime, int RestTime)
    {
        public int DistanceTraveled { get; private set; } = 0;
        public bool IsRunning { get; private set; }
        public int Points { get; private set; }

        public void ElapseTime(int time)
        {
            for (int i = 0; i < time; i++)
                ElapseTime();
        }

        public void ElapseTime()
        {
            if (_timer <= 0)
            {
                IsRunning = !IsRunning;
                _timer = IsRunning ? RunTime : RestTime;
            }

            if (IsRunning)
                DistanceTraveled += Speed;

            _timer--;
        }

        public void AddPoint() => Points++;

        private int _timer;
    }
}
