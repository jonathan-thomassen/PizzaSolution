using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Services;

namespace PizzaPlace.Controllers
{
    [Route("api/recipe")]
    public class RecipeController(IRecipeService recipeService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddRecipe([FromBody] RecipeDto recipe)
        {
            return Ok(await recipeService.AddPizzaRecipe(recipe));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRecipe([FromBody] RecipeDto recipe, long id)
        {
            return Ok(await recipeService.UpdatePizzaRecipe(recipe, id));
        }
    }
}
