namespace AdventOfCode._2015;

[Challenge(2015, 1)]
internal class Day01_NotQuiteLisp
{
    private readonly string _testData;

    public Day01_NotQuiteLisp()
    {
        _testData = GetTestData();
    }

    [Part(1)]
    public void ExecutePart01()
    {
        var data = _testData;
        var floor = GetFloorNumber(data);

        Console.WriteLine("Floor is {0}", floor);
    }

    [Part(2)]
    public void ExecutePart02()
    {
        var data = _testData;
        var position = GetPositionFirstTimeBasement(data);

        Console.WriteLine("Position is {0}", position);
    }

    public static string GetTestData()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
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
