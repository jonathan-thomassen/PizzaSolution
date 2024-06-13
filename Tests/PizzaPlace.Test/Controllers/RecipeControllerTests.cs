using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PizzaPlace.Controllers;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Controllers;

[TestClass]
public class RecipeControllerTests
{
    private static RecipeController GetController(Mock<IRecipeService> recipeService) =>
        new(recipeService.Object);

    [TestMethod]
    public async Task AddRecipe()
    {
        // Arrange
        RecipeDto recipe = new(PizzaRecipeType.StandardPizza, [new(StockType.Dough, 1)], 12);

        Mock<IRecipeRepository> recipeRepository = new(MockBehavior.Strict);
        recipeRepository.Setup(x => x.AddRecipe(recipe)).ReturnsAsync(recipe.Id);

        Mock<IRecipeService> recipeService = new Mock<IRecipeService>(MockBehavior.Strict);
        recipeService.Setup(x => x.AddPizzaRecipe(recipe)).ReturnsAsync(recipe.Id);

        RecipeController controller = GetController(recipeService);

        // Act
        IActionResult actual = await controller.AddRecipe(recipe);

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(actual);
        recipeService.VerifyAll();
    }

    [TestMethod]
    public async Task UpdateRecipe()
    {
        // Arrange
        RecipeDto oldRecipe = new(PizzaRecipeType.StandardPizza, [new(StockType.Dough, 1)], 12);

        RecipeDto newRecipe = new(PizzaRecipeType.StandardPizza,
                                    [new(StockType.Dough, 1), new(StockType.Tomatoes, 1)],
                                    12);

        Mock<IRecipeRepository> recipeRepository = new(MockBehavior.Strict);
        recipeRepository.Setup(x => x.AddRecipe(oldRecipe))
            .ReturnsAsync(oldRecipe.Id);
        recipeRepository.Setup(x => x.UpdateRecipe(newRecipe, oldRecipe.Id))
            .ReturnsAsync(oldRecipe.Id);

        Mock<IRecipeService> recipeService = new(MockBehavior.Strict);
        recipeService.Setup(x => x.AddPizzaRecipe(oldRecipe))
            .ReturnsAsync(oldRecipe.Id);
        recipeService.Setup(x => x.UpdatePizzaRecipe(newRecipe, oldRecipe.Id))
            .ReturnsAsync(oldRecipe.Id);

        RecipeController controller = GetController(recipeService);

        // Act
        await controller.AddRecipe(oldRecipe);
        IActionResult actual = await controller.UpdateRecipe(newRecipe, oldRecipe.Id);

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(actual);
        recipeService.VerifyAll();
    }
}
