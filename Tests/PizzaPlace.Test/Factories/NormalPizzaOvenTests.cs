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
        FakeTimeProvider timeProvider = new();
        ComparableList<PizzaPrepareOrder> order =
        [
            new(GetTestStandardPizzaRecipe(), 1),
        ];
        ComparableList<StockDto> stock =
        [
            new(StockType.Dough, 1),
            new(StockType.Tomatoes, 2),
            new(StockType.GratedCheese, 1),
            new(StockType.GenericSpices, 1),
        ];

        NormalPizzaOven oven = GetOven(timeProvider);
        int expectedTime = StandardPizzaPrepareTime;
        int expectedPizzas = 1;

        // Act
        Task<IEnumerable<Pizza>> pizzasTask = oven.PreparePizzas(order, stock);
        timeProvider.PassTimeInMinuteIntervals(expectedTime - 1);
        bool firstCheck = pizzasTask.IsCompleted;
        timeProvider.PassTimeInMinuteIntervals(1);
        bool secondCheck = pizzasTask.IsCompleted;

        // Assert
        Assert.IsFalse(firstCheck);
        Assert.IsTrue(secondCheck);
        IEnumerable<Pizza> pizzas = await pizzasTask;
        Assert.AreEqual(expectedPizzas, pizzas.Count());
        Assert.IsTrue(pizzas.All(x => x is StandardPizza), "Only standard pizzas");
    }

    [TestMethod]
    public async Task PreparePizzas_FivePizzas()
    {
        // Arrange
        FakeTimeProvider timeProvider = new();
        ComparableList<PizzaPrepareOrder> order =
        [
            new(GetTestStandardPizzaRecipe(), 2),
            new(GetTestTastyPizzaRecipe(), 3)
        ];
        ComparableList<StockDto> stock =
        [
            new(StockType.Dough, 10),
            new(StockType.Tomatoes, 20),
            new(StockType.GratedCheese, 10),
            new(StockType.GenericSpices, 10),
            new(StockType.FermentedDough, 3),
            new(StockType.RottenTomatoes, 10),
            new(StockType.Bacon, 3)
        ];

        NormalPizzaOven oven = GetOven(timeProvider);
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
        IEnumerable<Pizza> pizzas = await pizzasTask;
        Assert.AreEqual(expectedPizzas, pizzas.Count());
        Assert.AreEqual(expectedStandard, pizzas.Count(x => x is StandardPizza));
        Assert.AreEqual(expectedTasty, pizzas.Count(x => x is ExtremelyTastyPizza));
    }

    [TestMethod]
    public async Task PreparePizzas_InsufficientIngredients()
    {
        // Arrange
        FakeTimeProvider timeProvider = new();
        ComparableList<PizzaPrepareOrder> order =
        [
            new(GetTestTastyPizzaRecipe(), 3)
        ];
        var stock = new ComparableList<StockDto>
        {
            new(StockType.Dough, 10),
            new(StockType.Tomatoes, 20),
            new(StockType.GratedCheese, 10),
            new(StockType.GenericSpices, 10),
            new(StockType.FermentedDough, 3),
            new(StockType.RottenTomatoes, 10),
            new(StockType.Bacon, 2),
        };

        var oven = GetOven(timeProvider);

        // Act
        PizzaException ex = await Assert.ThrowsExceptionAsync<PizzaException>(() => oven.PreparePizzas(order, stock));

        // Assert
        Assert.AreEqual("Not enough ingredients to create all pizzas.", ex.Message);
    }

    public static RecipeDto GetTestStandardPizzaRecipe() =>
        new(PizzaRecipeType.StandardPizza, [
                new(StockType.Dough, 1),
                new(StockType.Tomatoes, 2),
                new(StockType.GratedCheese, 1),
                new(StockType.GenericSpices, 1)
            ], StandardPizzaPrepareTime);

    public static RecipeDto GetTestTastyPizzaRecipe() =>
        new(PizzaRecipeType.ExtremelyTastyPizza, [
                new(StockType.FermentedDough, 1),
                new(StockType.RottenTomatoes, 2),
                new(StockType.Bacon, 1),
                new(StockType.GenericSpices, 1)
            ], TastyPizzaPrepareTime);

    public static ComparableList<StockDto> GetPlentyStock() =>
        new(Enum.GetValues<StockType>().Select(type => new StockDto(type, int.MaxValue)));
}
