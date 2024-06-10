using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories
{
    public interface IRecipeRepository
    {
        Task<long> AddRecipe(Recipe recipe);
        Task<long> UpdateRecipe(Recipe recipe, long id);
        Task<Recipe?> GetRecipe(PizzaRecipeType recipeType);
    }
}
