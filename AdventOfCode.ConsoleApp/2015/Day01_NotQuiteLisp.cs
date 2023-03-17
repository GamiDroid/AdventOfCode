using System;
namespace AdventOfCode.ConsoleApp._2015;
internal static class Day01_NotQuiteLisp
{
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
}
