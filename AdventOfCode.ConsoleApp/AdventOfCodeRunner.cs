using System.Reflection;

namespace AdventOfCode.ConsoleApp;
internal static class AdventOfCodeRunner
{
    public static void RunParts<T>() where T : class, new()
    {
        var partMethods = typeof(T).GetMethods()
            .Where(mi => mi.CustomAttributes.Any(a => a.AttributeType == typeof(PartAttribute)))
            .Select(mi => new { MethodInfo = mi, PartAttribute = mi.GetCustomAttribute<PartAttribute>()! })
            .OrderBy(ma => ma.PartAttribute.Number)
            .ToList();

        if (!partMethods.Any())
            Console.WriteLine("There are no Advent of Code parts found to execute.");

        var firstSetupMethod = typeof(T).GetMethods()
            .Where(mi => mi.CustomAttributes.Any(a => a.AttributeType == typeof(SetupAttribute)))
            .FirstOrDefault();

        Console.WriteLine($"Executing parts for {typeof(T).Name}:");

        var instance = Activator.CreateInstance<T>();

        if (firstSetupMethod is not null)
        {
            Console.WriteLine($"Executing setup '{firstSetupMethod.Name}'...\n");
            firstSetupMethod.Invoke(instance, null);
        }

        foreach (var part in partMethods)
        {
            Console.WriteLine($"Executing part {part.PartAttribute.Number} '{part.MethodInfo.Name}'...\n");

            T? ins = null;
            if (!part.MethodInfo.IsStatic)
                ins = instance;

            part.MethodInfo.Invoke(ins, null);
        }
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
internal class PartAttribute : Attribute
{
    public PartAttribute(int number)
    {
        Number = number;
    }

    public int Number { get; init; }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
internal class SetupAttribute : Attribute
{
    public SetupAttribute()
    {
    }
}

