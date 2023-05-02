using System.Reflection;

namespace AdventOfCode;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ChallengeAttribute : Attribute
{
    public ChallengeAttribute(int year, int day)
    {
        Year = year;
        Day = day;
    }

    public int Year { get; set; }
    public int Day { get; set; }

    public static IDictionary<ChallengeAttribute, Type> GetChallenges()
    {
        var challengeTypes = Assembly.GetAssembly(typeof(ChallengeAttribute))!.DefinedTypes
            .Where(t => t.CustomAttributes.Any(a => a.AttributeType == typeof(ChallengeAttribute)))
            .Select(t => new { Type = t, Attribute = t.GetCustomAttribute<ChallengeAttribute>()! })
            .ToList();

        var challenges = new Dictionary<ChallengeAttribute, Type>();

        foreach (var challengeType in challengeTypes)
            challenges[challengeType.Attribute] = challengeType.Type;

        return challenges;
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class PartAttribute : Attribute
{
    public PartAttribute(int number)
    {
        Number = number;
    }

    public int Number { get; init; }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class SetupAttribute : Attribute
{
    public SetupAttribute()
    {
    }
}
