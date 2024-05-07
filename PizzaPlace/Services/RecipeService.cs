using PizzaPlace.Models;
using PizzaPlace.Repositories;

namespace PizzaPlace.Services;

public class RecipeService(IRecipeRepository recipeRepository) : IRecipeService
{
    public async Task<ComparableList<PizzaRecipeDto>> GetPizzaRecipes(PizzaOrder order)
    {
        var pizzaTypes = order.RequestedOrder
            .Select(x => x.PizzaType)
            .Distinct()
            .ToList();

        ComparableList<PizzaRecipeDto> recipes = [];
        foreach (var pizzaType in pizzaTypes)
        {
            recipes.Add(await recipeRepository.GetRecipe(pizzaType));
        }

        return recipes;
    }

    public async Task<long> AddPizzaRecipe(PizzaRecipeDto recipe)
    {
        var result = await recipeRepository.AddRecipe(recipe);

        return result;
    }

    public async Task<long> UpdatePizzaRecipe(PizzaRecipeDto recipe, long id)
    {
        var result = await recipeRepository.UpdateRecipe(recipe, id);

        return result;
    }
}
