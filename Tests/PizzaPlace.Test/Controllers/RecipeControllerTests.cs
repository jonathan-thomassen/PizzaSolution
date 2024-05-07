using Microsoft.AspNetCore.Mvc;
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
        var recipe = new PizzaRecipeDto(PizzaRecipeType.StandardPizza, new ComparableList<StockDto>() { new StockDto(StockType.Dough, 1)}, 12);

        var recipeRepository = new Mock<IRecipeRepository>(MockBehavior.Strict);
        recipeRepository.Setup(x => x.AddRecipe(recipe)).ReturnsAsync(recipe.Id);

        var recipeService = new Mock<IRecipeService>(MockBehavior.Strict);
        recipeService.Setup(x => x.AddPizzaRecipe(recipe)).ReturnsAsync(recipe.Id);

        var controller = GetController(recipeService);

        // Act
        var actual = await controller.AddRecipe(recipe);

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(actual);
        recipeService.VerifyAll();
    }

    [TestMethod]
    public async Task UpdateRecipe()
    {
        // Arrange
        var oldRecipe = new PizzaRecipeDto(PizzaRecipeType.StandardPizza, new ComparableList<StockDto>() { new StockDto(StockType.Dough, 1) }, 12);

        var newRecipe = new PizzaRecipeDto(PizzaRecipeType.StandardPizza, new ComparableList<StockDto>() { new StockDto(StockType.Dough, 1), new StockDto(StockType.Tomatoes, 1) }, 12);


        var recipeRepository = new Mock<IRecipeRepository>(MockBehavior.Strict);
        recipeRepository.Setup(x => x.AddRecipe(oldRecipe)).ReturnsAsync(oldRecipe.Id);
        recipeRepository.Setup(x => x.UpdateRecipe(newRecipe, oldRecipe.Id)).ReturnsAsync(oldRecipe.Id);

        var recipeService = new Mock<IRecipeService>(MockBehavior.Strict);
        recipeService.Setup(x => x.AddPizzaRecipe(oldRecipe)).ReturnsAsync(oldRecipe.Id);
        recipeService.Setup(x => x.UpdatePizzaRecipe(newRecipe, oldRecipe.Id)).ReturnsAsync(oldRecipe.Id);

        var controller = GetController(recipeService);

        // Act
        await controller.AddRecipe(oldRecipe);
        var actual = await controller.UpdateRecipe(newRecipe, oldRecipe.Id);

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(actual);
        recipeService.VerifyAll();
    }
}
