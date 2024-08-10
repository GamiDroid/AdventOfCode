namespace AdventOfCode._2023;
[Challenge(2023, 10, "Pipe Maze")]
internal class Day10_PipeMaze
{
    private readonly char[,] _maze;

    public Day10_PipeMaze()
    {
        var lines = ChallengeHelper.ReadAllLinesFromResourceFile();

        var lineLength = lines[0].Length;
        _maze = new char[lines[0].Length, lines.Length];

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lineLength; x++)
            {
                _maze[x, y] = lines[y][x];
            }
        }
    }

    [Part(1)]
    public void Part01()
    {
        var startPoint = GetStartPoint();

        var points = GetNextPossiblePoints(startPoint);

        var state = new State(points[0], startPoint, 1);

        bool isBackToStart = false;
        while (!isBackToStart)
        {
            var nextPoint = GetNextPoint(state);
            state.Move(nextPoint);
            if (nextPoint == startPoint)
            {
                isBackToStart = true;
            }
        }

        var steps = state.Steps / 2 + (state.Steps % 2);

        Console.WriteLine($"It took {steps} steps to get to the other side");
    }

    private Point GetNextPoint(State state)
    {
        var tile = GetTile(state.CurrentPoint);

        Point[] points = tile switch
        {
            '|' => [state.CurrentPoint.GetNorth(), state.CurrentPoint.GetSouth()],
            '-' => [state.CurrentPoint.GetWest(), state.CurrentPoint.GetEast()],
            'L' => [state.CurrentPoint.GetNorth(), state.CurrentPoint.GetEast()],
            'F' => [state.CurrentPoint.GetSouth(), state.CurrentPoint.GetEast()],
            'J' => [state.CurrentPoint.GetNorth(), state.CurrentPoint.GetWest()],
            '7' => [state.CurrentPoint.GetSouth(), state.CurrentPoint.GetWest()],
            _ => throw new ArgumentException("State is not on a correct tile.")
        };

        return points[0].Equals(state.PreviousPoint) ? points[1] : points[0];
    }

    private Point[] GetNextPossiblePoints(Point point)
    {
        (Func<Point, Point> GetPoint, char[] NextOptions)[] directions = [
             (p => p.GetNorth(), ['|', '7', 'F'] ),
             (p => p.GetSouth(), ['|' , 'L' , 'J'] ),
             (p => p.GetEast(), ['-' , '7' , 'J'] ),
             (p => p.GetWest(), ['-' , 'L' , 'F'] )];


        var points = new List<Point>();

        foreach (var (GetPoint, NextOptions) in directions)
        {
            var nextPoint = GetPoint(point);
            var tile = GetTile(nextPoint);
            if (NextOptions.Contains(tile))
                points.Add(nextPoint);
        }

        return [.. points];
    }

    private Point GetStartPoint()
    {
        var yLength = _maze.GetLength(1);
        var xLength = _maze.GetLength(0);

        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLength; x++)
            {
                if (_maze[x, y].Equals('S'))
                    return new Point(x, y);
            }
        }

        throw new ArgumentException("Start point in maze could not be found");
    }

    private char GetTile(Point point)
    {
        if (point.X < 0 || point.Y < 0 ||
            point.X >= _maze.GetLength(0) ||
            point.Y >= _maze.GetLength(1))
            return '.';

        return _maze[point.X, point.Y];
    }

    private record struct Point(int X, int Y)
    {
        public readonly Point GetNorth() => new(X, Y - 1);
        public readonly Point GetSouth() => new(X, Y + 1);
        public readonly Point GetEast() => new(X + 1, Y);
        public readonly Point GetWest() => new(X - 1, Y);
    }

    private class State(
        Point currentPoint, 
        Point previousPoint, 
        int steps)
    {
        public Point CurrentPoint { get; private set; } = currentPoint;
        public Point PreviousPoint { get; private set; } = previousPoint;
        public int Steps { get; private set; } = steps;

        public void Move(Point nextPoint)
        {
            PreviousPoint = CurrentPoint;
            CurrentPoint = nextPoint;
            Steps++;
        }
    }
}
