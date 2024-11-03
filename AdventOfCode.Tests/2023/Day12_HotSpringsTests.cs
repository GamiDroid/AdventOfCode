using AdventOfCode._2023;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Tests._2023;
public class Day12_HotSpringsTests
{
    // 3,2,1
    // ?###????????
    // 3,2,1,3,2,1,3,2,1,3,2,1,3,2,1
    // ?###??????????###??????????###??????????###??????????###????????

    [Theory]
    [InlineData(".#.", "#")]
    [InlineData("..#..", "#")]
    [InlineData("#..", "#")]
    [InlineData("..#", "#")]
    [InlineData("..#.#..", "#.#")]
    public void SpringSolver_Trim_Should_Remove_Hot_Springs_From_Front_And_End(string springs, string expected)
    {
        // arrange
        var solver = new Day12_HotSprings.SpringSolver(springs, []);

        // act
        solver.Trim();
        var actual = solver.GetSprings();

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("#??", "1", "#.?")]
    [InlineData("#??", "2", "##.")]
    [InlineData("#??", "3", "###")]
    [InlineData("#?#?", "1,2", "#.##")]
    [InlineData(".#?", "1", ".#.")]
    [InlineData("?#?", "1", "?#.")]
    [InlineData(".#?", "2", ".##")]
    [InlineData(".??..??...?##.", "1,1,3", ".??..??...?##.")]
    [InlineData("?#?#?#?#?#?#?#?", "1,3,1,6", "?#.###.#.######")]
    [InlineData("???#?#?#?#?#?#?", "1,3,1,6", "???#?#?#?#?#?#?")]
    public void SpringSolver_TrySolveLeftToRight_Should_Try_Fill_From_Left_To_Right(string springs, string groupsAsString, string expected)
    {
        // arrange
        var groups = groupsAsString.Split(',').Select(x => int.Parse(x)).ToArray();
        var solver = new Day12_HotSprings.SpringSolver(springs, groups);

        // act
        solver.TrySolveLeftToRight();
        var actual = solver.GetSprings();
        // assert

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("??#", "1", "?.#")]
    [InlineData("??#", "2", ".##")]
    [InlineData("??#", "3", "###")]
    [InlineData("?#?#", "2,1", "##.#")]
    [InlineData("?#.", "1", ".#.")]
    [InlineData("?#?", "1", ".#?")]
    [InlineData("?#.", "2", "##.")]
    [InlineData(".??..??...?##.", "1,1,3", ".??..??...###.")]
    [InlineData("???#?#?#?#?#?#?", "1,3,1,6", "???#?#?#?#####?")]
    public void SpringSolver_TrySolveRightToLeft_Should_Try_Fill_From_Right_To_Left(string springs, string groupsAsString, string expected)
    {
        // arrange
        var groups = groupsAsString.Split(',').Select(x => int.Parse(x)).ToArray();
        var solver = new Day12_HotSprings.SpringSolver(springs, groups);

        // act
        solver.TrySolveRightToLeft();
        var actual = solver.GetSprings();
        // assert

        Assert.Equal(expected, actual);
    }
}
