﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            PizzaRecipe recipeDto = new(
                Models.Types.PizzaRecipeType.StandardPizza,
                [new Stock(Models.Types.StockType.Dough, 1),
                    new Stock(Models.Types.StockType.Tomatoes, 1)],
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

            PizzaRecipe recipeDto = new(
                Models.Types.PizzaRecipeType.StandardPizza,
                [new Stock(Models.Types.StockType.Dough, 1),
                    new Stock(Models.Types.StockType.Tomatoes, 1)],
                12);

            ComparableList<Stock> expected = [];

            Mock<IStockRepository> stockRepository = new();

            StockService service = GetService(stockRepository);

            // Act
            ComparableList<Stock> actual = await service.GetStock(order, [recipeDto]);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
