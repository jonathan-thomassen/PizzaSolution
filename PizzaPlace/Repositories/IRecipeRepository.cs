using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories;

public interface IRecipeRepository
{
    Task<long> AddRecipe(PizzaRecipeDto recipe);
    Task<long> UpdateRecipe(PizzaRecipeDto recipe, long id);
    Task<PizzaRecipeDto> GetRecipe(PizzaRecipeType recipeType);
}
