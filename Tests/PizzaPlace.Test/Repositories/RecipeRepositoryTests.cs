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
    private static long StandardRecipeId { get; set; }

    [TestMethod]
    public async Task AddRecipe()
    {
        // Arrange
        if (StandardRecipeId > 0)
            return;

        var recipe = new PizzaRecipe(PizzaRecipeType.StandardPizza,
                                        GetStandardIngredients(),
                                        StandardCookingTime);
        var repository = GetRecipeRepository();

        // Act
        var actual = await repository.AddRecipe(recipe);

        // Assert
        Assert.IsTrue(actual > 0, "Recipe has an id.");
        StandardRecipeId = actual;
    }

    [TestMethod]
    public async Task AddRecipe_AlreadyAdded()
    {
        // Arrange
        await AddRecipe();
        var recipe = new PizzaRecipe(PizzaRecipeType.StandardPizza, [new Stock(StockType.UnicornDust, 123), new Stock(StockType.Anchovies, 1)], StandardCookingTime);
        var repository = GetRecipeRepository();

        // Act
        var ex = await Assert.ThrowsExceptionAsync<PizzaException>(() => repository.AddRecipe(recipe));

        // Assert
        Assert.AreEqual("Recipe already added for StandardPizza.", ex.Message);
    }

    [TestMethod]
    public async Task GetRecipe()
    {
        // Arrange
        var pizzaType = PizzaRecipeType.StandardPizza;
        await AddRecipe();
        var expected = new PizzaRecipe(pizzaType, GetStandardIngredients(), StandardCookingTime, StandardRecipeId);
        var repository = GetRecipeRepository();

        // Act
        var actual = await repository.GetRecipe(pizzaType);

        // Assert
        Assert.AreEqual(expected, actual);
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
