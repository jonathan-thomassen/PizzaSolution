using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PizzaPlace.Models;
using PizzaPlace.Repositories;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Services
{
    [TestClass]
    public class StockServiceTests
    {
        private static StockService GetService(
            Mock<IStockRepository> stockRepository) => new(stockRepository.Object);

        [TestMethod]
        public async Task HasInsufficientStock()
        {
            // Arrange
            PizzaAmount pAmount = new(Models.Types.PizzaRecipeType.StandardPizza, 2);
            PizzaOrder order = new([pAmount]);

            Recipe recipeDto = new(
                Models.Types.PizzaRecipeType.StandardPizza,
                [new Ingredient(Models.Types.IngredientType.Dough, 1),
                    new Ingredient(Models.Types.IngredientType.Tomatoes, 1)],
                12);

            bool expected = true;

            Mock<IStockRepository> stockRepository = new();

            StockService service = GetService(stockRepository);

            // Act
            bool actual = await service.HasInsufficientStock(order, [recipeDto]);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task GetStock()
        {
            // Arrange
            PizzaAmount pAmount = new(Models.Types.PizzaRecipeType.StandardPizza, 2);
            PizzaOrder order = new([pAmount]);

            Recipe recipeDto = new(
                Models.Types.PizzaRecipeType.StandardPizza,
                [new Ingredient(Models.Types.IngredientType.Dough, 1),
                    new Ingredient(Models.Types.IngredientType.Tomatoes, 1)],
                12);

            ComparableList<Ingredient> expected = [];

            Mock<IStockRepository> stockRepository = new();

            StockService service = GetService(stockRepository);

            // Act
            ComparableList<Ingredient> actual = await service.GetStock(order, [recipeDto]);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
