
using System.Text.RegularExpressions;

namespace AdventOfCode;
public static class RegexHelper
{
    public static int ToInt32(this Group group) => int.Parse(group.ValueSpan);
}
