using System.Reflection;

namespace AdventOfCode.ConsoleApp;
internal static class AdventOfCodeRunner
{
    public static void Run(AppOptions? options)
    {
        int year;
        int day;

        if (options?.Year is not null && options?.Day is not null)
        {
            year = (int)options.Year;
            day = (int)options.Day;
        }
        else if (options?.Year is not null)
        {
            year = (int)options.Year;
            day = ChooseDay(year);
        }
        else
        {
            year = ChooseYear();
            day = ChooseDay(year);
        }

        var challengeType = _challenges.Where(c => c.Key.Year == year && c.Key.Day == day).Select(c => c.Value).FirstOrDefault();
        if (challengeType is not null)
        {
            RunParts(challengeType);
        }
    }

    private static IDictionary<ChallengeAttribute, Type> _challenges = ChallengeAttribute.GetChallenges();

    private static int ChooseDay(int year)
    {
        int day = 0;
        foreach (var challenge in _challenges.Where(c => c.Key.Year == year))
        {
            Console.WriteLine($"{challenge.Key.Year} {challenge.Key.Day} : {challenge.Value.Name}");
            day = challenge.Key.Day;
        }

        while (true)
        {
            if (day > 0)
                Console.Write("Day (default '{0}'): ", day);
            else
                Console.Write("Day: ");

            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input) && day > 0)
                break;

            if (int.TryParse(input, out day))
            {
                var challenge = _challenges.Where(c => c.Key.Year == year && c.Key.Day == day).Select(c => c.Value).FirstOrDefault();
                if (challenge != null)
                {
                    break;
                }
            }
        }

        return day;
    }

    private static int ChooseYear()
    {
        var years = _challenges.Select(c => c.Key.Year).Distinct();

        Console.WriteLine($"Years: {string.Join(", ", years)}");

        int year;
        while (true)
        {
            Console.Write("Year: ");
            var input = Console.ReadLine();

            if (int.TryParse(input, out year))
            {
                if (years.Contains(year))
                    break;
            }
        }

        return year;
    }

    private static void RunParts(Type type)
    {
        var partMethods = type.GetMethods()
            .Where(mi => mi.CustomAttributes.Any(a => a.AttributeType == typeof(PartAttribute)))
            .Select(mi => new { MethodInfo = mi, PartAttribute = mi.GetCustomAttribute<PartAttribute>()! })
            .OrderBy(ma => ma.PartAttribute.Number)
            .ToList();

        if (!partMethods.Any())
            Console.WriteLine("There are no Advent of Code parts found to execute.");

        var firstSetupMethod = type.GetMethods()
            .Where(mi => mi.CustomAttributes.Any(a => a.AttributeType == typeof(SetupAttribute)))
            .FirstOrDefault();

        Console.WriteLine($"Executing parts for {type.Name}:");

        var instance = Activator.CreateInstance(type);

        if (firstSetupMethod is not null)
        {
            Console.WriteLine($"Executing setup '{firstSetupMethod.Name}'...\n");
            firstSetupMethod.Invoke(instance, null);
        }

        foreach (var part in partMethods)
        {
            Console.WriteLine($"Executing part {part.PartAttribute.Number} '{part.MethodInfo.Name}'...\n");

            part.MethodInfo.Invoke(instance, null);
        }
    }

    private static void RunParts<T>() where T : class, new()
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
