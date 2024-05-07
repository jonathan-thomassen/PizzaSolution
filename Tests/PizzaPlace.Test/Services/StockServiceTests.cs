using PizzaPlace.Models;
using PizzaPlace.Repositories;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Services;

[TestClass]
public class StockServiceTests
{
    private static StockService GetService(Mock<IStockRepository> stockRepository) =>
        new(stockRepository.Object);

    [TestMethod]
    public async Task HasInsufficientStock()
    {
        // Arrange
        var pAmount = new PizzaAmount(Models.Types.PizzaRecipeType.StandardPizza, 2);
        var order = new PizzaOrder(new ComparableList<PizzaAmount>() { pAmount });

        var recipeDto = new PizzaRecipeDto(Models.Types.PizzaRecipeType.StandardPizza, new ComparableList<StockDto>() { new StockDto(Models.Types.StockType.Dough, 1), new StockDto(Models.Types.StockType.Tomatoes, 1) }, 12);

        bool expected = true;

        var stockRepository = new Mock<IStockRepository>();

        var service = GetService(stockRepository);

        // Act
        var actual = await service.HasInsufficientStock(order, new ComparableList<PizzaRecipeDto>() { recipeDto });

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task GetStock()
    {
        // Arrange
        var pAmount = new PizzaAmount(Models.Types.PizzaRecipeType.StandardPizza, 2);
        var order = new PizzaOrder(new ComparableList<PizzaAmount>() { pAmount });

        var recipeDto = new PizzaRecipeDto(Models.Types.PizzaRecipeType.StandardPizza, new ComparableList<StockDto>() { new StockDto(Models.Types.StockType.Dough, 1), new StockDto(Models.Types.StockType.Tomatoes, 1) }, 12);

        ComparableList<StockDto> expected = [];

        var stockRepository = new Mock<IStockRepository>();

        var service = GetService(stockRepository);

        // Act
        var actual = await service.GetStock(order, new ComparableList<PizzaRecipeDto>() { recipeDto });

        // Assert
        Assert.AreEqual(expected, actual);
    }
}
