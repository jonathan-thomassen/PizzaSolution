using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PizzaPlace.Factories;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Pizzas;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Services
{
    [TestClass]
    public class OrderingServiceTests
    {
        private static OrderingService GetService(
            IStockService stockService, IRecipeService recipeService, IPizzaOven pizzaOven) =>
            new(stockService, recipeService, pizzaOven);

        [TestMethod]
        public async Task HandlePizzaOrder()
        {
            // Arrange
            ComparableList<PizzaAmount> requests =
            [
                new PizzaAmount(PizzaRecipeType.StandardPizza, 54),
            new PizzaAmount(PizzaRecipeType.ExtremelyTastyPizza, 2),
            new PizzaAmount(PizzaRecipeType.StandardPizza, 0),
            new PizzaAmount(PizzaRecipeType.StandardPizza, 4),
        ];
            var order = new PizzaOrder(requests);
            var standardRecipe = new Recipe(PizzaRecipeType.StandardPizza,
                [
                    new Ingredient(IngredientType.Dough, 2),
                new Ingredient(IngredientType.Tomatoes, 1),
            ], 10);
            var tastyRecipe = new Recipe(PizzaRecipeType.ExtremelyTastyPizza,
                [
                    new Ingredient(IngredientType.UnicornDust, 1),
                new Ingredient(IngredientType.BellPeppers, 2),
            ], 15);
            ComparableList<Recipe> recipes = [standardRecipe, tastyRecipe];
            ComparableList<Ingredient> returnedStock = [new Ingredient(IngredientType.Anchovies, 2)]; // Doesn't matter that it doesn't match recipes.
            ComparableList<PizzaPrepareOrder> prepareOrders =
            [
                new PizzaPrepareOrder(standardRecipe, 58),
            new PizzaPrepareOrder(tastyRecipe, 2),
        ];

            var pizzas = new List<Pizza>{
            new StandardPizza(),
            new ExtremelyTastyPizza(),
        };

            var stockService = new Mock<IStockService>(MockBehavior.Strict);
            var recipeService = new Mock<IRecipeService>(MockBehavior.Strict);
            var pizzaOven = new Mock<IPizzaOven>(MockBehavior.Strict);

            recipeService.Setup(x => x.GetPizzaRecipes(order))
                .ReturnsAsync(recipes);
            stockService.Setup(x => x.HasInsufficientStock(order, recipes))
                .ReturnsAsync(false);
            stockService.Setup(x => x.GetStock(order, recipes))
                .ReturnsAsync(returnedStock);
            pizzaOven.Setup(x => x.PreparePizzas(prepareOrders, returnedStock))
                .ReturnsAsync(pizzas);

            var service = GetService(stockService.Object, recipeService.Object, pizzaOven.Object);

            // Act
            var actual = await service.HandlePizzaOrder(order);

            // Assert
            CollectionAssert.AreEqual(pizzas, actual.ToList());
            stockService.VerifyAll();
            recipeService.VerifyAll();
            pizzaOven.VerifyAll();
        }

        [TestMethod]
        public async Task HandlePizzaOrder_InsufficientStock()
        {
            // Arrange
            ComparableList<PizzaAmount> requests =
            [
                new PizzaAmount(PizzaRecipeType.ExtremelyTastyPizza, 2),
        ];
            var order = new PizzaOrder(requests);
            var tastyRecipe = new Recipe(PizzaRecipeType.ExtremelyTastyPizza,
                [
                    new Ingredient(IngredientType.UnicornDust, 1),
                new Ingredient(IngredientType.BellPeppers, 2),
            ], 15);
            ComparableList<Recipe> recipes = [tastyRecipe];

            var stockService = new Mock<IStockService>(MockBehavior.Strict);
            var recipeService = new Mock<IRecipeService>(MockBehavior.Strict);
            var pizzaOven = new Mock<IPizzaOven>(MockBehavior.Strict);

            recipeService.Setup(x => x.GetPizzaRecipes(order))
                .ReturnsAsync(recipes);
            stockService.Setup(x => x.HasInsufficientStock(order, recipes))
                .ReturnsAsync(true);

            var service = GetService(stockService.Object, recipeService.Object, pizzaOven.Object);

            // Act
            var ex = await Assert.ThrowsExceptionAsync<PizzaException>(() => service.HandlePizzaOrder(order));

            // Assert
            Assert.AreEqual("Unable to take in order. Insufficient stock.", ex.Message);
            stockService.VerifyAll();
            recipeService.VerifyAll();
            pizzaOven.VerifyAll();
        }

        [TestMethod]
        public async Task HandlePizzaOrder_BrokenRecipeServiceDoesNotReturnRecipe()
        {
            // Arrange
            ComparableList<PizzaAmount> requests =
            [
                new PizzaAmount(PizzaRecipeType.StandardPizza, 1),
        ];
            var order = new PizzaOrder(requests);
            ComparableList<Recipe> recipes = [];
            ComparableList<Ingredient> returnedStock = [new Ingredient(IngredientType.Anchovies, 2)]; // Doesn't matter that it doesn't match recipes.

            var stockService = new Mock<IStockService>(MockBehavior.Strict);
            var recipeService = new Mock<IRecipeService>(MockBehavior.Strict);
            var pizzaOven = new Mock<IPizzaOven>(MockBehavior.Strict);

            recipeService.Setup(x => x.GetPizzaRecipes(order))
                .ReturnsAsync(recipes);
            stockService.Setup(x => x.HasInsufficientStock(order, recipes))
                .ReturnsAsync(false);
            stockService.Setup(x => x.GetStock(order, recipes))
                .ReturnsAsync(returnedStock);

            var service = GetService(stockService.Object, recipeService.Object, pizzaOven.Object);

            // Act
            var ex = await Assert.ThrowsExceptionAsync<PizzaException>(() => service.HandlePizzaOrder(order));

            // Assert
            Assert.AreEqual("Missing recipe. Recipe service did not return a recipe for StandardPizza which was expected.", ex.Message);
            stockService.VerifyAll();
            recipeService.VerifyAll();
            pizzaOven.VerifyAll();
        }
    }
}
