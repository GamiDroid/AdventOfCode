
namespace AdventOfCode;
internal record struct Location(int X, int Y)
{
    internal readonly Location Move(int dx, int dy) => new(X + dx, Y + dy);
}