using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2015;
[Challenge(2015, 25)]
internal class Day25_LetItSnow
{
    private readonly static int s_codeRow = 3010;
    private readonly static int s_codeColumn = 3019;

    [Part(1)]
    public void Part01()
    {
        var prevPos = new Position(1, 1);
        ulong prevValue = 20151125;
        while (true)
        {
            var nextPos = GetNextPosition(prevPos);
            prevValue = Solve(prevValue);

            if (nextPos.Row == s_codeRow && nextPos.Column == s_codeColumn)
                break;

            prevPos = nextPos;
        }

        Console.WriteLine($"Value at pos is '{prevValue}'");
    }

    private static ulong Solve(ulong prevValue)
    {
        return (prevValue * 252533) % 33554393;
    }

    private static Position GetNextPosition(Position pos)
    {
        int row, col;
        if (pos.Row == 1)
        {
            row = pos.Column + 1;
            col = 1;
        }
        else
        {
            row = pos.Row - 1;
            col = pos.Column + 1;
        }

        return new Position(row, col);
    }

    record struct Position(int Row, int Column);
}
