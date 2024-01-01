﻿using AdventOfCode._2023;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Tests._2023;
public class Day04_ScratchcardsTests
{
    [Fact]
    public void CountWinningCards_ShouldReturnAmountOfWinningCards()
    {
        // arrange
        var card = new Day04_Scratchcards.Scratchcard(1, 
            [41, 48, 83, 86, 17], 
            [83, 86, 6, 31, 17, 9, 48, 53]);

        // act
        var actual = Day04_Scratchcards.CountWinningNumbers(card);

        // assert
        Assert.Equal(4, actual);
    }

    [Fact]
    public void CountCards_ShouldReturnAmountOfWinningCards()
    {
        // arrange
        var cards = new Day04_Scratchcards.Scratchcard[]
        {
            new(1, [41, 48, 83, 86, 17], [83, 86, 6, 31, 17, 9, 48, 53]),
            new(2, [13, 32, 20, 16, 61], [61, 30, 68, 82, 17, 32, 24, 19]),
            new(3, [1, 21, 53, 59, 44], [69, 82, 63, 72, 16, 21, 14, 1]),
            new(4, [41, 92, 73, 84, 69], [59, 84, 76, 51, 58, 5, 54, 83]),
            new(5, [87, 83, 26, 28, 32], [88, 30, 70, 12, 93, 22, 82, 36]),
            new(6, [31, 18, 13, 56, 72], [74, 77, 10, 23, 35, 67, 36, 11]),
        };

        // act
        var actual = Day04_Scratchcards.CountCards(cards);

        // assert
        Assert.Equal(30, actual);
    }
}
