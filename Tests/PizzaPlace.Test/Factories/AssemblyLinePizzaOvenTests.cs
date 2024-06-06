using Microsoft.Extensions.Time.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzaPlace.Factories;
using PizzaPlace.Models;
using PizzaPlace.Pizzas;
using PizzaPlace.Test.TestExtensions;

namespace PizzaPlace.Test.Factories
{
    [TestClass]
    public class AssemblyLinePizzaOvenTests
    {
        private static AssemblyLinePizzaOven GetOven(TimeProvider timeProvider)
            => new(timeProvider);

        [TestMethod]
        public async Task PreparePizzas_OnePizza()
        {
            // Arrange
            FakeTimeProvider timeProvider = new();
            ComparableList<PizzaPrepareOrder> order =
            [
                new(NormalPizzaOvenTests.GetTestStandardPizzaRecipe(), 1)
            ];
            ComparableList<StockDto> stock = NormalPizzaOvenTests.GetPlentyStock();

            AssemblyLinePizzaOven oven = GetOven(timeProvider);
            int expectedTime = NormalPizzaOvenTests.StandardPizzaPrepareTime +
                AssemblyLinePizzaOven.SetupTimeMinutes;
            int expectedPizzas = 1;

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
                new(NormalPizzaOvenTests.GetTestTastyPizzaRecipe(), 23),
        ];
            var stock = NormalPizzaOvenTests.GetPlentyStock();

            var oven = GetOven(timeProvider);

            int expectedTime = 134;
            int expectedPizzas = 23;

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
            Assert.IsTrue(pizzas.All(x => x is ExtremelyTastyPizza), "Only tasty pizzas");
        }

        [TestMethod]
        public async Task PreparePizzas_MixedPizzaTypes()
        {
            // Arrange
            FakeTimeProvider timeProvider = new();
            ComparableList<PizzaPrepareOrder> order =
            [
                new(NormalPizzaOvenTests.GetTestTastyPizzaRecipe(), 10),
            new(NormalPizzaOvenTests.GetTestStandardPizzaRecipe(), 7)
            ];
            ComparableList<StockDto> stock = NormalPizzaOvenTests.GetPlentyStock();

            AssemblyLinePizzaOven oven = GetOven(timeProvider);
            int expectedTime = 134;
            int expectedPizzas = 17;

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
            Assert.AreEqual(10, pizzas.Count(x => x is ExtremelyTastyPizza));
            Assert.AreEqual(7, pizzas.Count(x => x is StandardPizza));
        }
    }
}
