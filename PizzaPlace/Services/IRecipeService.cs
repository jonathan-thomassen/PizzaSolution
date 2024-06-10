using PizzaPlace.Models;

namespace PizzaPlace.Services
{
    public interface IRecipeService
    {
        Task<ComparableList<Recipe>> GetPizzaRecipes(PizzaOrder order);
        Task<long> AddPizzaRecipe(Recipe recipe);
        Task<long> UpdatePizzaRecipe(Recipe recipe, long id);
    }
}
