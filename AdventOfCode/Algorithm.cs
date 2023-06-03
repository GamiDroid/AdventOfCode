namespace AdventOfCode;
public static class Algorithm
{
    /// <summary>
    /// Get Permutations of an list of items.
    /// </summary>
    public static T[][] GetPermutations<T>(this ICollection<T> list, T[]? current = null, T[][]? permutations = null)
    {
        current ??= Array.Empty<T>();
        permutations ??= Array.Empty<T[]>();

        if (current.Length == list.Count)
        {
            var newPermutations = new List<T[]>(permutations) { current }.ToArray();
            return newPermutations;
        }
        else
        {
            foreach (var name in list)
            {
                if (!current.Contains(name))
                {
                    var newCurrent = new List<T>(current) { name }.ToArray();
                    permutations = GetPermutations(list, newCurrent, permutations);
                }
            }
        }

        return permutations;
    }
}
