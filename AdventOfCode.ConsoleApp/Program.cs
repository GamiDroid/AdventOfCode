using AdventOfCode.ConsoleApp;

App.Startup(args);

try
{
    AdventOfCodeRunner.Run(App.Options);
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
