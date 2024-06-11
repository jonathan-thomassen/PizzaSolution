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
        ComparableList<Ingredient> stock =
        [
            new(IngredientType.Dough, 1),
            new(IngredientType.Tomatoes, 2),
            new(IngredientType.GratedCheese, 1),
            new(IngredientType.GenericSpices, 1),
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
        ComparableList<Ingredient> stock =
        [
            new(IngredientType.Dough, 10),
            new(IngredientType.Tomatoes, 20),
            new(IngredientType.GratedCheese, 10),
            new(IngredientType.GenericSpices, 10),
            new(IngredientType.FermentedDough, 3),
            new(IngredientType.RottenTomatoes, 10),
            new(IngredientType.Bacon, 3)
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
        var stock = new ComparableList<Ingredient>
        {
            new(IngredientType.Dough, 10),
            new(IngredientType.Tomatoes, 20),
            new(IngredientType.GratedCheese, 10),
            new(IngredientType.GenericSpices, 10),
            new(IngredientType.FermentedDough, 3),
            new(IngredientType.RottenTomatoes, 10),
            new(IngredientType.Bacon, 2),
        };

        var oven = GetOven(timeProvider);

        // Act
        PizzaException ex = await Assert.ThrowsExceptionAsync<PizzaException>(() => oven.PreparePizzas(order, stock));

        // Assert
        Assert.AreEqual("Not enough ingredients to create all pizzas.", ex.Message);
    }

    public static Recipe GetTestStandardPizzaRecipe() =>
        new(PizzaRecipeType.StandardPizza, [
                new(IngredientType.Dough, 1),
                new(IngredientType.Tomatoes, 2),
                new(IngredientType.GratedCheese, 1),
                new(IngredientType.GenericSpices, 1)
            ], StandardPizzaPrepareTime);

    public static Recipe GetTestTastyPizzaRecipe() =>
        new(PizzaRecipeType.ExtremelyTastyPizza, [
                new(IngredientType.FermentedDough, 1),
                new(IngredientType.RottenTomatoes, 2),
                new(IngredientType.Bacon, 1),
                new(IngredientType.GenericSpices, 1)
            ], TastyPizzaPrepareTime);

    public static ComparableList<Ingredient> GetPlentyStock() =>
        new(Enum.GetValues<IngredientType>().Select(type => new Ingredient(type, int.MaxValue)));
}
