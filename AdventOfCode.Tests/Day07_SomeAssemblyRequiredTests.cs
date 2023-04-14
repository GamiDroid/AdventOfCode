using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode.ConsoleApp._2015.Day07_SomeAssemblyRequired;

namespace AdventOfCode.Tests;
public class Day07_SomeAssemblyRequiredTests
{
    [Fact]
    public void Example_ShouldHaveSameOutput()
    {
        // Arrange
        WireSet wires = new();

        // Act
        wires.Connect("123 -> x");
        wires.Connect("456 -> y");
        wires.Connect("x AND y -> d");
        wires.Connect("x OR y -> e");
        wires.Connect("x LSHIFT 2 -> f");
        wires.Connect("y RSHIFT 2 -> g");
        wires.Connect("NOT x -> h");
        wires.Connect("NOT y -> i");

        // Assert
        Assert.Equal(72, wires.GetSignal("d"));
        Assert.Equal(507, wires.GetSignal("e"));
        Assert.Equal(492, wires.GetSignal("f"));
        Assert.Equal(114, wires.GetSignal("g"));
        Assert.Equal(65412, wires.GetSignal("h"));
        Assert.Equal(65079, wires.GetSignal("i"));
        Assert.Equal(123, wires.GetSignal("x"));
        Assert.Equal(456, wires.GetSignal("y"));
    }
}
