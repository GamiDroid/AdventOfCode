using AdventOfCode.ConsoleApp._2015;

App.SetProjectRootFolder();

try
{
    var data = Day01_NotQuiteLisp.GetTestData();
    var floor = Day01_NotQuiteLisp.GetFloorNumber(data);

    Console.WriteLine("Floor is {0}", floor);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
finally
{
    Console.WriteLine("End of program. Press any key to exit.");
    Console.ReadKey();
}
