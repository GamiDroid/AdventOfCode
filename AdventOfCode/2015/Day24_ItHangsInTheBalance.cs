﻿using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Challenge(2015, 24)]
internal class Day24_ItHangsInTheBalance
{
    private readonly int[] _presents;

    public Day24_ItHangsInTheBalance()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var lines = File.ReadAllLines(filePath);

        var presents = new List<int>();
        foreach (var line in lines)
        {
            if (int.TryParse(line, out int i))
                presents.Add(i);
        }

        _presents = presents.ToArray();
    }

    [Part(1)]
    public void Part01()
    {
        var foundCombinations = new List<int[]>();
        var countPresents = 2;

        while (foundCombinations.Count == 0)
        {
            foundCombinations = FindGroupsWithEqualWeight(_presents, countPresents, 3).ToList();
            countPresents++;
        }


        var smallest = GetSmallestQuantumEntanglement(foundCombinations);

        Console.WriteLine($"Smallest: {smallest}");
    }

    [Part(2)]
    public void Part02()
    {
        var foundCombinations = new List<int[]>();
        var countPresents = 2;

        while (foundCombinations.Count == 0)
        {
            foundCombinations = FindGroupsWithEqualWeight(_presents, countPresents, 4).ToList();
            countPresents++;
        }

        var smallest = GetSmallestQuantumEntanglement(foundCombinations);

        Console.WriteLine($"Smallest: {smallest}");
    }

    private static ulong GetSmallestQuantumEntanglement(ICollection<int[]> combinations)
    {
        if (!combinations.Any())
            return ulong.MaxValue;

        ulong? smallest = null;
        foreach (var combination in combinations)
        {
            ulong total = Convert.ToUInt64(combination[0]);

            if (combination.Length > 1)
            {
                for (int i = 1; i < combination.Length; i++)
                {
                    total *= Convert.ToUInt64(combination[i]);
                }
            }

            if (smallest is null || total < smallest)
                smallest = total;
        }

        return smallest ?? ulong.MaxValue;
    }

    private static ICollection<int[]> FindGroupsWithEqualWeight(int[] presents, int countPresentsFirstGroup, int totalGroups)
    {
        var groupOnesWithEqualSums = new List<int[]>();

        foreach (var group1 in GetFixedLengthCombinations(countPresentsFirstGroup, presents))
        {
            var sumGrp1 = group1.Sum();

            if (FindGroups(sumGrp1, totalGroups - 1, presents, group1))
                groupOnesWithEqualSums.Add(group1);
        }

        return groupOnesWithEqualSums;
    }

    private static bool FindGroups(int sumFirstGroup, int groupNum, int[] presents, int[] presentsPreviousGroup)
    {
        var newPresentsPool = CopyAndSubtract(presents, presentsPreviousGroup);

        if (groupNum == 1)
        {
            var sum = newPresentsPool.Sum();
            if (sum == sumFirstGroup)
                return true;
        }
        else
        {
            foreach (var group in GetFixedSumCombinations(sumFirstGroup, newPresentsPool))
            {
                return FindGroups(sumFirstGroup, groupNum - 1, newPresentsPool, group);
            }
        }

        return false;
    }

    private static int[] CopyAndSubtract(int[] src, int[] dst)
    {
        var newArr = new List<int>(src);

        // Remove already taken presents.
        foreach (var present in dst)
            newArr.Remove(present);

        return newArr.ToArray();
    }

    private static IEnumerable<int[]> GetFixedLengthCombinations(int length, int[] source, int? startIndex = null, int[]? current = null)
    {
        current ??= Array.Empty<int>();
        startIndex ??= source.Length - 1;

        if (current.Length == length)
            yield return current;
        else
        {
            for (int i = (int)startIndex; i >= 0; i--)
            {
                var temp = new List<int>(current) { source[i] };
                foreach (var item in GetFixedLengthCombinations(length, source, i - 1, temp.ToArray()))
                    yield return item;
            }
        }
    }

    private static IEnumerable<int[]> GetFixedSumCombinations(int sum, int[] source, int? startIndex = null, int[]? current = null)
    {
        current ??= Array.Empty<int>();
        startIndex ??= source.Length - 1;

        var currentSum = current.Sum();

        if (currentSum == sum)
            yield return current;
        else if (currentSum > sum || startIndex < 0)
            yield return Array.Empty<int>();
        else
        {
            for (int i = (int)startIndex; i >= 0; i--)
            {
                var temp = new List<int>(current) { source[i] };
                foreach (var item in GetFixedSumCombinations(sum, source, i - 1, temp.ToArray()))
                {
                    if (item.Length > 0)
                        yield return item;
                }
            }
        }
    }
}
