using Microsoft.EntityFrameworkCore;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private static readonly PizzaContext s_dbContext = new();
        private static readonly object s_lock = new();

        public async Task<long> AddRecipe(Recipe recipe)
        {
            await s_dbContext.AddAsync(recipe);
            await s_dbContext.SaveChangesAsync();

            return recipe.Id;
        }

        public async Task<long> UpdateRecipe(Recipe recipe, long id)
        {
            Recipe? oldRecipe = await s_dbContext.FindAsync<Recipe>(id);
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

        public async Task<Recipe?> GetRecipe(PizzaRecipeType recipeType)
        {
            Recipe? recipe =
                await s_dbContext.Recipes.FirstOrDefaultAsync(r => r.RecipeType == recipeType);

            return recipe;
        }
    }
}
