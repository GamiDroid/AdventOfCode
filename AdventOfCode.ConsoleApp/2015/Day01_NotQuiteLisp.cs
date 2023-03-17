namespace AdventOfCode.ConsoleApp._2015;
internal static class Day01_NotQuiteLisp
{
    public static void ExecutePart01()
    {
        var data = GetTestData();
        var floor = GetFloorNumber(data);

        Console.WriteLine("Floor is {0}", floor);
    }

    public static void ExecutePart02()
    {
        var data = GetTestData();
        var position = GetPositionFirstTimeBasement(data);

        Console.WriteLine("Position is {0}", position);
    }

    public static string GetTestData()
    {
        var rootFolder = App.ProjectRootFolder;
        var filePath = Path.Combine(rootFolder, "2015", "Resources", "NotQuiteLisp.txt");
        return File.ReadAllText(filePath);
    }

    public static int GetFloorNumber(string input)
    {
        int currentFloor = 0;
        foreach (var chr in input)
        {
            if (chr == '(')
                currentFloor++;
            else if (chr == ')')
                currentFloor--;
        }

        return currentFloor;
    }

    public static int GetPositionFirstTimeBasement(string input)
    {
        int currentFloor = 0;
        for (int i = 0; i < input.Length; i++)
        {
            var chr = input[i];
            if (chr == '(')
                currentFloor++;
            else if (chr == ')')
                currentFloor--;

            if (currentFloor == -1)
                return i + 1;
        }

        return -1;
    }
}
