using System.Reflection;

namespace AdventOfCode;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ChallengeAttribute : Attribute
{
    public ChallengeAttribute(int year, int day, string? title = null)
    {
        Year = year;
        Day = day;
        Title = title;
    }

    public int Year { get; set; }
    public int Day { get; set; }
    public string? Title { get; set; }

    public static IDictionary<ChallengeAttribute, TypeInfo> GetChallenges()
    {
        var challenges = Assembly.GetAssembly(typeof(ChallengeAttribute))!.DefinedTypes
            .Where(t => t.CustomAttributes.Any(a => a.AttributeType == typeof(ChallengeAttribute)))
            .Select(t => new { Type = t, Attribute = t.GetCustomAttribute<ChallengeAttribute>()! })
            .ToDictionary(c => c.Attribute, c => c.Type);

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
