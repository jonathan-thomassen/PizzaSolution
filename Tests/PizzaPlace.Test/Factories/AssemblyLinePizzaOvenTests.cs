using PizzaPlace.Factories;
using PizzaPlace.Models;
using PizzaPlace.Pizzas;
using PizzaPlace.Test.TestExtensions;

namespace PizzaPlace.Test.Factories;

[TestClass]
public class AssemblyLinePizzaOvenTests
{
    private static AssemblyLinePizzaOven GetOven(TimeProvider timeProvider) => new(timeProvider);

    [TestMethod]
    public async Task PreparePizzas_OnePizza()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        var order = new ComparableList<PizzaPrepareOrder>
         {
             new PizzaPrepareOrder(NormalPizzaOvenTests.GetTestStandardPizzaRecipe(), 1),
         };
        var stock = NormalPizzaOvenTests.GetPlentyStock();

        var oven = GetOven(timeProvider);
        var expectedTime = NormalPizzaOvenTests.StandardPizzaPrepareTime + AssemblyLinePizzaOven.SetupTimeMinutes;
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
    public async Task PreparePizzas_23_OfTheSameTypePizza()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        ComparableList<PizzaPrepareOrder> order =
        [
            new PizzaPrepareOrder(NormalPizzaOvenTests.GetTestTastyPizzaRecipe(), 23),
         ];
        var stock = NormalPizzaOvenTests.GetPlentyStock();

        var oven = GetOven(timeProvider);
        // NormalPizzaOvenTests.TastyPizzaPrepareTime + AssemblyLinePizzaOven.SetupTimeMinutes = 22
        // AssemblyLinePizzaOven.SubsequentPizzaTimeSavingsInMinutes = 5
        // AssemblyLinePizzaOven.MinimumCookingTimeMinutes = 4
        var expectedTime = new[] {22, 17, 12, 7, 4,
             4, 4, 4, 4, 4,
             4, 4, 4, 4, 4,
             4, 4, 4, 4, 4,
             4, 4, 4}.Sum();
        var expectedPizzas = 23;

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
        Assert.IsTrue(pizzas.All(x => x is ExtremelyTastyPizza), "Only tasty pizzas");
    }

    [TestMethod]
    public async Task PreparePizzas_MixedPizzaTypes()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        ComparableList<PizzaPrepareOrder> order =
        [
            new PizzaPrepareOrder(NormalPizzaOvenTests.GetTestTastyPizzaRecipe(), 10),
             new PizzaPrepareOrder(NormalPizzaOvenTests.GetTestStandardPizzaRecipe(), 7),
         ];
        var stock = NormalPizzaOvenTests.GetPlentyStock();

        var oven = GetOven(timeProvider);
        var expectedTime = new[] {22, 17, 12, 7, 4,
             4, 4, 4, 4, 4,
             17, 12, 7, 4, 4,
             4, 4, }.Sum();
        var expectedPizzas = 17;

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
        Assert.AreEqual(10, pizzas.Count(x => x is ExtremelyTastyPizza));
        Assert.AreEqual(7, pizzas.Count(x => x is StandardPizza));
    }
}
