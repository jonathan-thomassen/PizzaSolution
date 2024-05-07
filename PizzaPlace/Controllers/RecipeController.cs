using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Services;

namespace PizzaPlace.Controllers;

[Route("api/recipe")]
public class RecipeController(IRecipeService recipeService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddRecipe([FromBody] PizzaRecipeDto recipe)
    {
        return Ok(recipeService.AddPizzaRecipe(recipe));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRecipe([FromBody] PizzaRecipeDto recipe, long id)
    {
        return Ok(recipeService.UpdatePizzaRecipe(recipe, id));
    }
}