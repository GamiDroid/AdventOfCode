namespace AdventOfCode.ConsoleApp._2015;
internal class Day02_IWasToldThereWouldBeNoMath
{
    public static void ExecutePart01()
    {
        var dimentions = GetTestData();

        var totalPaper = 0;
        foreach (var box in dimentions)
        {
            var paperNeeded = GetSquareFeetOfWrappingPaper(box.Length, box.Width, box.Height);
            totalPaper += paperNeeded;
        }

        Console.WriteLine("Total paper needed: {0}", totalPaper);
    }

    public static BoxDimentions[] GetTestData()
    {
        var rootFolder = App.ProjectRootFolder;
        var filePath = Path.Combine(rootFolder, "2015", "Resources", "IWasToldThereWouldBeNoMath.txt");
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

    public record struct BoxDimentions(int Length, int Width, int Height);
}
