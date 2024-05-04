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

        var challengeType = s_challenges.Where(c => c.Key.Year == year && c.Key.Day == day).Select(c => c.Value).FirstOrDefault();
        if (challengeType is not null)
        {
            RunParts(challengeType);
        }
    }

    private static readonly IDictionary<ChallengeAttribute, TypeInfo> s_challenges = ChallengeAttribute.GetChallenges();

    private static int ChooseDay(int year)
    {
        int day = 0;
        foreach (var challenge in s_challenges.Where(c => c.Key.Year == year))
        {
            var challengeTitle = challenge.Key.Title;
            if (string.IsNullOrWhiteSpace(challengeTitle))
                challengeTitle = challenge.Value.Name;

            Console.WriteLine($"{challenge.Key.Year} --- Day {challenge.Key.Day}: {challengeTitle}");
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
                var challenge = s_challenges.Where(c => c.Key.Year == year && c.Key.Day == day).Select(c => c.Value).FirstOrDefault();
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
        var years = s_challenges.Select(c => c.Key.Year).Distinct();

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

        Console.WriteLine($"Executing parts for {type.Name}:");

        foreach (var part in partMethods)
        {
            var instance = Activator.CreateInstance(type);

            Console.WriteLine($"Executing part {part.PartAttribute.Number} '{part.MethodInfo.Name}'...\n");

            var result = part.MethodInfo.Invoke(instance, null);

            if (result is not null)
                Console.WriteLine($"Puzzle result: {result}");
        }
    }
}
