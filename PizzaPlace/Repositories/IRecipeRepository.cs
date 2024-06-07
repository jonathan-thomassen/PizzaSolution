using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories
{
    public interface IRecipeRepository
    {
        Task<long> AddRecipe(PizzaRecipe recipe);
        Task<long> UpdateRecipe(PizzaRecipe recipe, long id);
        Task<PizzaRecipe?> GetRecipe(PizzaRecipeType recipeType);
    }
}
