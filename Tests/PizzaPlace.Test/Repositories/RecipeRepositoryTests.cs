using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;

namespace PizzaPlace.Test.Repositories;

[TestClass]
public class RecipeRepositoryTests
{
    private static RecipeRepository GetRecipeRepository() => new();

    private static ComparableList<IngredientDto> GetStandardIngredients() =>
    [
        new IngredientDto(StockType.Dough, 3),
        new IngredientDto(StockType.Tomatoes, 10),
        new IngredientDto(StockType.Bacon, 2),
    ];
    private const int StandardCookingTime = 19;

    [TestMethod]
    public async Task AddRecipe()
    {
        // Arrange
        RecipeDto recipe = new(
            PizzaRecipeType.StandardPizza,
            GetStandardIngredients(),
            StandardCookingTime);
        RecipeRepository repository = GetRecipeRepository();

        // Act
        long actual = await repository.AddRecipe(recipe);

        // Assert
        Assert.IsTrue(actual > 0, "Recipe has an id.");

        // Cleanup
        await repository.DeleteRecipe(recipe.Id);
    }

    [TestMethod]
    public async Task AddRecipe_AlreadyAdded()
    {
        // Arrange
        RecipeDto oldRecipe = new(
            PizzaRecipeType.StandardPizza,
            GetStandardIngredients(),
            StandardCookingTime);

        RecipeRepository repository = GetRecipeRepository();
        await repository.AddRecipe(oldRecipe);

        RecipeDto newRecipe = new(
            PizzaRecipeType.StandardPizza,
            [new(StockType.UnicornDust, 123), new(StockType.Anchovies, 1)],
            StandardCookingTime)
        {
            Id = oldRecipe.Id
        };

        // Act
        InvalidOperationException ex =
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
            repository.AddRecipe(newRecipe));

        // Assert
        Assert.AreEqual(
            "The instance of entity type 'RecipeDto' cannot be tracked because another instance " +
            "with the same key value for {'Id'} is already being tracked. When attaching " +
            "existing entities, ensure that only one entity instance with a given key value is " +
            "attached. Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to " +
            "see the conflicting key values.", ex.Message);

        // Cleanup
        await repository.DeleteRecipe(oldRecipe.Id);
    }

    [TestMethod]
    public async Task GetRecipe()
    {
        // Arrange
        PizzaRecipeType pizzaType = PizzaRecipeType.StandardPizza;

        RecipeDto recipe = new(
            PizzaRecipeType.StandardPizza,
            GetStandardIngredients(),
            StandardCookingTime);
        RecipeRepository repository = GetRecipeRepository();

        await repository.AddRecipe(recipe);

        // Act
        RecipeDto? actual = await repository.GetRecipe(pizzaType);

        // Assert
        Assert.AreEqual(recipe.RecipeType, actual?.RecipeType);
        Assert.AreEqual(recipe.IngredientDtos, actual?.IngredientDtos);

        // Cleanup
        await repository.DeleteRecipe(recipe.Id);
    }

    [TestMethod]
    public async Task GetRecipe_DoesNotExist()
    {
        // Arrange
        PizzaRecipeType pizzaType = PizzaRecipeType.ExtremelyTastyPizza;
        RecipeRepository repository = GetRecipeRepository();

        // Act
        PizzaException ex = await Assert
            .ThrowsExceptionAsync<PizzaException>(() => repository.GetRecipe(pizzaType));

        // Assert
        Assert.AreEqual("Recipe does not exists of type ExtremelyTastyPizza.", ex.Message);
    }
}
