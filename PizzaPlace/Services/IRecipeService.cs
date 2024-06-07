using PizzaPlace.Models;

namespace PizzaPlace.Services
{
    public interface IRecipeService
    {
        Task<ComparableList<PizzaRecipe>> GetPizzaRecipes(PizzaOrder order);
        Task<long> AddPizzaRecipe(PizzaRecipe recipe);
        Task<long> UpdatePizzaRecipe(PizzaRecipe recipe, long id);
    }
}
