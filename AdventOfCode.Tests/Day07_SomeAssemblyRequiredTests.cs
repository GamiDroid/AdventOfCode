using static AdventOfCode._2015.Day07_SomeAssemblyRequired;

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
        Assert.Equal(72, wires.Signal("d"));
        Assert.Equal(507, wires.Signal("e"));
        Assert.Equal(492, wires.Signal("f"));
        Assert.Equal(114, wires.Signal("g"));
        Assert.Equal(65412, wires.Signal("h"));
        Assert.Equal(65079, wires.Signal("i"));
        Assert.Equal(123, wires.Signal("x"));
        Assert.Equal(456, wires.Signal("y"));
    }
}
