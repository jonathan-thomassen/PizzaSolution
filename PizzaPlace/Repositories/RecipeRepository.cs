using Microsoft.EntityFrameworkCore;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private static readonly PizzaContext s_dbContext = new();
        private static readonly object s_lock = new();

        public async Task<long> AddRecipe(RecipeDto recipe)
        {
            await s_dbContext.AddAsync(recipe);
            await s_dbContext.SaveChangesAsync();

            return recipe.Id;
        }

        public async Task<long> UpdateRecipe(RecipeDto recipe, long id)
        {
            RecipeDto? oldRecipe = await s_dbContext.FindAsync<RecipeDto>(id);
            if (oldRecipe != null)
            {
                lock (s_lock)
                {
                    s_dbContext.Entry(oldRecipe).CurrentValues.SetValues(recipe);
                }
            }
            await s_dbContext.SaveChangesAsync();

            return id;
        }

        public async Task<RecipeDto?> GetRecipe(PizzaRecipeType recipeType)
        {
            RecipeDto? recipe =
                await s_dbContext.RecipeDtos.FirstOrDefaultAsync(r => r.RecipeType == recipeType);

            return recipe;
        }
    }
}
