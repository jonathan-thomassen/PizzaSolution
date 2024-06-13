using PizzaPlace.Models;

namespace PizzaPlace.Services
{
    public interface IRecipeService
    {
        Task<ComparableList<RecipeDto>> GetPizzaRecipes(PizzaOrder order);
        Task<long> AddPizzaRecipe(RecipeDto recipe);
        Task<long> UpdatePizzaRecipe(RecipeDto recipe, long id);
    }
}
