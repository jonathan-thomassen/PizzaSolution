using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories;

public class RecipeRepository : IRecipeRepository
{
    public Task<long> AddRecipe(PizzaRecipeDto recipe) => throw new NotImplementedException("A real repository must be implemented.");
    public Task<PizzaRecipeDto> GetRecipe(PizzaRecipeType recipeType) => throw new NotImplementedException("A real repository must be implemented.");
}
