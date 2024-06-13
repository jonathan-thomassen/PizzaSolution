using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;

namespace PizzaPlace.Test.Repositories;

[TestClass]
public class RecipeRepositoryTests
{
    private static RecipeRepository GetRecipeRepository() => new();

    private static ComparableList<StockDto> GetStandardIngredients() =>
    [
        new StockDto(StockType.Dough, 3),
        new StockDto(StockType.Tomatoes, 10),
        new StockDto(StockType.Bacon, 2),
    ];
    private const int StandardCookingTime = 19;

    [TestMethod]
    public async Task AddRecipe()
    {
        // Arrange
        var recipe = new RecipeDto(PizzaRecipeType.StandardPizza,
                                        GetStandardIngredients(),
                                        StandardCookingTime);
        var repository = GetRecipeRepository();

        // Act
        long actual = await repository.AddRecipe(recipe);

        // Assert
        Assert.IsTrue(actual > 0, "Recipe has an id.");
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
            StandardCookingTime);
        newRecipe.Id = oldRecipe.Id;

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
        Assert.AreEqual(recipe.RecipeType, actual.RecipeType);
        Assert.AreEqual(recipe.StockDto, actual.StockDto);
    }

    [TestMethod]
    public async Task GetRecipe_DoesNotExist()
    {
        // Arrange
        var pizzaType = PizzaRecipeType.ExtremelyTastyPizza;
        var repository = GetRecipeRepository();

        // Act
        var ex = await Assert.ThrowsExceptionAsync<PizzaException>(() => repository.GetRecipe(pizzaType));

        // Assert
        Assert.AreEqual("Recipe does not exists of type ExtremelyTastyPizza.", ex.Message);
    }
}
