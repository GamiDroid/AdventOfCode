using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024;
[Challenge(2024, 9, "Disk Fragmenter")]
internal class Day09_DiskFragmenter
{
    private readonly string _input;

    public Day09_DiskFragmenter()
    {
        _input = ChallengeHelper.ReadAllTextFromResourceFile();
    }

    [Part(1)]
    public void Part01()
    {
        var disk = GetDiskMap(_input);

        int nextFirstEmpty = disk.IndexOf(-1, 0);
        for (var i = disk.Count -1; i > nextFirstEmpty; i--)
        {
            if (disk[i] == -1)
            {
                continue;
            }

            disk[nextFirstEmpty] = disk[i];
            disk[i] = -1;
            nextFirstEmpty = disk.IndexOf(-1, nextFirstEmpty);
        }

        long answer = disk.Select((value, index) => value >= 0 ? (long)(index * value) : 0).Sum();
        Console.WriteLine($"Part 1: {answer}");
    }

    [Part(2)]
    public void Part02()
    {
        var idx = 0;
        int diskIndex = 0;

        var disk = new List<DiskFragment>();
        for (var i = 0; i < _input.Length; i++)
        {
            var c = int.Parse($"{_input[i]}");
            if (i % 2 == 0)
            {
                disk.Add(new DiskFragment(idx++, diskIndex, c));
                diskIndex += c;
            }
            else
            {
                disk.Add(new DiskFragment(-idx, diskIndex, c));
                diskIndex += c;
            }
        }

        foreach (var id in disk.Select(f => f.Id).Where(x => x > 0).OrderDescending())
        {
            var fragment = disk.Single(f => f.Id == id);
            var emptyFragment = disk.FirstOrDefault(e => e.Id < 0 && e.Index < fragment.Index && e.Length >= fragment.Length);
            
            if (emptyFragment == null)
            {
                continue;
            }

            fragment.Index = emptyFragment.Index;
            emptyFragment.Index += fragment.Length;
            emptyFragment.Length -= fragment.Length;
        }


        long answer = disk.Where(f => f.Id >= 0).Sum(f => Calculate(f.Id, f.Index, f.Length));
        Console.WriteLine($"Part 2: {answer}");
    }

    private static long Calculate(int id, int startIndex, int length)
    {
        long sum = 0;
        for (int i = startIndex; i < startIndex + length; i++)
        {
            sum += i * id;
        }
        return sum;
    }

    public class DiskFragment(int id, int index, int length)
    {
        public int Id { get; set; } = id;
        public int Index { get; set; } = index;
        public int Length { get; set; } = length;
    }

    private static List<int> GetDiskMap(string input)
    {
        var id = 0;
        var disk = new List<int>();
        for (var i = 0; i < input.Length; i++)
        {
            var c = int.Parse($"{input[i]}");
            if (i % 2 == 0)
            {
                for (var j = 0; j < c; j++)
                {
                    disk.Add(id);
                }

                id++;
            }
            else
            {
                for (var j = 0; j < c; j++)
                {
                    disk.Add(-1);
                }
            }
        }
        return disk;
    }
}
