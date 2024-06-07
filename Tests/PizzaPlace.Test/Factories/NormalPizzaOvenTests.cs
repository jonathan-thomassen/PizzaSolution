using Microsoft.Extensions.Time.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzaPlace.Factories;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Pizzas;
using PizzaPlace.Test.TestExtensions;

namespace PizzaPlace.Test.Factories;

[TestClass]
public class NormalPizzaOvenTests
{
    private static NormalPizzaOven GetOven(TimeProvider timeProvider) => new(timeProvider);

    public const int StandardPizzaPrepareTime = 10;
    public const int TastyPizzaPrepareTime = 15;

    [TestMethod]
    public async Task PreparePizzas_OnePizza()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        var order = new ComparableList<PizzaPrepareOrder>
        {
            new PizzaPrepareOrder(GetTestStandardPizzaRecipe(), 1),
        };
        var stock = new ComparableList<Stock>
        {
            new Stock(StockType.Dough, 1),
            new Stock(StockType.Tomatoes, 2),
            new Stock(StockType.GratedCheese, 1),
            new Stock(StockType.GenericSpices, 1),
        };

        var oven = GetOven(timeProvider);
        var expectedTime = StandardPizzaPrepareTime;
        var expectedPizzas = 1;

        // Act
        var pizzasTask = oven.PreparePizzas(order, stock);
        timeProvider.PassTimeInMinuteIntervals(expectedTime - 1);
        var firstCheck = pizzasTask.IsCompleted;
        timeProvider.PassTimeInMinuteIntervals(1);
        var secondCheck = pizzasTask.IsCompleted;

        // Assert
        Assert.IsFalse(firstCheck);
        Assert.IsTrue(secondCheck);
        var pizzas = await pizzasTask;
        Assert.AreEqual(expectedPizzas, pizzas.Count());
        Assert.IsTrue(pizzas.All(x => x is StandardPizza), "Only standard pizzas");
    }

    [TestMethod]
    public async Task PreparePizzas_FivePizzas()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        var order = new ComparableList<PizzaPrepareOrder>
        {
            new(GetTestStandardPizzaRecipe(), 2),
            new(GetTestTastyPizzaRecipe(), 3)
        };
        var stock = new ComparableList<Stock>
        {
            new(StockType.Dough, 10),
            new(StockType.Tomatoes, 20),
            new(StockType.GratedCheese, 10),
            new(StockType.GenericSpices, 10),
            new(StockType.FermentedDough, 3),
            new(StockType.RottenTomatoes, 10),
            new(StockType.Bacon, 3)
        };

        var oven = GetOven(timeProvider);
        var expectedTime = StandardPizzaPrepareTime + TastyPizzaPrepareTime;
        var expectedPizzas = 5;
        var expectedStandard = 2;
        var expectedTasty = 3;

        // Act
        var pizzasTask = oven.PreparePizzas(order, stock);
        timeProvider.PassTimeInMinuteIntervals(expectedTime - 1);
        var firstCheck = pizzasTask.IsCompleted;
        timeProvider.PassTimeInMinuteIntervals(1);
        var secondCheck = pizzasTask.IsCompleted;

        // Assert
        Assert.IsFalse(firstCheck);
        Assert.IsTrue(secondCheck);
        var pizzas = await pizzasTask;
        Assert.AreEqual(expectedPizzas, pizzas.Count());
        Assert.AreEqual(expectedStandard, pizzas.Count(x => x is StandardPizza));
        Assert.AreEqual(expectedTasty, pizzas.Count(x => x is ExtremelyTastyPizza));
    }

    [TestMethod]
    public async Task PreparePizzas_InsufficientIngredients()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        var order = new ComparableList<PizzaPrepareOrder>
        {
            new PizzaPrepareOrder(GetTestTastyPizzaRecipe(), 3)
        };
        var stock = new ComparableList<Stock>
        {
            new Stock(StockType.Dough, 10),
            new Stock(StockType.Tomatoes, 20),
            new Stock(StockType.GratedCheese, 10),
            new Stock(StockType.GenericSpices, 10),
            new Stock(StockType.FermentedDough, 3),
            new Stock(StockType.RottenTomatoes, 10),
            new Stock(StockType.Bacon, 2),
        };

        var oven = GetOven(timeProvider);

        // Act
        var ex = await Assert.ThrowsExceptionAsync<PizzaException>(() => oven.PreparePizzas(order, stock));

        // Assert
        Assert.AreEqual("Not enough ingredients to create all pizzas.", ex.Message);
    }

    public static PizzaRecipe GetTestStandardPizzaRecipe() =>
        new PizzaRecipe(PizzaRecipeType.StandardPizza, [
                new Stock(StockType.Dough, 1),
                new Stock(StockType.Tomatoes, 2),
                new Stock(StockType.GratedCheese, 1),
                new Stock(StockType.GenericSpices, 1)
            ], StandardPizzaPrepareTime);

    public static PizzaRecipe GetTestTastyPizzaRecipe() =>
        new PizzaRecipe(PizzaRecipeType.ExtremelyTastyPizza, [
                new Stock(StockType.FermentedDough, 1),
                new Stock(StockType.RottenTomatoes, 2),
                new Stock(StockType.Bacon, 1),
                new Stock(StockType.GenericSpices, 1)
            ], TastyPizzaPrepareTime);

    public static ComparableList<Stock> GetPlentyStock() =>
        new ComparableList<Stock>(Enum.GetValues<StockType>().Select(type => new Stock(type, int.MaxValue)));
}
