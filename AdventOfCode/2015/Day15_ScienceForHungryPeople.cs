using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Challenge(2015, 15)]
internal class Day15_ScienceForHungryPeople
{
    private Ingredient[] _ingredients = Array.Empty<Ingredient>();

    [Setup]
    public void Setup()
    {
        var filePath = ChallengeHelper.GetResourceFilePath();
        var text = File.ReadAllText(filePath);
        _ingredients = GetIngredients(text);
    }

    private static Ingredient[] GetIngredients(string text)
    {
        var ingredients = new List<Ingredient>();

        var matches = Regex.Matches(text, "(?<name>\\w+): capacity (?<capacity>-?\\d+), durability (?<durability>-?\\d+), flavor (?<flavor>-?\\d+), texture (?<texture>-?\\d+), calories (?<calories>\\d+)");
        foreach (var match in matches.Cast<Match>())
        {
            var ingredient = new Ingredient
            {
                Name = match.Groups["name"].Value,
                Capacity = match.Groups["capacity"].ToInt32(),
                Durability = match.Groups["durability"].ToInt32(),
                Flavor = match.Groups["flavor"].ToInt32(),
                Texture = match.Groups["texture"].ToInt32(),
                Calories = match.Groups["calories"].ToInt32()
            };

            ingredients.Add(ingredient);
        }

        return ingredients.ToArray();
    }

    [Part(1)]
    public void Part01()
    {
        var recipe = new Recipe(_ingredients);
        foreach (var ingredient in _ingredients)
            recipe.IncreaseAmount(ingredient.Name);

        for (int i = 0; i < 96; i++) 
        {
            var highestScore = 0u;
            Ingredient bestIngredient = _ingredients[0]; 
            foreach (var ingredient in _ingredients)
            {
                recipe.IncreaseAmount(ingredient.Name);
                var score = recipe.RecipeScore();
                recipe.DecreaseAmount(ingredient.Name);

                if (score > highestScore)
                {
                    highestScore = score;
                    bestIngredient = ingredient;
                }
            }

            recipe.IncreaseAmount(bestIngredient.Name);
        }

        var recipeScore = recipe.RecipeScore();
        Console.WriteLine("Highest recipe score: {0}", recipeScore);
        Console.WriteLine(recipe);
    }

    [Part(1)]
    public void Part02()
    {
        var recipe = new Recipe(_ingredients);

        uint highestScore = 0;
        Backtrack(4, 100, a => 
        {
            Console.WriteLine(string.Join(", ", a));
            var (score, calories) = GetRecipeScore(recipe, a);
            if (calories == 500 && score > highestScore)
            {
                highestScore = score;
            }
        });

        Console.WriteLine("Highest recipe score: {0}", highestScore);
    }

    private static (uint score, uint calories) GetRecipeScore(Recipe recipe, uint[] amounts)
    {
        var ingredients = recipe.Ingredients;
        for (int i = 0; i < amounts.Length; i++)
        {
            var amount = amounts[i];
            var ingredient = ingredients[i];

            recipe.SetAmount(ingredient.Name, amount);
        }

        var score = recipe.RecipeScore();
        var calories = recipe.SumCalories();

        return (score, calories);
    }

    private void Backtrack(int n, int k, Action<uint[]> action, uint curr = 0, int index = 0, uint[]? list = null)
    {
        list ??= new uint[n];

        if (curr == k)
        {
            action(list);
            return;
        }

        if (index >= n)
            return;

        var prev = index == 0 ? 0u : list[index];

        for (uint i = prev; i <= k; i++)
        {
            list[index] = i;
            Backtrack(n, k, action, curr + i, index + 1, list);
            list[index] = 0;
        }
    }

    public record struct Ingredient(string Name, int Capacity, int Durability, int Flavor, int Texture, int Calories);

    public class Recipe
    {
        private readonly IDictionary<string, Ingredient> _ingredients;
        private readonly IDictionary<string, uint> _ingredientAmounts;

        public Recipe(Ingredient[] ingredients)
        {
            _ingredients = ingredients.ToDictionary(i => i.Name, i => i);
            _ingredientAmounts = _ingredients.Keys.ToDictionary(i => i, _ => 0u);
        }

        public Ingredient[] Ingredients => _ingredients.Values.ToArray();

        public uint GetAmount(string ingredient) => _ingredientAmounts[ingredient];
        public void SetAmount(string ingredient, uint amount)
        {
            if (_ingredientAmounts.ContainsKey(ingredient))
                _ingredientAmounts[ingredient] = amount;
        }

        public void IncreaseAmount(string ingredient)
        {
            if (_ingredientAmounts.TryGetValue(ingredient, out uint current))
                _ingredientAmounts[ingredient] = ++current;
        }

        public void DecreaseAmount(string ingredient)
        {
            if (_ingredientAmounts.TryGetValue(ingredient, out uint current))
                _ingredientAmounts[ingredient] = --current;
        }

        public uint SumCapacity() => SumProperty(i => i.Capacity);
        public uint SumDurability() => SumProperty(i => i.Durability);
        public uint SumFlavor() => SumProperty(i => i.Flavor);
        public uint SumTexture() => SumProperty(i => i.Texture);
        public uint SumCalories() => SumProperty(i => i.Calories);

        private uint SumProperty(Func<Ingredient, int> propertyGetter)
        {
            long sum = 0;
            foreach (var ingredientNames in _ingredients.Keys)
            {
                var ingredient = _ingredients[ingredientNames];
                var amount = _ingredientAmounts[ingredientNames];

                sum += amount * propertyGetter(ingredient);
            }

            return sum > 0 ? (uint)sum : 0;
        }

        public uint RecipeScore()
        {
            var capacity = SumCapacity();
            var durability = SumDurability();
            var flavor = SumFlavor();
            var texture = SumTexture();

            return capacity * durability * flavor * texture;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("Recipe: { ");
            bool first = true;
            foreach (var amount in _ingredientAmounts)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(", ");
                }

                sb.Append($"'{amount.Key}': {amount.Value}");
            }
            sb.Append(" }");

            return sb.ToString();
        }
    }
}
