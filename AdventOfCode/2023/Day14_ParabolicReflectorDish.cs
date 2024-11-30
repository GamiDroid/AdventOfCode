using System.Text;

namespace AdventOfCode._2023;

[Challenge(2023, 14, "Parabolic Reflector Dish")]
public class Day14_ParabolicReflectorDish
{
    private readonly char[,] _map;
    private readonly int _lengthX;
    private readonly int _lengthY;

    public Day14_ParabolicReflectorDish()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();

        var map = new char[lines[0].Length, lines.Length];

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                map[x, y] = lines[y][x];
            }
        }

        _map = map;
        _lengthX = _map.GetLength(0);
        _lengthY = _map.GetLength(1);
    }

    [Part(1)]
    public void Part01()
    {
        TiltNorth();
        long sum = CalculateTotalLoad();
        Console.WriteLine($"Part one: answer is {sum}");
    }

    [Part(2)]
    public void Part02()
    {
        Dictionary<string, long> dict = [];
        for (long x = 1; x <= 1_000_000_000; x++)
        {
            TiltNorth();
            TiltWest();
            TiltSouth();
            TiltEast();

            var mapAsString = MapAsString();
            if (dict.TryGetValue(mapAsString, out long p))
            {
                long diff = x - p;
                long remaining = (1_000_000_000 - x) % diff;
                x = 1_000_000_000 - remaining;
            }
            else
            {
                dict[mapAsString] = x;
            }
        }

        var sum = CalculateTotalLoad();

        Console.WriteLine($"Part two: answer is {sum}");
    }

    private long CalculateTotalLoad()
    {
        long sum = 0;
        for (int x = 0; x < _lengthX; x++)
        {
            for (int y = 0; y < _lengthY; y++)
            {
                if (_map[x, y] == 'O')
                {
                    // found a rock
                    sum += _lengthX - y;
                }
            }
        }

        return sum;
    }

    private long CalculateTotalLoadNorth()
    {
        long sum = 0;
        for (int x = 0; x < _lengthX; x++)
        {
            int rockCounterTillPillarOrEnd = 0;
            int pillarIndex = -1;

            for (int y = 0; y < _lengthY; y++)
            {
                if (_map[x, y] == '#')
                {
                    sum += Calculate((_lengthY - 1) - pillarIndex, rockCounterTillPillarOrEnd);

                    // found a pillar
                    pillarIndex = y;
                    rockCounterTillPillarOrEnd = 0;
                }

                if (_map[x, y] == 'O')
                {
                    // found a rock
                    rockCounterTillPillarOrEnd++;
                }
            }

            sum += Calculate((_lengthY - 1) - pillarIndex, rockCounterTillPillarOrEnd);
        }

        return sum;
    }

    private void PrintMap()
    {
        for (int y = 0; y < _lengthY; y++)
        {
            for (int x = 0; x < _lengthX; x++)
            {
                Console.Write(_map[x, y]);
            }
            Console.WriteLine();
        }
    }

    private string MapAsString()
    {
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < _lengthY; y++)
        {
            for (int x = 0; x < _lengthX; x++)
            {
                sb.Append(_map[x, y]);
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    private void TiltNorth()
    {
        for (int x = 0; x < _lengthX; x++)
        {
            int rockCounterTillPillarOrEnd = 0;
            int pillarIndex = -1;

            for (int y = 0; y < _lengthY; y++)
            {
                if (_map[x, y] == '#')
                {
                    ShiftNorth(x, pillarIndex, y, rockCounterTillPillarOrEnd);

                    // found a pillar
                    pillarIndex = y;
                    rockCounterTillPillarOrEnd = 0;
                }

                if (_map[x, y] == 'O')
                {
                    // found a rock
                    rockCounterTillPillarOrEnd++;
                }
            }

            ShiftNorth(x, pillarIndex, _lengthY, rockCounterTillPillarOrEnd);
        }
    }

    private void ShiftNorth(int x, int startY, int endY, int count)
    {
        if (count == 0)
            return;

        for (int y = startY+1; y < endY; y++)
        {
            _map[x, y] = count > 0 ? 'O' : '.';
            count--;
        }
    }

    private void TiltSouth()
    {
        for (int x = 0; x < _lengthX; x++)
        {
            int rockCounterTillPillarOrEnd = 0;
            int pillarIndex = _lengthY;

            for (int y = _lengthY - 1; y >= 0; y--)
            {
                if (_map[x, y] == '#')
                {
                    ShiftSouth(x, pillarIndex, y, rockCounterTillPillarOrEnd);

                    // found a pillar
                    pillarIndex = y;
                    rockCounterTillPillarOrEnd = 0;
                }

                if (_map[x, y] == 'O')
                {
                    // found a rock
                    rockCounterTillPillarOrEnd++;
                }
            }

            ShiftSouth(x, pillarIndex, -1, rockCounterTillPillarOrEnd);
        }
    }

    private void ShiftSouth(int x, int startY, int endY, int count)
    {
        if (count == 0)
            return;

        for (int y = startY-1; y > endY; y--)
        {
            _map[x, y] = count > 0 ? 'O' : '.';
            count--;
        }
    }

    private void TiltWest()
    {
        for (int y = 0; y < _lengthY; y++)
        {
            int rockCounterTillPillarOrEnd = 0;
            int pillarIndex = -1;

            for (int x = 0; x < _lengthX; x++)
            {
                if (_map[x, y] == '#')
                {
                    ShiftWest(y, pillarIndex, x, rockCounterTillPillarOrEnd);

                    // found a pillar
                    pillarIndex = x;
                    rockCounterTillPillarOrEnd = 0;
                }

                if (_map[x, y] == 'O')
                {
                    // found a rock
                    rockCounterTillPillarOrEnd++;
                }
            }

            ShiftWest(y, pillarIndex, _lengthX, rockCounterTillPillarOrEnd);
        }
    }

    private void ShiftWest(int y, int startX, int endX, int count)
    {
        if (count == 0)
            return;

        for (int x = startX+1; x < endX; x++)
        {
            _map[x, y] = count > 0 ? 'O' : '.';
            count--;
        }
    }

    private void TiltEast()
    {
        for (int y = 0; y < _lengthY; y++)
        {
            int rockCounterTillPillarOrEnd = 0;
            int pillarIndex = _lengthX;

            for (int x = _lengthX - 1; x >= 0; x--)
            {
                if (_map[x, y] == '#')
                {
                    ShiftEast(y, pillarIndex, x, rockCounterTillPillarOrEnd);

                    // found a pillar
                    pillarIndex = x;
                    rockCounterTillPillarOrEnd = 0;
                }

                if (_map[x, y] == 'O')
                {
                    // found a rock
                    rockCounterTillPillarOrEnd++;
                }
            }

            ShiftEast(y, pillarIndex, -1, rockCounterTillPillarOrEnd);
        }
    }

    private void ShiftEast(int y, int startX, int endX, int count)
    {
        if (count == 0)
            return;

        for (int x = startX-1; x > endX; x--)
        {
            _map[x, y] = count > 0 ? 'O' : '.';
            count--;
        }
    }

    private static long Calculate(int start, int count)
    {
        long sum = 0;
        for (int i = 0; i < count; i++)
        {
            sum += start;
            start--;
        }

        return sum;
    }
}
