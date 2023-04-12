using AdventOfCode.ConsoleApp;
using AdventOfCode.ConsoleApp._2015;

App.SetProjectRootFolder();

try
{
    //AdventOfCodeRunner.RunParts<Day01_NotQuiteLisp>();
    //AdventOfCodeRunner.RunParts<Day02_IWasToldThereWouldBeNoMath>();
    //AdventOfCodeRunner.RunParts<Day03_PerfectlySphericalHousesInAVacuum>();
    //AdventOfCodeRunner.RunParts<Day04_TheIdealStockingStuffer>();
    //AdventOfCodeRunner.RunParts<Day05_DoesntHeHaveInternElvesForThis>();
    //AdventOfCodeRunner.RunParts<Day06_ProbablyAFireHazard>();
    AdventOfCodeRunner.RunParts<Day07_SomeAssemblyRequired>();
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
