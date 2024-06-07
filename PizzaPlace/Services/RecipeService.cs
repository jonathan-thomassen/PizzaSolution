using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;

namespace PizzaPlace.Services
{
    public class RecipeService(IRecipeRepository recipeRepository) : IRecipeService
    {
        public async Task<ComparableList<PizzaRecipe>> GetPizzaRecipes(PizzaOrder order)
        {
            List<PizzaRecipeType> pizzaTypes = order.RequestedOrder
                .Select(x => x.PizzaType)
                .Distinct()
                .ToList();

            ComparableList<PizzaRecipe> recipes = [];
            foreach (PizzaRecipeType pizzaType in pizzaTypes)
            {
                PizzaRecipe? recipe = await recipeRepository.GetRecipe(pizzaType);
                if (recipe != null)
                {
                    recipes.Add(recipe);
                }
            }

            return recipes;
        }

        public async Task<long> AddPizzaRecipe(PizzaRecipe recipe)
        {
            long result = await recipeRepository.AddRecipe(recipe);

            return result;
        }

        public async Task<long> UpdatePizzaRecipe(
            PizzaRecipe recipe, long id)
        {
            long result = await recipeRepository.UpdateRecipe(recipe, id);

            return result;
        }
    }
}
