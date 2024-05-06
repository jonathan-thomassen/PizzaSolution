using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories;

public class FakeRecipeRepository : FakeDatabase<PizzaRecipeDto>, IRecipeRepository
{
    private static readonly object _lock = new();

    public Task<long> AddRecipe(PizzaRecipeDto recipe)
    {
        lock (_lock)
        {
            if (Get(x => x.RecipeType == recipe.RecipeType).Any())
                throw new PizzaException($"Recipe already added for {recipe.RecipeType}.");

            var id = Insert(recipe);
            return Task.FromResult(id);
        }
    }

    public Task<PizzaRecipeDto> GetRecipe(PizzaRecipeType recipeType)
    {
        var recipe = Get(x => x.RecipeType == recipeType)
            .FirstOrDefault() ?? throw new PizzaException($"Recipe does not exists of type {recipeType}.");

        return Task.FromResult(recipe);
    }

    public void AddStandardRecipes()
    {
        if (Get(_ => true).Any())
            return;

        foreach (var recipe in GetStandardRecipes())
            AddRecipe(recipe).GetAwaiter().GetResult();

        static List<PizzaRecipeDto> GetStandardRecipes() =>
        [
            new PizzaRecipeDto(PizzaRecipeType.StandardPizza, [
                new StockDto(StockType.Dough, 1),
                new StockDto(StockType.Tomatoes, 2),
                new StockDto(StockType.GratedCheese, 1),
                new StockDto(StockType.GenericSpices, 1)
            ], 10),
            new PizzaRecipeDto(PizzaRecipeType.ExtremelyTastyPizza, [
                new StockDto(StockType.FermentedDough, 1),
                new StockDto(StockType.Tomatoes, 1),
                new StockDto(StockType.RottenTomatoes, 1),
                new StockDto(StockType.GratedCheese, 1),
                new StockDto(StockType.UngenericSpices, 1)
            ], 10),
        ];
    }
}
