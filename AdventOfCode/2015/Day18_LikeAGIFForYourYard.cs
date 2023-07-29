using System.Diagnostics.Metrics;

namespace AdventOfCode._2015;

[Challenge(2015, 18)]
internal class Day18_LikeAGIFForYourYard
{
    private Lights _lights = new(0, 0);

    [Setup]
    public void Setup()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var lines = File.ReadAllLines(filePath);

        var lights = new Lights(100, 100);
        for (int y = 0; y < lines.Length; y++)
        {
            var line = lines[y].AsSpan();
            for (int x = 0; x < line.Length; x++) 
                lights[x, y] = line[x] == '#';
        }

        _lights = lights;
    }

    [Part(1)]
    public void Part01()
    {
        Console.WriteLine("Initial there are {0} lights on.", _lights.CountLightOn());

        _lights.ElapseTime(100);

        Console.WriteLine("After 100 times there are {0} lights on.", _lights.CountLightOn());
    }

    public class Lights
    {
        public Lights(int x, int y)
        {
            _maxX = x;
            _maxY = y;

            _lights = new bool[_maxX, _maxY];
        }

        public bool this[int x, int y] 
        { 
            get => _lights[x, y]; 
            set => _lights[x, y] = value; 
        }

        public void ElapseTime(int time) 
        {
            for (int i = 0; i < time; i++)
                ElapseTime();
        }

        public void ElapseTime()
        {
            foreach (var (x, y) in GetLightsToToggle())
                ToggleLight(x, y);
        }

        public int CountLightOn()
        {
            var count = 0;
            
            for (int y = 0; y < _maxY; y++)
            {
                for (int x = 0; x < _maxX; x++)
                {
                    if (_lights[x, y])
                        count++;
                }
            }

            return count;
        }

        private void ToggleLight(int x, int y)
        {
            var curr = _lights[x, y];
            _lights[x, y] = !curr;
        }

        private IEnumerable<(int x, int y)> GetLightsToToggle()
        {
            var lightsToToggle = new List<(int x, int y)>();
            for (int y = 0; y < _maxY; y++)
            {
                for (int x = 0; x < _maxX; x++)
                {
                    if (LightMustBeToggled(x, y))
                        lightsToToggle.Add((x , y));
                }
            }

            return lightsToToggle;
        }

        private bool LightMustBeToggled(int x, int y)
        {
            var lightIsOn = _lights[x, y];

            var adjancentLightsOnCount = CountAdjacentLightsOn(x, y);

            return lightIsOn ? 
                adjancentLightsOnCount != 2 && adjancentLightsOnCount != 3 : 
                adjancentLightsOnCount == 3;
        }

        private int CountAdjacentLightsOn(int x, int y)
        {
            var count = 0;

            for (int y1 = -1; y1 <= 1; y1++)
            {
                var adjacentY = y + y1;
                if (adjacentY < 0 || adjacentY >= _maxY)
                    continue;

                for (int x1 = -1; x1 <= 1; x1++)
                {
                    if (y1 == 0 && x1 == 0)
                        continue;

                    var adjacentX = x + x1;
                    if (adjacentX < 0 || adjacentX >= _maxX)
                        continue;

                    if (_lights[adjacentX, adjacentY])
                        count++;
                }
            }

            return count;
        }

        private readonly bool[,] _lights;

        private readonly int _maxX = 100;
        private readonly int _maxY = 100;
    }
}
