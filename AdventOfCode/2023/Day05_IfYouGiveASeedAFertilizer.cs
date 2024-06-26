﻿using System.Text.RegularExpressions;

namespace AdventOfCode._2023;

[Challenge(2023, 5, "If You Give A Seed A Fertilizer")]
internal class Day05_IfYouGiveASeedAFertilizer
{
    private readonly ulong[] _seeds;
    private readonly List<Converter[]> _maps = [];

    public Day05_IfYouGiveASeedAFertilizer()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var lines = File.ReadAllLines(filePath);

        var seedsNumberAsMatches = Regex.Matches(lines[0], "\\d+");
        _seeds = seedsNumberAsMatches.Select(m => m.ToUInt64()).ToArray();

        var mapName = "";
        List<Converter> converters = [];
        for (int i = 1; i <= lines.Length; i++)
        {
            if (i == lines.Length || string.IsNullOrWhiteSpace(lines[i]))
            {
                if (!string.IsNullOrEmpty(mapName))
                {
                    // close current map
                    _maps.Add(converters.ToArray());
                    converters.Clear();
                }

                continue;
            }

            var match = Regex.Match(lines[i], "(\\w+-\\w+-\\w+) map:");
            if (match.Success)
            {
                mapName = match.Groups[1].Value;
                continue;
            }

            var converter = Parse(lines[i]);
            converters.Add(converter);
        }
    }

    [Part(1)]
    public void Part01()
    {
        var lowestLocation = ulong.MaxValue;
        foreach (var seed in _seeds)
        {
            Console.WriteLine("getting location for seed {0}", seed);
            var location = GetLocationForSeed(seed);

            Console.WriteLine("location is {0}", location);
            lowestLocation = (location < lowestLocation) ? location : lowestLocation;
        }

        Console.WriteLine("Anwser for part 1 is {0}", lowestLocation);
    }

    [Part(2)]
    public void Part02()
    {
        var lowestLocation = ulong.MaxValue;
        for (int i = 0; i < _seeds.Length; i++)
        {
            var start = _seeds[i];
            var length = _seeds[++i];
            var end = start + (length-1);

            Console.WriteLine($"start {start}, length {length}, end {end}");

            for (var y = start; y <= end; y++)
            {
                var location = GetLocationForSeed(y);
                lowestLocation = (location < lowestLocation) ? location : lowestLocation;
            }
        }

        Console.WriteLine("lowest: {0}", lowestLocation);
    }

    public ulong GetLocationForSeed(ulong seed)
    {
        var number = seed;
        foreach (var map in _maps)
        {
            number = GetDestination(number, map);
        }
        return number;
    }

    public static Converter Parse(string input)
    {
        var converterMatch = Regex.Match(input, "(\\d+) (\\d+) (\\d+)");
        var converter = new Converter
        {
            DestinationStartRange = converterMatch.Groups[1].ToUInt64(),
            SourceStartRange = converterMatch.Groups[2].ToUInt64(),
            LengthRange = converterMatch.Groups[3].ToUInt64(),
        };
        return converter;
    }

    public static ulong GetDestination(ulong source, Converter[] converters)
    {
        foreach (var converter in converters)
        {
            if (converter.IsInRange(source))
                return converter.GetDestination(source);
        }
        return source;
    }

    public record struct Converter(
        ulong DestinationStartRange,
        ulong SourceStartRange,
        ulong LengthRange)
    {
        public readonly bool IsInRange(ulong number)
        {
            if (number < SourceStartRange)
                return false;
            return SourceStartRange + (LengthRange) > number;
        }

        public readonly ulong GetDestination(ulong source)
        {
            if (!IsInRange(source))
                return source;
            var diff = source - SourceStartRange;
            return DestinationStartRange + diff;
        }
    }
}
