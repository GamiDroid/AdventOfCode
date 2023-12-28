
using System.Text.RegularExpressions;

namespace AdventOfCode;
public static class RegexHelper
{
    public static int ToInt32(this Group group) => int.Parse(group.ValueSpan);
    public static uint ToUInt32(this Group group) => uint.Parse(group.ValueSpan);
    public static long ToInt64(this Group group) => long.Parse(group.ValueSpan);
    public static ulong ToUInt64(this Group group) => ulong.Parse(group.ValueSpan);
}
