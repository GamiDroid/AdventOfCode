using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2015;

[Challenge(2015, 9)]
internal class Day09_AllInASingleNight
{
    private readonly TravelMap _map;

    public Day09_AllInASingleNight()
    {
        _map = GetTravelMap();
    }
   
    [Part(1)]
    public void ExecutePart01()
    {
        _map.FindShortest();
    }

    [Part(2)]
    public void ExecutePart02()
    {
        _map.FindLongest();
    }

    private static string[] GetTestData()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        return File.ReadAllLines(filePath);
    }

    public TravelMap GetTravelMap()
    {
        var map = new TravelMap();
        foreach (var line in GetTestData())
        {
            var match = Regex.Match(line, "(\\w+) to (\\w+) = (\\d+)");
            if (match.Success)
            {
                var start = match.Groups[1].Value;
                var end = match.Groups[2].Value;
                var distance = int.Parse(match.Groups[3].Value);

                map.AddRoute(start, end, distance);
            }
        }

        return map;
    }

    public class TravelMap
    {
        public void AddRoute(string start, string end, int distance)
        {
            AddRouteToLocation(start, end, distance);
            AddRouteToLocation(end, start, distance);
        }

        public void FindShortest()
        {
            var shortest = 0;
            foreach (var location in _locations.Keys)
            {
                var s = TravelShortest(location, Array.Empty<string>(), 0, shortest);
                if (s > 0)
                    shortest = s;
            }
        }

        public void FindLongest()
        {
            var longest = 0;
            foreach (var location in _locations.Keys)
            {
                var d = TravelLongest(location, Array.Empty<string>(), 0, longest);
                if (d > 0)
                    longest = d;
            }
        }

        private int TravelShortest(string location, string[] visited, int distanceTraveled, int shortest)
        {
            var newVisited = new List<string>(visited) { location };

            var routes = _locations[location].Keys.Where(l => !newVisited.Contains(l)).ToList();

            if (routes.Count == 0)
            {
                if (newVisited.Count == _locations.Count)
                {
                    if (shortest == 0 || (shortest > 0 && distanceTraveled < shortest))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(string.Join(" > ", newVisited) + " : " + distanceTraveled);
                        Console.ResetColor();

                        return distanceTraveled;
                    }
                }

                return 0;
            }


            foreach (var nextLocation in routes)
            {
                var distance = _locations[location][nextLocation];
                var newDistanceTraveled = distanceTraveled + distance;

                var s = TravelShortest(nextLocation, newVisited.ToArray(), newDistanceTraveled, shortest);
                if (s > 0)
                {
                    shortest = s;
                }
            }

            return shortest;
        }

        private int TravelLongest(string location, string[] visited, int distanceTraveled, int longest)
        {
            var newVisited = new List<string>(visited) { location };

            var routes = _locations[location].Keys.Where(l => !newVisited.Contains(l)).ToList();

            if (routes.Count == 0)
            {
                if (newVisited.Count == _locations.Count)
                {
                    if (longest == 0 || (longest > 0 && distanceTraveled > longest))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(string.Join(" > ", newVisited) + " : " + distanceTraveled);
                        Console.ResetColor();

                        return distanceTraveled;
                    }
                }

                return 0;
            }


            foreach (var nextLocation in routes)
            {
                var distance = _locations[location][nextLocation];
                var newDistanceTraveled = distanceTraveled + distance;

                var d = TravelLongest(nextLocation, newVisited.ToArray(), newDistanceTraveled, longest);
                if (d > 0)
                {
                    longest = d;
                }
            }

            return longest;
        }

        private void AddRouteToLocation(string location, string dest, int distance)
        {
            Routes? routes;
            if (!_locations.TryGetValue(location, out routes))
            {
                routes = new Routes();
                _locations[location] = routes;
            }
            routes.Add(dest, distance);
        }

        private readonly Locations _locations = new();
    }

    public class Locations : Dictionary<string, Routes> { }
    public class Routes : Dictionary<string, int> { }
}
