using Xunit;
using static AdventOfCode._2015.Day23_OpeningTheTuringLock;

namespace AdventOfCode.Tests;

public class Day23_OpeningTheTuringLockTests
{
    [Fact]
    public void InstructionParser_Parse_ShouldReturnHalfInstruction()
    {
        // Arrange
        // Act
        var instruction = InstructionParser.Parse("hlf a");

        // Assert
        Assert.IsType<HalfInstruction>(instruction);
        Assert.Equal('a', ((HalfInstruction)instruction).Register);
    }

    [Fact]
    public void InstructionParser_Parse_ShouldReturnTripleInstruction()
    {
        // Arrange
        // Act
        var instruction = InstructionParser.Parse("tpl a");

        // Assert
        Assert.IsType<TripleInstruction>(instruction);
        Assert.Equal('a', ((TripleInstruction)instruction).Register);
    }

    [Fact]
    public void InstructionParser_Parse_ShouldReturnIncrementInstruction()
    {
        // Arrange
        // Act
        var instruction = InstructionParser.Parse("inc a");

        // Assert
        Assert.IsType<IncrementInstruction>(instruction);
        Assert.Equal('a', ((IncrementInstruction)instruction).Register);
    }

    [Theory]
    [InlineData("jmp +0", 0)]
    [InlineData("jmp +1", 1)]
    [InlineData("jmp -1", -1)]
    [InlineData("jmp +12", 12)]
    [InlineData("jmp -12", -12)]
    [InlineData("jmp +123", 123)]
    [InlineData("jmp -123", -123)]
    public void InstructionParser_Parse_ShouldReturnJumpInstruction(string instructionText, int expectedOffset)
    {
        // Arrange
        // Act
        var instruction = InstructionParser.Parse(instructionText);

        // Assert
        Assert.IsType<JumpInstruction>(instruction);
        Assert.Equal(expectedOffset, ((JumpInstruction)instruction).Offset);
    }

    [Theory]
    [InlineData("jie a +0", 0, 'a')]
    [InlineData("jie a +1", 1, 'a')]
    [InlineData("jie a -1", -1, 'a')]
    [InlineData("jie a +12", 12, 'a')]
    [InlineData("jie a -12", -12, 'a')]
    [InlineData("jie a +123", 123, 'a')]
    [InlineData("jie a -123", -123, 'a')]
    public void InstructionParser_Parse_ShouldReturnJumpEvenInstruction(string instructionText, int expectedOffset, char expectedRegister)
    {
        // Arrange
        // Act
        var instruction = InstructionParser.Parse(instructionText);

        // Assert
        Assert.IsType<JumpEvenInstruction>(instruction);
        Assert.Equal(expectedOffset, ((JumpEvenInstruction)instruction).Offset);
        Assert.Equal(expectedRegister, ((JumpEvenInstruction)instruction).Register);
    }

    [Theory]
    [InlineData("jio a +0", 0, 'a')]
    [InlineData("jio a +1", 1, 'a')]
    [InlineData("jio a -1", -1, 'a')]
    [InlineData("jio a +12", 12, 'a')]
    [InlineData("jio a -12", -12, 'a')]
    [InlineData("jio a +123", 123, 'a')]
    [InlineData("jio a -123", -123, 'a')]
    public void InstructionParser_Parse_ShouldReturnJumpOneInstruction(string instructionText, int expectedOffset, char expectedRegister)
    {
        // Arrange
        // Act
        var instruction = InstructionParser.Parse(instructionText);

        // Assert
        Assert.IsType<JumpOneInstruction>(instruction);
        Assert.Equal(expectedOffset, ((JumpOneInstruction)instruction).Offset);
        Assert.Equal(expectedRegister, ((JumpOneInstruction)instruction).Register);
    }

    [Fact]
    public void Computer_GetRegister_ShouldReturnZeroWhenNotDefined()
    {
        // Arrange
        var computer = new Computer(Array.Empty<IInstruction>());

        // Act
        var actual = computer.GetRegister('a');

        // Assert
        Assert.Equal((uint)0, actual);
    }

    [Theory]
    [InlineData('a', 1)]
    [InlineData('a', uint.MinValue)]
    [InlineData('a', uint.MaxValue)]
    public void Computer_GetRegister_ShouldReturnSetRegisterValue(char register, uint expected)
    {
        // Arrange
        var computer = new Computer(Array.Empty<IInstruction>());
        computer.SetRegister(register, expected);

        // Act
        var actual = computer.GetRegister(register);

        // Assert
        Assert.Equal(expected, actual);
    }
}