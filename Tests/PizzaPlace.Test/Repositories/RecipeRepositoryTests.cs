using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;

namespace PizzaPlace.Test.Repositories;

[TestClass]
public class RecipeRepositoryTests
{
    private static RecipeRepository GetRecipeRepository() => new();

    private static ComparableList<Stock> GetStandardIngredients() =>
    [
        new Stock(StockType.Dough, 3),
        new Stock(StockType.Tomatoes, 10),
        new Stock(StockType.Bacon, 2),
    ];
    private const int StandardCookingTime = 19;

    [TestMethod]
    public async Task AddRecipe()
    {
        // Arrange
        var recipe = new Recipe(PizzaRecipeType.StandardPizza,
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
        Recipe oldRecipe = new(
            PizzaRecipeType.StandardPizza,
            GetStandardIngredients(),
            StandardCookingTime);

        RecipeRepository repository = GetRecipeRepository();
        await repository.AddRecipe(oldRecipe);

        Recipe newRecipe = new(
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
            "The instance of entity type 'Recipe' cannot be tracked because another instance " +
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

        Recipe recipe = new(
            PizzaRecipeType.StandardPizza,
            GetStandardIngredients(),
            StandardCookingTime);
        RecipeRepository repository = GetRecipeRepository();

        await repository.AddRecipe(recipe);

        // Act
        Recipe? actual = await repository.GetRecipe(pizzaType);

        // Assert
        Assert.AreEqual(recipe, actual);
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
