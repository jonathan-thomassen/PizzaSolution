﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Services
{
    [TestClass]
    public class RecipeServiceTests
    {
        private static RecipeService GetService(Mock<IRecipeRepository> recipeRepository) =>
            new(recipeRepository.Object);

        [TestMethod]
        public async Task GetPizzaRecipes()
        {
            // Arrange
            PizzaOrder order = new([
                new PizzaAmount(PizzaRecipeType.RarePizza, 1),
                new PizzaAmount(PizzaRecipeType.OddPizza, 2),
                new PizzaAmount(PizzaRecipeType.RarePizza, 20),
            ]);
            RecipeDto rareRecipe = new(
                PizzaRecipeType.RarePizza,
                [new StockDto(StockType.UnicornDust, 1)],
                1);
            RecipeDto oddRecipe = new(
                PizzaRecipeType.OddPizza,
                [new StockDto(StockType.Sulphur, 10)],
                100);
            ComparableList<RecipeDto> expected = [rareRecipe, oddRecipe];

            Mock<IRecipeRepository> recipeRepository = new(MockBehavior.Strict);
            recipeRepository.Setup(x => x.GetRecipe(PizzaRecipeType.RarePizza))
                .ReturnsAsync(rareRecipe);
            recipeRepository.Setup(x => x.GetRecipe(PizzaRecipeType.OddPizza))
                .ReturnsAsync(oddRecipe);

            RecipeService service = GetService(recipeRepository);

            // Act
            ComparableList<RecipeDto> actual = await service.GetPizzaRecipes(order);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
