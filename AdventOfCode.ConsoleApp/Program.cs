using AdventOfCode.ConsoleApp._2015;

App.SetProjectRootFolder();

try
{
    Day05_DoesntHeHaveInternElvesForThis.ExecutePart02();
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
