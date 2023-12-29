using static AdventOfCode._2015.Day15_ScienceForHungryPeople;

namespace AdventOfCode.Tests;
public class Day15_ScienceForHungryPeopleTests
{
    [Fact]
    public void Recipe_Create_ShouldCreateNewEmptyRecipe()
    {
        // Arrange
        var ingrients = new Ingredient[]
        {
            new Ingredient("A", 0, 0, 0, 0, 0),
            new Ingredient("B", 0, 0, 0, 0, 0),
        };

        // Act
        var recipe = new Recipe(ingrients);

        // Assert
        Assert.Equal(0u, recipe.GetAmount("A"));
        Assert.Equal(0u, recipe.GetAmount("B"));
    }

    [Fact]
    public void Recipe_SetAmount_ShouldSetTheAmountOfAIngredient()
    {
        // Arrange
        var ingrients = new Ingredient[]
        {
            new Ingredient("A", 0, 0, 0, 0, 0),
            new Ingredient("B", 0, 0, 0, 0, 0),
        };

        var expected = 12u;

        // Act
        var recipe = new Recipe(ingrients);
        recipe.SetAmount("A", expected);

        // Assert
        Assert.Equal(expected, recipe.GetAmount("A"));
        Assert.Equal(0u, recipe.GetAmount("B"));
    }

    [Fact]
    public void Recipe_GetAmount_ShouldGetTheAmountOfAIngredient()
    {
        // Arrange
        var ingrients = new Ingredient[]
        {
            new Ingredient("A", 0, 0, 0, 0, 0),
        };

        var expected = 12u;

        // Act
        var recipe = new Recipe(ingrients);
        recipe.SetAmount("A", expected);

        // Assert
        Assert.Equal(expected, recipe.GetAmount("A"));
    }

    [Fact]
    public void Recipe_IncreaseAmount_ShouldIncreaseAmountForIngredientByOne()
    {
        // Arrange
        var ingrients = new Ingredient[]
        {
            new Ingredient("A", 0, 0, 0, 0, 0),
        };

        // Act
        var recipe = new Recipe(ingrients);
        recipe.SetAmount("A", 13);
        recipe.IncreaseAmount("A");

        // Assert
        Assert.Equal(14u, recipe.GetAmount("A"));
    }

    [Fact]
    public void Recipe_DecreaseAmount_ShouldDecreaseAmountForIngredientByOne()
    {
        // Arrange
        var ingrients = new Ingredient[]
        {
            new Ingredient("A", 0, 0, 0, 0, 0),
        };

        // Act
        var recipe = new Recipe(ingrients);
        recipe.SetAmount("A", 13);
        recipe.DecreaseAmount("A");

        // Assert
        Assert.Equal(12u, recipe.GetAmount("A"));
    }

    private static Ingredient[] GetTestIngredients()
    {
        return new Ingredient[]
        {
            new Ingredient(Name: "Butterscotch", Capacity: -1, Durability: -2, Flavor: 6, Texture: 3, Calories: 8),
            new Ingredient(Name: "Cinnamon", Capacity: 2, Durability: 3, Flavor: -2, Texture: -1, Calories: 3),
        };
    }

    [Theory]
    [InlineData(44, 56, 68)]
    [InlineData(80, 20, 0)]
    private void Recipe_SumCapacity_ShouldGetTheSumOfCapacityAllIngredientsForTheSetAmounts(uint i0, uint i1, uint expected)
    {
        // Arrange
        var ingredients = GetTestIngredients();
        var recipe = new Recipe(ingredients);

        recipe.SetAmount("Butterscotch", i0);
        recipe.SetAmount("Cinnamon", i1);

        // Act
        var actual = recipe.SumCapacity();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(44, 56, 80)]
    private void Recipe_SumDurability_ShouldGetTheSumOfDurabilityAllIngredientsForTheSetAmounts(uint i0, uint i1, uint expected)
    {
        // Arrange
        var ingredients = GetTestIngredients();
        var recipe = new Recipe(ingredients);

        recipe.SetAmount("Butterscotch", i0);
        recipe.SetAmount("Cinnamon", i1);

        // Act
        var actual = recipe.SumDurability();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(44, 56, 152)]
    private void Recipe_SumFlavor_ShouldGetTheSumOfFlavorAllIngredientsForTheSetAmounts(uint i0, uint i1, uint expected)
    {
        // Arrange
        var ingredients = GetTestIngredients();
        var recipe = new Recipe(ingredients);

        recipe.SetAmount("Butterscotch", i0);
        recipe.SetAmount("Cinnamon", i1);

        // Act
        var actual = recipe.SumFlavor();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(44, 56, 76)]
    private void Recipe_SumTexture_ShouldGetTheSumOfTextureAllIngredientsForTheSetAmounts(uint i0, uint i1, uint expected)
    {
        // Arrange
        var ingredients = GetTestIngredients();
        var recipe = new Recipe(ingredients);

        recipe.SetAmount("Butterscotch", i0);
        recipe.SetAmount("Cinnamon", i1);

        // Act
        var actual = recipe.SumTexture();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(40, 60, 500)]
    private void Recipe_SumCalories_ShouldGetTheSumOfCaloriesAllIngredientsForTheSetAmounts(uint i0, uint i1, uint expected)
    {
        // Arrange
        var ingredients = GetTestIngredients();
        var recipe = new Recipe(ingredients);

        recipe.SetAmount("Butterscotch", i0);
        recipe.SetAmount("Cinnamon", i1);

        // Act
        var actual = recipe.SumCalories();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(44, 56, 62842880)]
    [InlineData(80, 20, 0)]
    private void Recipe_RecipeScore_ShouldGetTheScore(uint i0, uint i1, uint expected)
    {
        // Arrange
        var ingredients = GetTestIngredients();
        var recipe = new Recipe(ingredients);

        recipe.SetAmount("Butterscotch", i0);
        recipe.SetAmount("Cinnamon", i1);

        // Act
        var actual = recipe.RecipeScore();

        // Assert
        Assert.Equal(expected, actual);
    }
}
