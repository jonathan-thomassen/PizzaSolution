using PizzaPlace.Models;

namespace PizzaPlace.Services
{
    public interface IRecipeService
    {
        Task<ComparableList<PizzaRecipeDto>> GetPizzaRecipes(PizzaOrder order);
        Task<long> AddPizzaRecipe(PizzaRecipeDto recipe);
        Task<long> UpdatePizzaRecipe(PizzaRecipeDto recipe, long id);
    }
}
