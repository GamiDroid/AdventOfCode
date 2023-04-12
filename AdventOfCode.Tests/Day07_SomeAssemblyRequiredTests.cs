using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode.ConsoleApp._2015.Day07_SomeAssemblyRequired;

namespace AdventOfCode.Tests;
public class Day07_SomeAssemblyRequiredTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(65535)]
    public void AssignConstant_ShouldHaveTheConstantOutput(ushort expected)
    {
        // Arrange
        WireSet wires = new();

        // Act
        wires.DoCommand($"{expected} -> a");
        var actual = wires["a"];

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(65535)]
    public void AssignOtherWire_ShouldHaveSameOutput(ushort expected)
    {
        // Arrange
        WireSet wires = new();

        // Act
        wires.DoCommand($"{expected} -> a");
        wires.DoCommand("a -> b");
        var actual = wires["b"];

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void EmptyWire_ShouldReturnZero()
    {
        // Arrange
        WireSet wires = new();

        // Act
        var actual = wires["a"];

        // Assert
        Assert.Equal(0, actual);
    }

    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1")]
    [InlineData(2, "10")]
    [InlineData(4, "100")]
    [InlineData(8, "1000")]
    [InlineData(65535, "1111111111111111")]
    public void OutputString_ShouldReturnUShortBits(ushort input, string expected)
    {
        // Arrange
        WireSet wires = new();

        // Act
        wires.DoCommand($"{input} -> a");
        var actual = wires.OutputString("a");

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Example_ShouldHaveSameOutput()
    {
        // Arrange
        WireSet wires = new();

        // Act
        wires.DoCommand("123 -> x");
        wires.DoCommand("456 -> y");
        wires.DoCommand("x AND y -> d");
        wires.DoCommand("x OR y -> e");
        wires.DoCommand("x LSHIFT 2 -> f");
        wires.DoCommand("y RSHIFT 2 -> g");
        wires.DoCommand("NOT x -> h");
        wires.DoCommand("NOT y -> i");

        // Assert
        Assert.Equal(72, wires["d"]);
        Assert.Equal(507, wires["e"]);
        Assert.Equal(492, wires["f"]);
        Assert.Equal(114, wires["g"]);
        Assert.Equal(65412, wires["h"]);
        Assert.Equal(65079, wires["i"]);
        Assert.Equal(123, wires["x"]);
        Assert.Equal(456, wires["y"]);
    }
}
