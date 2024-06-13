using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories
{
    public interface IRecipeRepository
    {
        Task<long> AddRecipe(RecipeDto recipe);
        Task<long> UpdateRecipe(RecipeDto recipe, long id);
        Task<RecipeDto?> GetRecipe(PizzaRecipeType recipeType);
    }
}
