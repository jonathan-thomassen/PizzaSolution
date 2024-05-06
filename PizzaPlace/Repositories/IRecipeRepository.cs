using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories;

public interface IRecipeRepository
{
    Task<long> AddRecipe(PizzaRecipeDto recipe);
    Task<PizzaRecipeDto> GetRecipe(PizzaRecipeType recipeType);
}
