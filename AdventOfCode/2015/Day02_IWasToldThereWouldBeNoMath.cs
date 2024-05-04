namespace AdventOfCode._2015;

[Challenge(2015, 2)]
internal class Day02_IWasToldThereWouldBeNoMath
{
    private readonly BoxDimentions[] _testData;

    public Day02_IWasToldThereWouldBeNoMath()
    {
        _testData = GetTestData();
    }

    [Part(1)]
    public void ExecutePart01()
    {
        var dimentions = _testData;

        var totalPaper = 0;
        foreach (var box in dimentions)
        {
            var paperNeeded = GetSquareFeetOfWrappingPaper(box.Length, box.Width, box.Height);
            totalPaper += paperNeeded;
        }

        Console.WriteLine("Total paper needed: {0}", totalPaper);
    }

    [Part(2)]
    public void ExecutePart02()
    {
        var dimentions = _testData;

        var totalFeetRibbon = 0;
        foreach (var box in dimentions)
        {
            var feetNeeded = GetFeetOfRibbon(box.Length, box.Width, box.Height);
            totalFeetRibbon += feetNeeded;
        }

        Console.WriteLine("Total feet ribbon needed: {0}", totalFeetRibbon);
    }

    private static BoxDimentions[] GetTestData()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var lines = File.ReadAllLines(filePath);

        var dimentions = new List<BoxDimentions>();
        foreach (var line in lines)
        {
            var values = line.Split('x');

            var sl = values[0];
            var sw = values[1];
            var sh = values[2];

            var l = int.Parse(sl);
            var w = int.Parse(sw);
            var h = int.Parse(sh);

            dimentions.Add(new BoxDimentions(l, w, h));
        }

        return dimentions.ToArray();
    }

    public static int GetSquareFeetOfWrappingPaper(int l, int w, int h)
    {
        var squareFeetBox = 2 * l * w + 2 * w * h + 2 * h * l;

        var list = new int[] { l, w, h };
        var ordered = list.Order().ToArray();

        var o0 = ordered[0];
        var o1 = ordered[1];

        var rest = o0 * o1;

        return squareFeetBox + rest;
    }

    public static int GetFeetOfRibbon(int l, int w, int h)
    {
        var list = new int[] { l, w, h };
        var ordered = list.Order().ToArray();

        var o0 = ordered[0];
        var o1 = ordered[1];

        var wrap = o0 * 2 + o1 * 2;
        var bow = l * w * h;

        return wrap + bow;
    }

    public record struct BoxDimentions(int Length, int Width, int Height);
}
