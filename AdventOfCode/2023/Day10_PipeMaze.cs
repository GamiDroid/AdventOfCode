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
        var state = new State(startPoint);

        var points = GetNextPossiblePoints(startPoint);
        state.Move(points[0]);

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
            return '#';

        return _maze[point.X, point.Y];
    }


    [Part(2)]
    public void Part02()
    {
        var islands = FindGroundIslands();

        // use THE SHOELACE ALGORITHM to find the area inside the pipe maze.
        // https://www.101computing.net/the-shoelace-algorithm/

        var startPoint = GetStartPoint();

        var state = new State(startPoint);

        var vertices = FindVertices(startPoint, ref state);
        var area = CalculateArea(vertices);

        // use PICK'S THEOREM to find the interior points inside the lace.
        // https://en.wikipedia.org/wiki/Pick%27s_theorem
        // A = i + b/2 − 1
        // A = area
        // i = iterior points
        // b = boundry points
        // i = A - b/2 - 1

        var interiorPoints = area - (state.Steps / 2) + 1;

        Console.WriteLine($"interior points: {interiorPoints}");
    }

    private int CalculateArea(Point[] vertices)
    {
        int sum1 = 0, sum2 = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            int i2 = i + 1;
            if (i2 == vertices.Length)
                i2 = 0;

            sum1 += vertices[i].X * vertices[i2].Y;
            sum2 += vertices[i].Y * vertices[i2].X;
        }

        var area = Math.Abs(sum1 - sum2) / 2;

        return area;
    }

    private Point[] FindVertices(Point startPoint, ref State state)
    {
        List<Point> vertices = [];

        var points = GetNextPossiblePoints(state.CurrentPoint);

        // check if startPoint is a vertices
        if (points[0].X != points[1].X && points[0].Y != points[1].Y)
            vertices.Add(state.CurrentPoint);

        state.Move(points[0]);

        while (true)
        {
            var currentTile = GetTile(state.CurrentPoint);
            if (currentTile is 'L' or 'F' or 'J' or '7')
                vertices.Add(state.CurrentPoint);

            var nextPoint = GetNextPoint(state);
            state.Move(nextPoint);

            if (nextPoint == startPoint)
            {
                break;
            }
        }

        return [.. vertices];
    }

    private Point[][] FindGroundIslands()
    {
        List<Point[]> islands = [];
        HashSet<Point> visitedGrounds = [];

        var yLength = _maze.GetLength(1);
        var xLength = _maze.GetLength(0);

        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLength; x++)
            {
                if (visitedGrounds.Contains(new Point(x, y)))
                    continue;

                if (_maze[x, y].Equals('.'))
                {
                    HashSet<Point> island = [];
                    GetIsland(new Point(x, y), island, visitedGrounds);
                    islands.Add([.. island]);
                }
            }
        }

        return [.. islands];
    }

    private void GetIsland(Point point, HashSet<Point> island, HashSet<Point> visited)
    {
        if (!visited.Add(point))
            return;

        island.Add(point);

        Point[] neigbors = [
            point.GetNorth(), 
            point.GetEast(), 
            point.GetSouth(), 
            point.GetWest()];

        foreach (var neigbor in neigbors)
        {
            if (GetTile(neigbor) == '.')
                GetIsland(neigbor, island, visited);
        }
    }

    private record struct Point(int X, int Y)
    {
        public readonly Point GetNorth() => new(X, Y - 1);
        public readonly Point GetSouth() => new(X, Y + 1);
        public readonly Point GetEast() => new(X + 1, Y);
        public readonly Point GetWest() => new(X - 1, Y);
    }

    private class State(Point currentPoint)
    {
        public Point CurrentPoint { get; private set; } = currentPoint;
        public Point PreviousPoint { get; private set; }
        public int Steps { get; private set; }

        public void Move(Point nextPoint)
        {
            PreviousPoint = CurrentPoint;
            CurrentPoint = nextPoint;
            Steps++;
        }
    }
}
